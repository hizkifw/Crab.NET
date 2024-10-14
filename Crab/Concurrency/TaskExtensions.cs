namespace Crab.Concurrency;

/// <summary>
/// Provides extension methods for working with tasks.
/// </summary>
public static class TaskExtensions
{
    // Task<T> -> U
    public static async Task<U> Then<T, U>(this Task<T> task, Func<T, U> then)
    {
        ArgumentNullException.ThrowIfNull(then, nameof(then));
        var result = await task.ConfigureAwait(false);
        return then(result);
    }

    // Task<T> -> Task<U>
    public static async Task<U> Then<T, U>(this Task<T> task, Func<T, Task<U>> then)
    {
        ArgumentNullException.ThrowIfNull(then, nameof(then));
        var result = await task.ConfigureAwait(false);
        return await then(result).ConfigureAwait(false);
    }

    // Task<T> -> Task
    public static async Task Then<T>(this Task<T> task, Func<T, Task> then)
    {
        ArgumentNullException.ThrowIfNull(then, nameof(then));
        var result = await task.ConfigureAwait(false);
        await then(result).ConfigureAwait(false);
    }

    // Task -> U
    public static async Task<U> Then<U>(this Task task, Func<U> then)
    {
        ArgumentNullException.ThrowIfNull(then, nameof(then));
        await task.ConfigureAwait(false);
        return then();
    }

    // Task -> Task<U>
    public static async Task<U> Then<U>(this Task task, Func<Task<U>> then)
    {
        ArgumentNullException.ThrowIfNull(then, nameof(then));
        await task.ConfigureAwait(false);
        return await then().ConfigureAwait(false);
    }

    // Task -> Task
    public static async Task Then(this Task task, Func<Task> then)
    {
        ArgumentNullException.ThrowIfNull(then, nameof(then));
        await task.ConfigureAwait(false);
        await then().ConfigureAwait(false);
    }
}
