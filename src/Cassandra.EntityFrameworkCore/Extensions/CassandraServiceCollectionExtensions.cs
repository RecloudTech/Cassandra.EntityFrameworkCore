using Cassandra.EntityFrameworkCore.Diagnostics;
using Cassandra.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
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
            // .TryAdd<IDatabase, MongoDatabaseWrapper>()
            // .TryAdd<IDbContextTransactionManager, MongoTransactionManager>()
            // .TryAdd<IModelValidator, MongoModelValidator>()
            // .TryAdd<IProviderConventionSetBuilder, MongoConventionSetBuilder>()
            // .TryAdd<IValueGeneratorSelector, MongoValueGeneratorSelector>()
            // .TryAdd<IDatabaseCreator, MongoDatabaseCreator>()
            // .TryAdd<IQueryContextFactory, MongoQueryContextFactory>()
            // .TryAdd<ITypeMappingSource, MongoTypeMappingSource>()
            // .TryAdd<IValueConverterSelector, MongoValueConverterSelector>()
            // .TryAdd<IQueryTranslationPreprocessorFactory, MongoQueryTranslationPreprocessorFactory>()
            // .TryAdd<IQueryCompilationContextFactory, MongoQueryCompilationContextFactory>()
            // .TryAdd<IQueryTranslationPostprocessorFactory, MongoQueryTranslationPostprocessorFactory>()
            // .TryAdd<IQueryableMethodTranslatingExpressionVisitorFactory, MongoQueryableMethodTranslatingExpressionVisitorFactory>()
            // .TryAdd<IShapedQueryCompilingExpressionVisitorFactory, MongoShapedQueryCompilingExpressionVisitorFactory>()
            // .TryAdd<IModelRuntimeInitializer, MongoModelRuntimeInitializer>()
            // .TryAddProviderSpecificServices(
            //     b => b
            //         .TryAddScoped<IMongoClientWrapper, MongoClientWrapper>()
            //         .TryAddSingleton<MongoShapedQueryCompilingExpressionVisitorDependencies,
            //             MongoShapedQueryCompilingExpressionVisitorDependencies>()
            //         .TryAddSingleton(new BsonSerializerFactory())
            // )
            .TryAddCoreServices();

        return serviceCollection;
    }
}