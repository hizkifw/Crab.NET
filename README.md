# Crab

Utilities for C#

## Features

### `Mutex<T>`

```csharp
// Initialize the value within the mutex
var mutex = new Mutex<List<int>>([1, 2, 3]);

// Lock and use the mutex within a `using` block
using (var guard = mutex.Lock())
{
    guard.Value.Add(1234);
}

// Works within an async context
using (var guard = await mutex.LockAsync(ct))
{
    // ...
}

// Convenience methods for mapping, the callback will run while the lock is
// acquired.
var sum = mutex.MapAsync((now) => now.Sum());

// Set a new value using a function
var ordered = mutex.SetAsync((now) => now.Order().ToList());
// Or by supplying a new value directly
var emptyList = mutex.SetAsync(new List<int>());
```

### `Result<T, E>`

```csharp
// Create a method that returns a Result
public IResult<string, Exception> Greet(string input)
{
    if (string.IsNullOrEmpty(input))
        return Result<string, Exception>.CreateErr(new ArgumentNullException(nameof(input)));

    return Result<string, Exception>.CreateOk($"Hello, {input}");
}
```

### `Option<T>`

```csharp

```
