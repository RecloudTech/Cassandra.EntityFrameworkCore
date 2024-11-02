using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Cassandra.EntityFrameworkCore.Query.Visitors;

public class CassandraProjectionBindingExpressionVisitor : ExpressionVisitor
{
    private readonly Dictionary<ProjectionMember, Expression> _projectionMapping = new();
    private readonly Stack<ProjectionMember> _projectionMembers = new();
    private bool _clientEval;
    private SelectExpression _queryExpression;

    public virtual Expression Translate(SelectExpression selectExpression, Expression expression)
    {
        _queryExpression = selectExpression;

        _projectionMembers.Push(new ProjectionMember());

        var result = Visit(expression);

        _queryExpression.ReplaceProjection(_projectionMapping);
        _projectionMapping.Clear();
        _queryExpression = null;

        _projectionMembers.Clear();

        return MatchTypes(result, expression.Type);
    }

    private static Expression MatchTypes(
        Expression expression,
        Type targetType)
    {
        return targetType != expression.Type && targetType.TryGetItemType() == null
            ? Expression.Convert(expression, targetType)
            : expression;
    }
}