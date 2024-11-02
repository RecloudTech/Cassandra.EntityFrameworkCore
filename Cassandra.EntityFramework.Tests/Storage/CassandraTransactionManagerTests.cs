using Cassandra.EntityFrameworkCore.Storage;

namespace Cassandra.EntityFramework.Tests.Storage;

public class CassandraTransactionManagerTests
{
    [Fact]
    public async Task Does_not_support_transactions()
    {
        var transactionManager = new CassandraTransactionManager();

        Assert.Throws<NotSupportedException>(() => transactionManager.BeginTransaction());
        await Assert.ThrowsAsync<NotSupportedException>(async () => await transactionManager.BeginTransactionAsync());

        Assert.Throws<NotSupportedException>(() => transactionManager.CommitTransaction());
        await Assert.ThrowsAsync<NotSupportedException>(async () => await transactionManager.CommitTransactionAsync());

        Assert.Throws<NotSupportedException>(() => transactionManager.RollbackTransaction());
        await Assert.ThrowsAsync<NotSupportedException>(async () =>
            await transactionManager.RollbackTransactionAsync());

        Assert.Null(transactionManager.CurrentTransaction);
        Assert.Null(transactionManager.EnlistedTransaction);

        Assert.Throws<NotSupportedException>(() => transactionManager.EnlistTransaction(null));

        transactionManager.ResetState();
        await transactionManager.ResetStateAsync();

        Assert.Null(transactionManager.CurrentTransaction);
        Assert.Null(transactionManager.EnlistedTransaction);
    }
}