using System.Data.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cassandra.EntityFrameworkCore.Storage.Internal;

public class CassandraRelationalConnection(
    ICurrentDbContext currentDbContext,
    RelationalConnectionDependencies dependencies)
    : RelationalConnection(dependencies), ICassandraRelationalConnection
{
    protected override DbConnection CreateDbConnection()
    {
        return new CassandraDatabaseConnection(currentDbContext, ConnectionString);
    }
}