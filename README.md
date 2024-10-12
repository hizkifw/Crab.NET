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
    if (string.IsNullOrEmpty(input))
        return Result.Err<string, Exception>(new ArgumentNullException(nameof(input)));

    return Result.Ok<string, Exception>($"Hello, {input}");
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
