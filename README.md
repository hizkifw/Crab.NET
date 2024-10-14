# Crab

A collection of utilities for C#.

```sh
dotnet add package Crab.NET
```

## Features

### `.Then`

Extends the `Task` class with a `.Then` method that allows chaining of tasks.

```csharp
var result = await Task.FromResult(1)
    .Then((result) => result + 1)
    .Then((result) => result * 2);
```

### `Mutex<T>`

A mutex that wraps a value and provides a lockable guard for safe access.

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

A result type that can represent either a successful value or an error.

```csharp
// Create a method that returns a Result
private IResult<string, Exception> Greet(string input)
{
    var rb = Result.Builder<string, Exception>();

    if (string.IsNullOrEmpty(input))
        return rb.Err(new ArgumentNullException(nameof(input)));

    return rb.Ok($"Hello, {input}");
}

// Use the result
var result = Greet("world");
if (result.IsOk)
{
    Console.WriteLine(result.Unwrap());
}
else
{
    Console.WriteLine(result.UnwrapErr().Message);
}
```

### `Option<T>`

An option type that can represent either a value or nothing.

```csharp
// Create an option
var some = Option.Some(123);
var none = Option.None<int>();

// Use the option
if (some.TryUnwrap(out var value))
{
    Console.WriteLine(value);
}
```

### `IEnumerable.MapFilter`

An extension method for `IEnumerable` that allows mapping and filtering in a
single pass.

```csharp
// Map and filter in a single pass
var result = new[] { 1, 2, 3, 4, 5 }
    .MapFilter((x) =>
    {
        if (x % 2 == 0)
            return Option.Some(x * 2);
        return Option.None<int>();
    });
```
