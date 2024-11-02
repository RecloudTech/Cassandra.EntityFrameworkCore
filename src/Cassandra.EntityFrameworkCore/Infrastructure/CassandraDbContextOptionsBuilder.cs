using Microsoft.EntityFrameworkCore;

namespace Cassandra.EntityFrameworkCore.Infrastructure;

public class CassandraDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
    : ICassandraDbContextOptionsBuilderInfrastructure
{
    protected virtual DbContextOptionsBuilder OptionsBuilder { get; } = optionsBuilder;
    
    DbContextOptionsBuilder ICassandraDbContextOptionsBuilderInfrastructure.OptionsBuilder => OptionsBuilder;
}