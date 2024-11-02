using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cassandra.EntityFrameworkCore.Storage;

public class CassandraTransactionManager : IDbContextTransactionManager, ITransactionEnlistmentManager
{
    /// <inheritdoc />
    public void ResetState()
    {
    }

    /// <inheritdoc />
    public Task ResetStateAsync(CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public IDbContextTransaction BeginTransaction()
    {
        throw CreateNotSupportedException();
    }

    /// <inheritdoc />
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = new())
    {
        throw CreateNotSupportedException();
    }

    /// <inheritdoc />
    public void CommitTransaction()
    {
        throw CreateNotSupportedException();
    }

    /// <inheritdoc />
    public Task CommitTransactionAsync(CancellationToken cancellationToken = new())
    {
        throw CreateNotSupportedException();
    }

    /// <inheritdoc />
    public void RollbackTransaction()
    {
        throw CreateNotSupportedException();
    }

    /// <inheritdoc />
    public Task RollbackTransactionAsync(CancellationToken cancellationToken = new())
    {
        throw CreateNotSupportedException();
    }

    /// <inheritdoc />
    public IDbContextTransaction? CurrentTransaction
        => null;

    /// <inheritdoc />
    public void EnlistTransaction(Transaction? transaction)
    {
        throw CreateNotSupportedException();
    }

    /// <inheritdoc />
    public Transaction? EnlistedTransaction
        => null;

    private static NotSupportedException CreateNotSupportedException()
    {
        return new NotSupportedException("The MongoDB EF Core Provider does not support transactions.");
    }
}