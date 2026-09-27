using ConcurrentChunking;
using Microsoft.EntityFrameworkCore;

namespace Playground;

internal static class ExamplesForDocumentation
{
    public static async Task ChunkedEntityLoaderDemoAsync()
    {
        using var loader = new ChunkedEntityLoader<TestDbContext, SimpleEntity>(
            contextFactory: () => new TestDbContext(),
            countProvider: (query, cancellationToken) => query.LongCountAsync(cancellationToken),
            chunkSize: 100_000,
            maxConcurrentProducerCount: 3,
            maxPrefetchCount: 5,
            sourceQueryProvider: ctx => ctx.SimpleEntities.OrderBy(e => e.Id)
        );

        await foreach (var chunk in loader.LoadAsync(CancellationToken.None))
        {
            foreach (var entity in chunk.Entities)
            {
                Console.WriteLine($"Entity ID: {entity.Id}");
            }
        }
    }
}
