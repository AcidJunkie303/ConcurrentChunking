using ConcurrentChunking.Testing;
using ConcurrentChunking.Testing.Data;
using ConcurrentChunking.Testing.Entities;

namespace ConcurrentChunking.EntityFrameworkCore.IntegrationTests;

public sealed class OrderedQueryableExtensionsTests : OrderedQueryableExtensionsTestBase<SqlServerDbContext, SqlServerTestData>
{
    public OrderedQueryableExtensionsTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }
}
