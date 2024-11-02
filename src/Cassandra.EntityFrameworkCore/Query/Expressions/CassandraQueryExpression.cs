using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Cassandra.EntityFrameworkCore.Query.Expressions;

public class CassandraQueryExpression : Expression
{
    private readonly List<ProjectionExpression> _projection = [];
    private Dictionary<ProjectionMember, Expression> _projectionMapping = new();

    public CassandraQueryExpression(IEntityType entityType)
    {
        CollectionExpression = new CassandraCollectionExpression(entityType);
        // _projectionMapping[new ProjectionMember()] =
        //     new EntityProjectionExpression(entityType, new RootReferenceExpression(entityType));
    }

    public CassandraCollectionExpression CollectionExpression { get; private set; }

    public Expression? CapturedExpression { get; set; }

    /// <inheritdoc />
    public override Type Type
        => typeof(object);

    /// <inheritdoc />
    public override ExpressionType NodeType
        => ExpressionType.Extension;

    public IReadOnlyList<ProjectionExpression> Projection
        => _projection;

    public int AddToProjection(Expression expression, string? alias = null)
    {
        // var existingIndex = _projection.FindIndex(pe => pe.Expression.Equals(expression));
        // if (existingIndex != -1)
        // {
        //     return existingIndex;
        // }
        //
        // var baseAlias = alias ?? (expression as IAccessExpression)?.Name;
        //
        // var currentAlias = baseAlias;
        // var counter = 0;
        // while (_projection.Any(pe => string.Equals(pe.Alias, currentAlias, StringComparison.OrdinalIgnoreCase)))
        // {
        //     currentAlias = $"{baseAlias}{counter++}";
        // }
        //
        // _projection.Add(new ProjectionExpression(expression, currentAlias, false));

        return _projection.Count - 1;
    }

    public Expression GetMappedProjection(ProjectionMember projectionMember)
    {
        return _projectionMapping[projectionMember];
    }

    public void ApplyProjection()
    {
        if (Projection.Any()) return;

        Dictionary<ProjectionMember, Expression> result = new();
        foreach (var (projectionMember, expression) in _projectionMapping)
            result[projectionMember] = Constant(AddToProjection(expression, projectionMember.Last?.Name));

        _projectionMapping = result;
    }

    public void ReplaceProjectionMapping(IDictionary<ProjectionMember, Expression> projectionMapping)
    {
        _projectionMapping.Clear();
        foreach (var (projectionMember, expression) in projectionMapping)
            _projectionMapping[projectionMember] = expression;
    }
}