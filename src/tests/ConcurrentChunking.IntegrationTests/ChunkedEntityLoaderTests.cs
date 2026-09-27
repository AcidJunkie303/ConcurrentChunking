using ConcurrentChunking.Testing;
using ConcurrentChunking.Testing.Data;
using ConcurrentChunking.Testing.Entities;

namespace ConcurrentChunking.IntegrationTests;

public sealed class ChunkedEntityLoaderTests : ChunkedEntityLoaderTestBase<SqlServerDbContext, SqlServerTestData>
{
    public ChunkedEntityLoaderTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }
}
