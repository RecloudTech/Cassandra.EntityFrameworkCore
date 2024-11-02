using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Cassandra.EntityFrameworkCore.Query.Expressions;

public abstract class EntityTypedExpression : Expression
{
    /// <summary>
    ///     Create a <see cref="EntityTypedExpression" />.
    /// </summary>
    /// <param name="entityType">The <see cref="IEntityType" /> for this expression.</param>
    protected EntityTypedExpression(IEntityType entityType)
    {
        EntityType = entityType;
    }

    /// <summary>
    ///     The <see cref="IEntityType" /> this expression relates to.
    /// </summary>
    public IEntityType EntityType { get; }

    /// <inheritdoc />
    public override ExpressionType NodeType
        => ExpressionType.Extension;

    /// <inheritdoc />
    public override Type Type
        => EntityType.ClrType;

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj != null
               && (ReferenceEquals(this, obj)
                   || (obj is EntityTypedExpression entityTypedExpression
                       && Equals(entityTypedExpression)));
    }

    private bool Equals(EntityTypedExpression entityTypedExpression)
    {
        return Equals(EntityType, entityTypedExpression.EntityType);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(EntityType);
    }
}