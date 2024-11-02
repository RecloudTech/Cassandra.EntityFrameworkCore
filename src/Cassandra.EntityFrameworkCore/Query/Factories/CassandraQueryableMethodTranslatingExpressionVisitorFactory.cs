using Cassandra.EntityFrameworkCore.Query.Visitors;
using Microsoft.EntityFrameworkCore.Query;

namespace Cassandra.EntityFrameworkCore.Query.Factories;

public class CassandraQueryableMethodTranslatingExpressionVisitorFactory(
    QueryableMethodTranslatingExpressionVisitorDependencies dependencies)
    : IQueryableMethodTranslatingExpressionVisitorFactory
{
    public virtual QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext)
    {
        return new CassandraQueryableMethodTranslatingExpressionVisitor(dependencies,
            (CassandraQueryCompilationContext)queryCompilationContext);
    }
}