using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cassandra.EntityFrameworkCore.Storage;

public class CassandraDatabaseCreator(
    IRelationalConnection relationalConnection,
    IRawSqlCommandBuilder rawCommandBuilder,
    RelationalConnectionDependencies relationalConnectionDependencies,
    RelationalDatabaseCreatorDependencies dependencies)
    : RelationalDatabaseCreator(dependencies)
{
    private readonly IRawSqlCommandBuilder _rawSqlCommandBuilder = rawCommandBuilder;

    private readonly RelationalConnectionDependencies _relationalConnectionDependencies =
        relationalConnectionDependencies;

    public override bool Exists()
    {
        try
        {
            return relationalConnection.Open();
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            relationalConnection.Close();
        }
    }

    public override bool HasTables()
    {
        var optionsExtensions = CassandraOptionsExtension.Extract(_relationalConnectionDependencies.ContextOptions);
        var sql =
            $"SELECT count(*) FROM system_schema.tables WHERE keyspace_name='{optionsExtensions.DefaultKeyspace}'";

        var result = Dependencies.ExecutionStrategy.Execute(relationalConnection, connection =>
        {
            var countResult = _rawSqlCommandBuilder.Build(sql).ExecuteScalar(
                new RelationalCommandParameterObject(connection, null, null, null, null)
            );

            return (countResult as long? ?? 0) > 0;
        });

        return result;
    }

    public override void Create()
    {
    }

    public override void Delete()
    {
    }
}