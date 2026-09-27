using System.Diagnostics.CodeAnalysis;

namespace ConcurrentChunking.Extensions;

internal static class QueryableExtensions
{
    [SuppressMessage("Minor Code Smell", "S4261:Methods should be named according to their synchronicities", Justification = "This method is an extension method for IQueryable<T> and is intended to be used in a synchronous context.")]
    [SuppressMessage("Roslynator", "RCS1046:Asynchronous method name should end with \'Async\'", Justification = "This method is an extension method for IQueryable<T> and is intended to be used in a synchronous context.")]
    public static IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(this IQueryable<TSource> source)
    {
        if (source is IAsyncEnumerable<TSource> asyncEnumerable)
        {
            return asyncEnumerable;
        }

        throw new InvalidOperationException($"The type does not implement '{nameof(IAsyncEnumerable<>)}'!");
    }

    public static async Task<List<TSource>> ToListAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
    {
        var list = new List<TSource>();

        await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            list.Add(element);
        }

        return list;
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("Minor Code Smell", "S4261:Methods should be named according to their synchronicities")]
    [SuppressMessage("Roslynator", "RCS1047:Non-asynchronous method name should not end with \'Async\'")]
    public static bool SupportsToListAsync<TSource>(this IQueryable<TSource> source) => source is IAsyncEnumerable<TSource>;
}
