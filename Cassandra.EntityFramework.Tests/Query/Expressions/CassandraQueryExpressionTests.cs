using Cassandra.EntityFramework.Tests.Query.Expressions.Data;
using Cassandra.EntityFrameworkCore.Query.Expressions;

namespace Cassandra.EntityFramework.Tests.Query.Expressions;

public class CassandraQueryExpressionTests
{
    [Fact]
    public static void Can_set_properties_from_constructor()
    {
        using var db = new BooksContext();
        var expectedEntityType = db.Model.GetEntityTypes().First();

        var actual = new CassandraQueryExpression(expectedEntityType);

        Assert.Equal(expectedEntityType, actual.CollectionExpression.EntityType);
    }
}