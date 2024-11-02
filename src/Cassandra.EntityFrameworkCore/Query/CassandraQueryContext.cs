using Microsoft.EntityFrameworkCore.Query;

namespace Cassandra.EntityFrameworkCore.Query;

public class CassandraQueryContext : QueryContext
{
    public CassandraQueryContext(QueryContextDependencies dependencies) : base(dependencies)
    {
    }
}