using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.TestModels.ConferencePlanner;
using Microsoft.Extensions.DependencyInjection;

namespace Cassandra.EntityFramework.Tests;

public class CassandraDbContextOptionsExtensionsTest
{
    [Theory]
    [InlineData("Contact Points=127.0.0.1;", "Messages")]
    public static void Can_configure_with_mongo_client_and_database_name(string connectionString, string keySpace)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddCassandra<ApplicationDbContext>(connectionString, keySpace, _ => { });

        var services = serviceCollection.BuildServiceProvider(true);
        using var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var cassandraOptions = serviceScope.ServiceProvider
            .GetRequiredService<DbContextOptions<ApplicationDbContext>>().GetExtension<CassandraOptionsExtension>();

        // Assert.Equal(mongoClient, cassandraOptions.ClusterBuilder);
        Assert.Equal(keySpace, cassandraOptions.DefaultKeyspace);
    }

    [Theory]
    [InlineData("Contact Points=127.0.0.1;", "Messages")]
    public static void Throws_when_multiple_ef_providers_specified(string connectionString, string keySpace)
    {
        var options = new DbContextOptionsBuilder()
            .UseCassandra(connectionString, keySpace)
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning))
            .Options;

        var context = new DbContext(options);

        var message = Assert.Throws<InvalidOperationException>(() => context.Model).Message;

        Assert.Contains(
            "Only a single database provider can be registered", message);
    }
}