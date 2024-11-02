using Cassandra;
using Cassandra.EntityFrameworkCore.Helpers;
using Cassandra.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore;

public static class CassandraDbContextOptionsExtensions
{
    public static DbContextOptionsBuilder UseCassandra(
        this DbContextOptionsBuilder optionsBuilder,
        string connectionString,
        string defaultKeyspace,
        Action<CassandraDbContextOptionsBuilder> cassandraOptionsAction = null)
    {
        return UseCassandra(optionsBuilder, connectionString, defaultKeyspace, options =>
        {
            options.WithQueryOptions(new QueryOptions().SetConsistencyLevel(ConsistencyLevel.LocalOne))
                .WithReconnectionPolicy(new ConstantReconnectionPolicy(1000))
                .WithRetryPolicy(new DefaultRetryPolicy())
                .WithLoadBalancingPolicy(new TokenAwarePolicy(Policies.DefaultPolicies.LoadBalancingPolicy))
                .WithDefaultKeyspace(nameof(DbContextOptionsBuilder))
                .WithPoolingOptions(
                    PoolingOptions.Create()
                        .SetMaxSimultaneousRequestsPerConnectionTreshold(HostDistance.Remote, 1_000_000)
                        .SetMaxSimultaneousRequestsPerConnectionTreshold(HostDistance.Local, 1_000_000)
                        .SetMaxConnectionsPerHost(HostDistance.Local, 1_000_000)
                        .SetMaxConnectionsPerHost(HostDistance.Remote, 1_000_000)
                        .SetMaxRequestsPerConnection(1_000_000)
                );
        }, cassandraOptionsAction);
    }

    public static DbContextOptionsBuilder UseCassandra(
        this DbContextOptionsBuilder optionsBuilder,
        string connectionString,
        string defaultKeyspace,
        Action<Builder> clusterBuilderCallback,
        Action<CassandraDbContextOptionsBuilder>? optionsAction = null)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);
        ArgumentNullException.ThrowIfNull(connectionString);
        defaultKeyspace.ThrowArgumentExceptionIfNullOrEmpty();

        var extension = (optionsBuilder.Options.FindExtension<CassandraOptionsExtension>()
                         ?? new CassandraOptionsExtension())
            .WithCallbackClusterBuilder(clusterBuilderCallback)
            .WithKeySpace(defaultKeyspace);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
        optionsAction?.Invoke(new CassandraDbContextOptionsBuilder(optionsBuilder));
        return optionsBuilder;
    }
}