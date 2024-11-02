using System.Globalization;
using Cassandra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore;

public class CassandraOptionsExtension : IDbContextOptionsExtension
{
    public string? KeySpace => _defaultKeyspace;
    public Action<Builder> ClusterBuilder => _callback;
    public virtual DbContextOptionsExtensionInfo Info => _info ??= new ExtensionInfo(this);
    
    const string MultipleConnectionConfigSpecifiedException =
        "Both ConnectionString and Cassandra were specified. Specify only one set of connection details.";
    
    private string _defaultKeyspace;
    private Action<Builder> _callback;
    private DbContextOptionsExtensionInfo? _info;
    
    public CassandraOptionsExtension()
    {
    }
    
    protected CassandraOptionsExtension(CassandraOptionsExtension copyFrom)
    {
        _callback = copyFrom._callback;
        _defaultKeyspace = copyFrom._defaultKeyspace;
        _callback = copyFrom._callback;
    }
    
    public virtual void ApplyServices(IServiceCollection services) => services.AddEntityFrameworkCassandra();

    public void Validate(IDbContextOptions options)
    {
        // ToDo: Validate
    }
    
    public virtual CassandraOptionsExtension WithKeySpace(string keySpace)
    {
        ArgumentNullException.ThrowIfNull(keySpace);

        var clone = Clone();
        clone._defaultKeyspace = keySpace;
        return clone;
    }
    
    public virtual CassandraOptionsExtension WithCallbackClusterBuilder(Action<Builder> callback)
    {
        ArgumentNullException.ThrowIfNull(callback);

        if (_callback != null)
        {
            throw new InvalidOperationException(MultipleConnectionConfigSpecifiedException);
        }

        var clone = Clone();
        clone._callback = callback;
        
        return clone;
    }

    private CassandraOptionsExtension Clone() => new(this);
    
    private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
    {
        private int? _serviceProviderHash;
        private new CassandraOptionsExtension Extension => (CassandraOptionsExtension)base.Extension;
        
        public ExtensionInfo(IDbContextOptionsExtension extension) : base(extension)
        {
        }

        public override int GetServiceProviderHashCode()
        {
            _serviceProviderHash ??= HashCode.Combine(Extension._callback, Extension._defaultKeyspace);
            return _serviceProviderHash.Value;
        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
            => other is ExtensionInfo otherInfo
               && Extension._callback == otherInfo.Extension._callback
               && Extension._defaultKeyspace == otherInfo.Extension._defaultKeyspace;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            AddDebugInfo(debugInfo, nameof(ClusterBuilder), Extension._callback);
            AddDebugInfo(debugInfo, nameof(KeySpace), Extension._defaultKeyspace);
        }
        private static void AddDebugInfo(IDictionary<string, string> debugInfo, string key, object? value)
        {
            debugInfo["Cassandra:" + key] = (value?.GetHashCode() ?? 0L).ToString(CultureInfo.InvariantCulture);
        }
        

        public override bool IsDatabaseProvider { get; }
        public override string LogFragment { get; }
    }
}