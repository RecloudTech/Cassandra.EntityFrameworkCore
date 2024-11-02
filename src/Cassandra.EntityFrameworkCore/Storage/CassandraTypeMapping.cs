using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cassandra.EntityFrameworkCore.Storage;

public class CassandraTypeMapping : CoreTypeMapping
{
    public CassandraTypeMapping(Type clrType, ValueComparer? comparer = null, ValueComparer? keyComparer = null)
        : base(new CoreTypeMappingParameters(clrType, null, comparer, keyComparer))
    {
    }

    protected CassandraTypeMapping(CoreTypeMappingParameters parameters)
        : base(parameters)
    {
    }

    public static CassandraTypeMapping Default { get; } = new(typeof(object));

    protected override CoreTypeMapping Clone(CoreTypeMappingParameters parameters)
    {
        return new CassandraTypeMapping(parameters);
    }

    public override CoreTypeMapping WithComposedConverter(ValueConverter? converter, ValueComparer? comparer = null,
        ValueComparer? keyComparer = null, CoreTypeMapping? elementMapping = null,
        JsonValueReaderWriter? jsonValueReaderWriter = null)
    {
        return new CassandraTypeMapping(Parameters.WithComposedConverter(converter, comparer, keyComparer,
            elementMapping,
            jsonValueReaderWriter));
    }
}