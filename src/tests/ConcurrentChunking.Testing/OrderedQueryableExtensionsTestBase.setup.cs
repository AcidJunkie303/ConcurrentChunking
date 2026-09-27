using ConcurrentChunking.Testing.Data;
using ConcurrentChunking.Testing.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ConcurrentChunking.Testing;

public abstract partial class OrderedQueryableExtensionsTestBase<TContext, TTestData> : TestBase
    where TContext : DbContext, IDbContext, new()
    where TTestData : ITestData<TContext>, ITestData
{
    private static int EntityCount => TTestData.EntityCount;

    protected OrderedQueryableExtensionsTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }
}
