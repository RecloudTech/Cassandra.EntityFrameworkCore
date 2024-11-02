using Cassandra.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Cassandra.EntityFrameworkCore.Extensions;

public static class CassandraEntityExtensions
{
    public static string GetCollectionName(this IReadOnlyEntityType entityType)
    {
        return entityType.BaseType != null
            ? entityType.GetRootType().GetCollectionName()
            : (string?)entityType[CassandraAnnotationNames.CollectionName]
              ?? GetDefaultCollectionName(entityType);
    }

    public static string GetDefaultCollectionName(this IReadOnlyEntityType entityType)
    {
        return entityType.HasSharedClrType ? entityType.ShortName() : entityType.ClrType.ShortDisplayName();
    }
}