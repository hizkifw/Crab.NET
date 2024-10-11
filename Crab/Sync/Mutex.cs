namespace Crab.Sync;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// A lock that wraps a value and allows for safe mutation and access.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Mutex<T> : IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private T _value;
    private bool _disposed = false;

    public Mutex(T value)
    {
        _value = value;
    }

    /// <summary>
    /// Locks the mutex and returns a guard that releases the lock when disposed.
    /// </summary>
    /// <example>
    /// <code>
    /// using var guard = mutex.Lock();
    /// var value = guard.Value;
    /// </code>
    /// </example>
    public MutexGuard<T> Lock()
    {
        _semaphore.Wait();
        return new MutexGuard<T>(this);
    }

    /// <summary>
    /// Locks the mutex asynchronously and returns a guard that releases the
    /// lock when disposed.
    /// </summary>
    /// <example>
    /// <code>
    /// using var guard = await mutex.LockAsync();
    /// var value = guard.Value;
    /// </code>
    /// </example>
    public async Task<MutexGuard<T>> LockAsync(CancellationToken ct = default)
    {
        await _semaphore.WaitAsync(ct);
        ct.ThrowIfCancellationRequested();

        return new MutexGuard<T>(this);
    }

    public async Task<U> MapAsync<U>(Func<T, U> map, CancellationToken ct = default)
    {
        using var guard = await LockAsync(ct);
        return map(guard.Value);
    }

    public async Task<T> GetAsync(CancellationToken ct = default)
    {
        using var guard = await LockAsync(ct);
        return guard.Value;
    }

    public async Task<T> SetAsync(T newValue, CancellationToken ct = default)
    {
        using var guard = await LockAsync(ct);
        guard.Value = newValue;
        return guard.Value;
    }

    public async Task<T> SetAsync(Func<T, T> mutate, CancellationToken ct = default)
    {
        using var guard = await LockAsync(ct);
        guard.Value = mutate(guard.Value);
        return guard.Value;
    }

    internal T GetValue() => _value;
    internal void SetValue(T value) => _value = value;
    internal void Release() => _semaphore.Release();

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _semaphore?.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Mutex()
    {
        Dispose(false);
    }
}
