using System.Collections.ObjectModel;
using Cassandra.EntityFrameworkCore.Storage.ChangeTracker;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cassandra.EntityFrameworkCore.Storage;

public class CassandraTypeMappingSource(TypeMappingSourceDependencies dependencies)
    : TypeMappingSource(dependencies)
{
    private static readonly Type[] SupportedCollectionInterfaces =
    [
        typeof(IList<>),
        typeof(IReadOnlyList<>),
        typeof(IEnumerable<>)
    ];

    private static readonly Type[] SupportedDictionaryInterfaces =
    [
        typeof(IDictionary<,>),
        typeof(IReadOnlyDictionary<,>)
    ];

    protected override CoreTypeMapping? FindMapping(in TypeMappingInfo mappingInfo)
    {
        if (mappingInfo.ClrType == null)
            throw new InvalidOperationException($"Unable to determine CLR type for mappingInfo '{mappingInfo}'");

        return FindPrimitiveMapping(mappingInfo)
               ?? FindCollectionMapping(mappingInfo)
               ?? base.FindMapping(mappingInfo);
    }

    private CassandraTypeMapping? FindPrimitiveMapping(in TypeMappingInfo mappingInfo)
    {
        var clrType = mappingInfo.ClrType!;
        if (clrType is { IsValueType: true } || clrType == typeof(string)) return new CassandraTypeMapping(clrType);

        return null;
    }

    private CassandraTypeMapping? FindCollectionMapping(in TypeMappingInfo mappingInfo)
    {
        var clrType = mappingInfo.ClrType!;
        if (mappingInfo.ElementTypeMapping != null) return null;

        var elementType = clrType.TryGetItemType();
        if (elementType == null) return null;

        if (clrType.IsArray) return CreateCollectionTypeMapping(clrType, elementType);

        if (clrType is { IsGenericType: true, IsGenericTypeDefinition: false })
        {
            if (clrType.HasInterface(SupportedDictionaryInterfaces)) return CreateDictionaryTypeMapping(clrType);

            if (clrType.HasInterface(SupportedCollectionInterfaces))
                return CreateCollectionTypeMapping(clrType, elementType);
        }

        return null;
    }

    private CassandraTypeMapping? CreateCollectionTypeMapping(Type collectionType, Type elementType)
    {
        var elementMappingInfo = new TypeMappingInfo(elementType);
        var elementMapping = FindMapping(elementMappingInfo);
        return elementMapping == null
            ? null
            : new CassandraTypeMapping(collectionType,
                CreateCollectionComparer(elementMapping, collectionType, elementType));
    }

    private static ValueComparer? CreateCollectionComparer(
        CoreTypeMapping elementMapping,
        Type collectionType,
        Type elementType)
    {
        var typeToInstantiate = FindCollectionTypeToInstantiate(collectionType, elementType);

        return (ValueComparer?)Activator.CreateInstance(
            elementType.IsNullableValueType()
                ? typeof(ListOfNullableValueTypesComparer<,>).MakeGenericType(typeToInstantiate,
                    elementType.UnwrapNullableType())
                : elementType.IsValueType
                    ? typeof(ListOfValueTypesComparer<,>).MakeGenericType(typeToInstantiate, elementType)
                    : typeof(ListOfReferenceTypesComparer<,>).MakeGenericType(typeToInstantiate, elementType),
            elementMapping.Comparer.ToNullableComparer(elementType)!);
    }

    private static Type FindCollectionTypeToInstantiate(Type collectionType, Type elementType)
    {
        if (collectionType.IsArray) return collectionType;

        var listOfT = typeof(List<>).MakeGenericType(elementType);

        if (collectionType.IsAssignableFrom(listOfT))
        {
            if (!collectionType.IsAbstract)
            {
                var constructor = collectionType.GetDeclaredConstructor(null);
                if (constructor?.IsPublic == true) return collectionType;
            }

            return listOfT;
        }

        return collectionType;
    }

    private CassandraTypeMapping? CreateDictionaryTypeMapping(Type dictionaryType)
    {
        var genericArguments = dictionaryType.GenericTypeArguments;
        if (genericArguments[0] != typeof(string)) return null;

        var elementType = genericArguments[1];
        var elementMappingInfo = new TypeMappingInfo(elementType);
        var elementMapping = FindPrimitiveMapping(elementMappingInfo)
                             ?? FindCollectionMapping(elementMappingInfo);

        var isReadOnly = dictionaryType.GetGenericTypeDefinition() == typeof(ReadOnlyDictionary<,>);

        return elementMapping == null
            ? null
            : new CassandraTypeMapping(
                dictionaryType,
                CreateStringDictionaryComparer(elementMapping, elementType, dictionaryType, isReadOnly));
    }

    private static ValueComparer CreateStringDictionaryComparer(
        CoreTypeMapping elementMapping,
        Type elementType,
        Type dictType,
        bool readOnly = false)
    {
        var unwrappedType = elementType.UnwrapNullableType();

        return (ValueComparer)Activator.CreateInstance(
            elementType == unwrappedType
                ? typeof(StringDictionaryComparer<,>).MakeGenericType(elementType, dictType)
                : typeof(NullableStringDictionaryComparer<,>).MakeGenericType(unwrappedType, dictType),
            elementMapping.Comparer,
            readOnly)!;
    }
}