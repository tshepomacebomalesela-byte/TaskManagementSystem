using Microsoft.Extensions.Caching.Hybrid;


namespace TaskManagementTests.Helpers;

public class PassthroughHybridCache : HybridCache
{
    // 1. The missing abstract member causing CS0534
    public override async ValueTask<T> GetOrCreateAsync<TState, T>(
        string key,
        TState state,
        Func<TState, CancellationToken, ValueTask<T>> factory,
        HybridCacheEntryOptions? options = null,
        IEnumerable<string>? tags = null,
        CancellationToken cancellationToken = default)
    {
        // For a passthrough, just execute the factory with the provided state
        return await factory(state, cancellationToken);
    }

    // 2. Ensure your existing overrides match these .NET 9 signatures exactly:

    public override ValueTask SetAsync<T>(string key, T value, HybridCacheEntryOptions? options = null, IEnumerable<string>? tags = null, CancellationToken cancellationToken = default)
        => ValueTask.CompletedTask;

    public override ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default)
        => ValueTask.CompletedTask;

    public override ValueTask RemoveByTagAsync(string tag, CancellationToken cancellationToken = default)
        => ValueTask.CompletedTask;

    public override ValueTask RemoveByTagAsync(IEnumerable<string> tags, CancellationToken cancellationToken = default)
        => ValueTask.CompletedTask;
}
