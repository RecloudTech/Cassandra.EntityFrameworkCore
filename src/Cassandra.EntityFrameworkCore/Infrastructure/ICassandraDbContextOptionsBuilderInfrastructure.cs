using Microsoft.EntityFrameworkCore;

namespace Cassandra.EntityFrameworkCore.Infrastructure;

public interface ICassandraDbContextOptionsBuilderInfrastructure
{
    DbContextOptionsBuilder OptionsBuilder { get; }
}