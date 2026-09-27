using ConcurrentChunking.Testing;
using ConcurrentChunking.Testing.Data;
using ConcurrentChunking.Testing.Entities;

namespace ConcurrentChunking.Tests;

public sealed class ChunkedEntityLoaderTests : ChunkedEntityLoaderTestBase<InMemoryDbContext, InMemoryTestData>
{
    public ChunkedEntityLoaderTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }
}
