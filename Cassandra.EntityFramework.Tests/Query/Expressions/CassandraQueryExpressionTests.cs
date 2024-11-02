using Cassandra.EntityFrameworkCore.Extensions;
using Cassandra.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Cassandra.EntityFramework.Tests.Query.Expressions;

public class CassandraQueryExpressionTests
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

    public class BooksContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Shelf> Shelves { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseCassandra("Contact Points=127.0.0.1;", "Messages")
                .ConfigureWarnings(x => x.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));
        }
    }

    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int ShelfId { get; set; }
        public Shelf Shelf { get; set; }
    }

    public class Shelf
    {
        public int ShelfId { get; set; }
        public string Location { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}