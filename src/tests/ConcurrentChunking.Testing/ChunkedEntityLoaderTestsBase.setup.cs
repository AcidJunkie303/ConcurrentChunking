using ConcurrentChunking.Testing.Data;
using ConcurrentChunking.Testing.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ConcurrentChunking.Testing;

#pragma warning disable MA0048 // File name must match type name

public abstract partial class ChunkedEntityLoaderTestBase<TDbContext, TTestData> : TestBase
    where TDbContext : DbContext, IDbContext, new()
    where TTestData : ITestData<TDbContext>
{
    private static int EntityCount => TTestData.EntityCount;

    protected ChunkedEntityLoaderTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    private static bool IsChunkOrderSequential<T>(in List<Chunk<T>> chunks)
        => !chunks.Where((chunk, i) => chunk.ChunkIndex != i).Any();

    private ChunkedEntityLoader<TDbContext, SimpleEntity> CreateLoader
    (
        int chunkSize,
        int maxConcurrentProducerCount,
        int maxPrefetchCount,
        ChunkedEntityLoaderOptions options,
        Func<int, Task>? chunkProductionStartedCallback = null
    )
    {
        return new ChunkedEntityLoader<TDbContext, SimpleEntity>(
            contextFactory: () => new TDbContext(),
            contextDestroyer: ctx => ctx.Dispose(),
            chunkSize: chunkSize,
            maxConcurrentProducerCount: maxConcurrentProducerCount,
            maxPrefetchCount: maxPrefetchCount,
            sourceQueryProvider: ctx => ctx.SimpleEntities.AsNoTracking().OrderBy(e => e.Id),
            countProvider: (query, cancellationToken) => query.LongCountAsync(cancellationToken),
            options: options,
            loggerFactory: LoggerFactory,
            logger: LoggerFactory.CreateLogger<ChunkedEntityLoader<TDbContext, SimpleEntity>>()
        )
        {
            ChunkProductionStarted = chunkProductionStartedCallback
        };
    }
}
