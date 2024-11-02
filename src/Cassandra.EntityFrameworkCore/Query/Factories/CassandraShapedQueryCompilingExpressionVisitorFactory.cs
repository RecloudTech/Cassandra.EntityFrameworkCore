using Cassandra.EntityFrameworkCore.Query.Visitors;
using Cassandra.EntityFrameworkCore.Query.Visitors.Dependencies;
using Microsoft.EntityFrameworkCore.Query;

namespace Cassandra.EntityFrameworkCore.Query.Factories;

public class CassandraShapedQueryCompilingExpressionVisitorFactory : IShapedQueryCompilingExpressionVisitorFactory
{
    public CassandraShapedQueryCompilingExpressionVisitorFactory(
        ShapedQueryCompilingExpressionVisitorDependencies dependencies,
        CassandraShapedQueryCompilingExpressionVisitorDependencies cassandraDependencies)
    {
        Dependencies = dependencies;
        CassandraDependencies = cassandraDependencies;
    }

    public CassandraShapedQueryCompilingExpressionVisitorDependencies CassandraDependencies { get; set; }

    public ShapedQueryCompilingExpressionVisitorDependencies Dependencies { get; set; }

    public ShapedQueryCompilingExpressionVisitor Create(QueryCompilationContext queryCompilationContext)
    {
        return new CassandraShapedQueryCompilingExpressionVisitor(
            Dependencies,
            CassandraDependencies,
            (CassandraQueryCompilationContext)queryCompilationContext);
    }
}