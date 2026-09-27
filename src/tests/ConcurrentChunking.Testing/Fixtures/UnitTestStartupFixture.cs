using ConcurrentChunking.Testing.Data;
using Xunit;

namespace ConcurrentChunking.Testing.Fixtures;

public sealed class UnitTestStartupFixture : IAsyncLifetime
{
    public async ValueTask InitializeAsync() => await InMemoryTestData.Instance.EnsureInitializedAsync();

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
