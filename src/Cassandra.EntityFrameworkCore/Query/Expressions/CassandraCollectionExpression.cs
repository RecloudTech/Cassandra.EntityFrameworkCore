using System.Linq.Expressions;
using Cassandra.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Cassandra.EntityFrameworkCore.Query.Expressions;

public class CassandraCollectionExpression : EntityTypedExpression
{
    public CassandraCollectionExpression(IEntityType entityType)
        : base(entityType)
    {
        CollectionName = entityType.GetCollectionName();
    }

    public string CollectionName { get; }

    /// <inheritdoc />
    protected override Expression VisitChildren(ExpressionVisitor visitor)
    {
        return this;
    }
}