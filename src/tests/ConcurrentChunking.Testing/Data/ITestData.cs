namespace ConcurrentChunking.Testing.Data;

public interface ITestData
{
    static abstract int EntityCount { get; }
    static abstract int ChunkSize { get; }
}

public interface ITestData<out TDbContext> : ITestData
    where TDbContext : class
{
    public static abstract ITestData<TDbContext> Instance { get; }
    Task EnsureInitializedAsync();
    TDbContext CreateDbContext();
}
