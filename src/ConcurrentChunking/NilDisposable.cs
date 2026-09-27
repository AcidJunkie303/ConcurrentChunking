namespace ConcurrentChunking;

internal sealed class NilDisposable : IDisposable
{
    public static IDisposable Instance { get; } = new NilDisposable();

    private NilDisposable()
    {
    }

    public void Dispose()
    {
    }
}
