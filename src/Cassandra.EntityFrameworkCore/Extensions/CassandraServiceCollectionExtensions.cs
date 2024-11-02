using Cassandra.EntityFrameworkCore.Diagnostics;
using Cassandra.EntityFrameworkCore.Infrastructure;
using Cassandra.EntityFrameworkCore.Query.Factories;
using Cassandra.EntityFrameworkCore.Query.Visitors.Dependencies;
using Cassandra.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore;

public static class CassandraServiceCollectionExtensions
{
    public static IServiceCollection AddCassandra<TContext>(
        this IServiceCollection serviceCollection,
        string connectionString,
        string defaultKeyspace,
        Action<CassandraDbContextOptionsBuilder> cassandraOptionsAction = null)
        where TContext : DbContext
    {
        return serviceCollection.AddDbContext<TContext>(
            (_, options) => { options.UseCassandra(connectionString, defaultKeyspace, cassandraOptionsAction); });
    }

    public static IServiceCollection AddEntityFrameworkCassandra(this IServiceCollection serviceCollection)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        new EntityFrameworkServicesBuilder(serviceCollection)
            .TryAdd<LoggingDefinitions, CassandraLoggingDefinitions>()
            .TryAdd<IDatabase, CassandraDatabaseWrapper>()
            .TryAdd<IDatabaseProvider, DatabaseProvider<CassandraOptionsExtension>>()
            .TryAdd<IDatabaseCreator, CassandraDatabaseCreator>()
            .TryAdd<IQueryContextFactory, CassandraQueryContextFactory>()
            .TryAdd<ITypeMappingSource, CassandraTypeMappingSource>()
            .TryAdd<IShapedQueryCompilingExpressionVisitorFactory,
                CassandraShapedQueryCompilingExpressionVisitorFactory>()
            .TryAdd<IQueryableMethodTranslatingExpressionVisitorFactory,
                CassandraQueryableMethodTranslatingExpressionVisitorFactory>()
            .TryAddProviderSpecificServices(
                b => b
                    .TryAddSingleton<CassandraShapedQueryCompilingExpressionVisitorDependencies,
                        CassandraShapedQueryCompilingExpressionVisitorDependencies>()
            )

            // .TryAdd<IDbContextTransactionManager, MongoTransactionManager>()
            // .TryAdd<IModelValidator, MongoModelValidator>()
            // .TryAdd<IProviderConventionSetBuilder, MongoConventionSetBuilder>()
            // .TryAdd<IValueGeneratorSelector, MongoValueGeneratorSelector>()
            // .TryAdd<IValueConverterSelector, MongoValueConverterSelector>()
            // .TryAdd<IQueryTranslationPreprocessorFactory, MongoQueryTranslationPreprocessorFactory>()
            // .TryAdd<IQueryCompilationContextFactory, MongoQueryCompilationContextFactory>()
            // .TryAdd<IQueryTranslationPostprocessorFactory, MongoQueryTranslationPostprocessorFactory>()

            // .TryAdd<IModelRuntimeInitializer, MongoModelRuntimeInitializer>()
            .TryAddCoreServices();

        return serviceCollection;
    }
}