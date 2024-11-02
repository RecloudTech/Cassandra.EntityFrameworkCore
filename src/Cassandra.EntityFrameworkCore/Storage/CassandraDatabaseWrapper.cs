using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace Cassandra.EntityFrameworkCore.Storage;

public class CassandraDatabaseWrapper : Database
{
    private readonly ICurrentDbContext _currentDbContext;
    private readonly IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> _transactionLogger;
    private readonly IDiagnosticsLogger<DbLoggerCategory.Update> _updateLogger;

    public CassandraDatabaseWrapper(
        DatabaseDependencies dependencies,
        ICurrentDbContext currentDbContext,
        IDiagnosticsLogger<DbLoggerCategory.Update> updateLogger,
        IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> transactionLogger)
        : base(dependencies)
    {
        _currentDbContext = currentDbContext;
        _updateLogger = updateLogger;
        _transactionLogger = transactionLogger;
    }

    public override int SaveChanges(IList<IUpdateEntry> entries)
    {
        return 0;
    }

    public override Task<int> SaveChangesAsync(IList<IUpdateEntry> entries,
        CancellationToken cancellationToken = new())
    {
        return Task.FromResult(0);
    }
}