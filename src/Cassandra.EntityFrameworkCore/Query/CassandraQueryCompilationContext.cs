using Microsoft.EntityFrameworkCore.Query;

namespace Cassandra.EntityFrameworkCore.Query;

public class CassandraQueryCompilationContext(QueryCompilationContextDependencies dependencies, bool async)
    : QueryCompilationContext(dependencies, async);