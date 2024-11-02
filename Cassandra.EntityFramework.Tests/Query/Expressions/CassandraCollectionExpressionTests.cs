using Cassandra.EntityFramework.Tests.Query.Expressions.Data;
using Cassandra.EntityFrameworkCore.Extensions;
using Cassandra.EntityFrameworkCore.Query.Expressions;

namespace Cassandra.EntityFramework.Tests.Query.Expressions;

public class CassandraCollectionExpressionTests
{
    [Fact]
    public static void Can_set_properties_from_constructor()
    {
        using var db = new BooksContext();

        foreach (var entityType in db.Model.GetEntityTypes())
        {
            var actual = new CassandraCollectionExpression(entityType);
            Assert.Equal(entityType, actual.EntityType);
            Assert.Equal(entityType.GetCollectionName(), actual.CollectionName);
        }
    }
}