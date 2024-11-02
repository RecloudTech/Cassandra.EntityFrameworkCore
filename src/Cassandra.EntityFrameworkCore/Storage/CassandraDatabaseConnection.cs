using Cassandra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Cassandra.EntityFrameworkCore.Storage;

public class CassandraDatabaseConnection(ICurrentDbContext currentDbContext, string? connectionString) : CqlConnection
{
    private readonly ICurrentDbContext _currentDbContext = currentDbContext;
    private CassandraOptionsExtension _cassandraOptionsExtension = new();
    private CassandraConnectionStringBuilder _connectionStringBuilder = new(connectionString);
}