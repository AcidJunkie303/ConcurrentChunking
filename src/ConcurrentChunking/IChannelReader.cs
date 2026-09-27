namespace ConcurrentChunking;

internal interface IChannelReader<TEntity>
{
    IAsyncEnumerable<Chunk<TEntity>> ReadAsync(CancellationToken cancellationToken);
}
