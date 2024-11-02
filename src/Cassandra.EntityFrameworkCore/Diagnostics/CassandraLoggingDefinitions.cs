using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Cassandra.EntityFrameworkCore.Diagnostics;

public class CassandraLoggingDefinitions : LoggingDefinitions
{
    public EventDefinitionBase? LogExecutedMqlQuery;

    public EventDefinitionBase? LogExecutingBulkWrite;
    public EventDefinitionBase? LogExecutedBulkWrite;

    public EventDefinitionBase? LogBeginningTransaction;
    public EventDefinitionBase? LogBeganTransaction;

    public EventDefinitionBase? LogCommittingTransaction;
    public EventDefinitionBase? LogCommittedTransaction;

    public EventDefinitionBase? LogRollingBackTransaction;
    public EventDefinitionBase? LogRolledBackTransaction;

    public EventDefinitionBase? LogTransactionError;
}