using ConcurrentChunking.Testing;
using ConcurrentChunking.Testing.Data;
using ConcurrentChunking.Testing.Entities;

namespace ConcurrentChunking.Linq.Tests;

public sealed class OrderedQueryableExtensionsTests : OrderedQueryableExtensionsTestBase<InMemoryDbContext, InMemoryTestData>
{
    public OrderedQueryableExtensionsTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }
}
