using Cassandra.EntityFrameworkCore.Query.Visitors;
using Microsoft.EntityFrameworkCore.Query;

namespace Cassandra.EntityFrameworkCore.Query.Factories;

public class CassandraQueryableMethodTranslatingExpressionVisitorFactory(
    QueryableMethodTranslatingExpressionVisitorDependencies dependencies,
    RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies)
    : IQueryableMethodTranslatingExpressionVisitorFactory
{
    private readonly QueryableMethodTranslatingExpressionVisitorDependencies _dependencies = dependencies;

    private readonly RelationalQueryableMethodTranslatingExpressionVisitorDependencies _relationalDependencies =
        relationalDependencies;

    public virtual QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext)
    {
        return new CassandraQueryableMethodTranslatingExpressionVisitor(dependencies,
            (CassandraQueryCompilationContext)queryCompilationContext);
    }
}