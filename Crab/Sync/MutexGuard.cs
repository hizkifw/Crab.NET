namespace Crab.Sync;

using System;

public struct MutexGuard<T> : IDisposable
{
    private readonly Mutex<T> _mutex;
    private bool _disposed;

    internal MutexGuard(Mutex<T> mutex)
    {
        _mutex = mutex;
        _disposed = false;
    }

    public readonly T Value
    {
        get
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(MutexGuard<T>));

            return _mutex.GetValue();
        }

        set
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(MutexGuard<T>));

            _mutex.SetValue(value);
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _mutex.Release();
            _disposed = true;
        }
    }
}