using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Cassandra.EntityFramework.Tests;

public class CassandraDbContextOptionsExtensionsTest
{
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