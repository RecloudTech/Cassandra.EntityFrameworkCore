using Microsoft.EntityFrameworkCore.Query;

namespace Cassandra.EntityFrameworkCore.Query.Factories;

public class CassandraQueryContextFactory : IQueryContextFactory
{
    public CassandraQueryContextFactory(
        QueryContextDependencies dependencies)
    {
        Dependencies = dependencies;
    }

    protected virtual QueryContextDependencies Dependencies { get; }

    public virtual QueryContext Create()
    {
        return new CassandraQueryContext(Dependencies);
    }
}