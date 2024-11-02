using Cassandra.EntityFrameworkCore.Diagnostics;
using Cassandra.EntityFrameworkCore.Query.Factories;
using Cassandra.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore;

public static class CassandraServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFrameworkCassandra(this IServiceCollection serviceCollection)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        new EntityFrameworkServicesBuilder(serviceCollection)
            .TryAdd<LoggingDefinitions, CassandraLoggingDefinitions>()
            .TryAdd<IDatabaseProvider, DatabaseProvider<CassandraOptionsExtension>>()
            .TryAdd<IDatabaseCreator, CassandraDatabaseCreator>()
            .TryAdd<IQueryableMethodTranslatingExpressionVisitorFactory,
                CassandraQueryableMethodTranslatingExpressionVisitorFactory>()

            // .TryAdd<IDbContextTransactionManager, MongoTransactionManager>()
            // .TryAdd<IModelValidator, MongoModelValidator>()
            // .TryAdd<IProviderConventionSetBuilder, MongoConventionSetBuilder>()
            // .TryAdd<IValueGeneratorSelector, MongoValueGeneratorSelector>()
            // .TryAdd<IQueryContextFactory, MongoQueryContextFactory>()
            // .TryAdd<ITypeMappingSource, MongoTypeMappingSource>()
            // .TryAdd<IValueConverterSelector, MongoValueConverterSelector>()
            // .TryAdd<IQueryTranslationPreprocessorFactory, MongoQueryTranslationPreprocessorFactory>()
            // .TryAdd<IQueryCompilationContextFactory, MongoQueryCompilationContextFactory>()
            // .TryAdd<IQueryTranslationPostprocessorFactory, MongoQueryTranslationPostprocessorFactory>()
            // .TryAddProviderSpecificServices(
            //     b => b
            //         .TryAddScoped<IMongoClientWrapper, MongoClientWrapper>()
            //         .TryAddSingleton<MongoShapedQueryCompilingExpressionVisitorDependencies,
            //             MongoShapedQueryCompilingExpressionVisitorDependencies>()
            //         .TryAddSingleton(new BsonSerializerFactory())
            // )

            // .TryAdd<IShapedQueryCompilingExpressionVisitorFactory, MongoShapedQueryCompilingExpressionVisitorFactory>()
            // .TryAdd<IDatabase, CassandraDatabaseWrapper>()
            // .TryAdd<IModelRuntimeInitializer, MongoModelRuntimeInitializer>()
            .TryAddCoreServices();

        return serviceCollection;
    }
}