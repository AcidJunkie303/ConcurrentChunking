# ConcurrentChunking

`ConcurrentChunking` helps load large EF Core queries in **parallel chunks** with optional prefetching.

It is useful when a single large query takes too long to materialize before processing can begin, especially for
workloads like:

- data migration
- reporting/export pipelines
- batch processing and background jobs

## Packages

- `ConcurrentChunking` - core loader API
- `ConcurrentChunking.EntityFrameworkCore` - `IQueryable<T>` extension methods

## Target frameworks

This repository currently targets:

- `net8.0`
- `net9.0`
- `net10.0`

## Installation

Install the package(s) you need:

```powershell
dotnet add package ConcurrentChunking
dotnet add package ConcurrentChunking.EntityFrameworkCore
```

## Quick start

### 1) Core loader (`ChunkedEntityLoader<TDbContext, TEntity>`)

```csharp
using ConcurrentChunking;

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
```

### 2) Entity Framework Core LINQ extension (`LoadChunkedAsync`)

`LoadChunkedAsync` requires an ordered query (`OrderBy` or `OrderByDescending`) to ensure stable chunking.

```csharp
using ConcurrentChunking;
using ConcurrentChunking.EntityFrameworkCore;

await using var ctx = new TestDbContext();

var chunks = ctx.SimpleEntities
                .Include(e => e.RelatedEntity).ThenInclude(e => e.AnotherRelatedEntity)
                .Include(e => e.RelatedEntity2)
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
```

## Tuning guidance

- `chunkSize`: larger chunks reduce round-trips but increase memory consumption per chunk.
- `maxConcurrentProducerCount`: controls how many chunk producers may run in parallel.
- `maxPrefetchCount`: limits queued chunk count and therefore memory pressure.
- `PreserveChunkOrder`: keeps output ordered by chunk index; disable if out-of-order consumption is acceptable.

Start conservatively, measure DB pressure and memory usage, then increase settings and measure again.

## Notes and caveats

- Always use deterministic ordering in source queries.
- Consider `AsNoTracking()` for read-only workloads.
- The context factory (`contextFactory` parameter) and context destroyer (`contextDestroyer`) gives you control about
  shared contexts and their destruction. If you use a shared context, be aware of potential concurrency issues and
  ensure proper disposal.

## Build and test

From `src`:

```powershell
dotnet restore
dotnet build -c Release
dotnet test -c Release
```

## License

MIT. See `LICENSE.txt`.

