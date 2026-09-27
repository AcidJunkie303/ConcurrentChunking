using ConcurrentChunking;
using ConcurrentChunking.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Playground.Logging;

namespace Playground;

#pragma warning disable

internal static class Program
{
    private static async Task Main()
    {
        using var consoleLoggerFactory = new ConsoleLoggerFactory();

        try
        {
            var loader = new ChunkedEntityLoader<SqlServerDbContext, SimpleEntity>(
                contextFactory: () => new SqlServerDbContext(),
                contextDestroyer: ctx => ctx.Dispose(),
                countProvider: (query, cancellationToken) => query.LongCountAsync(cancellationToken),
                chunkSize: 100_000,
                maxConcurrentProducerCount: 5,
                maxPrefetchCount: 5,
                sourceQueryProvider: ctx => ctx.SimpleEntities
                                               .AsNoTracking()
                                               .OrderBy(a => a.Id),
                loggerFactory: consoleLoggerFactory
            );

            var chunks = await loader.LoadAsync(CancellationToken.None).ToListAsync();

            Console.WriteLine($"Retrieved {chunks.Count} chunks with total {chunks.Sum(a => a.Entities.Count)} entities.");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex);
            Console.ResetColor();
        }

        /*
        InitializeDbContext();

        await DocSample1();

        return;

        // do your test stuff here
        await using var ctx = new TestDbContext();

        var entities = await ctx.SimpleEntities
                                .Where(a => a.Id < 1000)
                                .OrderByDescending(a => a.Id)
                                .Select(a => new { Bla = a.Id, a.Value })
                                .LoadChunkedAsync
                                 (
                                     dbContextFactory: () => new TestDbContext(),
                                     chunkSize: 10,
                                     maxConcurrentProducerCount: 5,
                                     maxPrefetchCount: 10,
                                     options: ChunkedEntityLoaderOptions.PreserveChunkOrder
                                 )
                                .ToListAsync();
                                */
    }


    private static async Task DocSample1()
    {
        await using var ctx = new TestDbContext();

        var chunks = ctx.SimpleEntities
                              .AsNoTracking()
                              .OrderByDescending(e => e.Id)
                              .LoadChunkedAsync (
                                   dbContextFactory: () => new TestDbContext(),
                                   dbContextDestroyer: c => c.Dispose(),
                                   chunkSize: 100_000,
                                   maxConcurrentProducerCount: 3,
                                   maxPrefetchCount: 5,
                                   options: ChunkedEntityLoaderOptions.PreserveChunkOrder);

        await foreach (var chunk in chunks)
        {
            foreach (var entity in chunk.Entities)
            {
                Console.WriteLine($"Entity ID: {entity.Id}");
            }
        }
    }
}
