using System.Linq.Expressions;
using Cassandra.EntityFrameworkCore.Query.Visitors.Dependencies;
using Microsoft.EntityFrameworkCore.Query;

namespace Cassandra.EntityFrameworkCore.Query.Visitors;

public class CassandraShapedQueryCompilingExpressionVisitor : ShapedQueryCompilingExpressionVisitor
{
    private readonly Type _contextType;
    private readonly bool _threadSafetyChecksEnabled;

    public CassandraShapedQueryCompilingExpressionVisitor(
        ShapedQueryCompilingExpressionVisitorDependencies dependencies,
        CassandraShapedQueryCompilingExpressionVisitorDependencies cassandraDependencies,
        QueryCompilationContext queryCompilationContext) : base(dependencies, queryCompilationContext)
    {
        _contextType = queryCompilationContext.ContextType;
        _threadSafetyChecksEnabled = dependencies.CoreSingletonOptions.AreThreadSafetyChecksEnabled;
    }

    protected override Expression VisitShapedQuery(ShapedQueryExpression shapedQueryExpression)
    {
        throw new NotImplementedException();
    }
}