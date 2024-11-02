using System.Globalization;
using Cassandra;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore;

public class CassandraOptionsExtension : IDbContextOptionsExtension
{
    const string MultipleConnectionConfigSpecifiedException =
        "Both ConnectionString and Cassandra were specified. Specify only one set of connection details.";

    private string _defaultKeyspace;
    private DbContextOptionsExtensionInfo? _info;

    public CassandraOptionsExtension()
    {
    }

    protected CassandraOptionsExtension(CassandraOptionsExtension copyFrom)
    {
        ClusterBuilder = copyFrom.ClusterBuilder;
        _defaultKeyspace = copyFrom._defaultKeyspace;
        ClusterBuilder = copyFrom.ClusterBuilder;
    }

    public string? DefaultKeyspace => _defaultKeyspace;
    public Action<Builder> ClusterBuilder { get; private set; }

    public virtual DbContextOptionsExtensionInfo Info => _info ??= new ExtensionInfo(this);

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

    public static CassandraOptionsExtension Extract(IDbContextOptions options)
    {
        var relationalOptionsExtensions
            = options.Extensions
                .OfType<CassandraOptionsExtension>()
                .ToList();
        return relationalOptionsExtensions[0];
    }

    public virtual CassandraOptionsExtension WithCallbackClusterBuilder(Action<Builder> callback)
    {
        ArgumentNullException.ThrowIfNull(callback);

        if (ClusterBuilder != null)
        {
            throw new InvalidOperationException(MultipleConnectionConfigSpecifiedException);
        }

        var clone = Clone();
        clone.ClusterBuilder = callback;

        return clone;
    }

    private CassandraOptionsExtension Clone() => new(this);

    private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
    {
        private int? _serviceProviderHash;

        public ExtensionInfo(IDbContextOptionsExtension extension) : base(extension)
        {
        }

        private new CassandraOptionsExtension Extension => (CassandraOptionsExtension)base.Extension;


        public override bool IsDatabaseProvider { get; }
        public override string LogFragment { get; }

        public override int GetServiceProviderHashCode()
        {
            _serviceProviderHash ??= HashCode.Combine(Extension.ClusterBuilder, Extension._defaultKeyspace);
            return _serviceProviderHash.Value;
        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
            => other is ExtensionInfo otherInfo
               && Extension.ClusterBuilder == otherInfo.Extension.ClusterBuilder
               && Extension._defaultKeyspace == otherInfo.Extension._defaultKeyspace;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            AddDebugInfo(debugInfo, nameof(ClusterBuilder), Extension.ClusterBuilder);
            AddDebugInfo(debugInfo, nameof(DefaultKeyspace), Extension._defaultKeyspace);
        }

        private static void AddDebugInfo(IDictionary<string, string> debugInfo, string key, object? value)
        {
            debugInfo["Cassandra:" + key] = (value?.GetHashCode() ?? 0L).ToString(CultureInfo.InvariantCulture);
        }
    }
}