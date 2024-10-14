using Crab.Concurrency;

namespace Crab.Errors;

public static class TransposeExtensions
{
    /// <summary>
    /// Transposes an option of a task into a task of an option.
    /// </summary>
    public static Task<IOption<T>> ToTaskOption<T>(this IOption<Task<T>> option)
    {
        var ob = Option.Builder<T>();
        return option.IsNone()
            ? Task.FromResult(ob.None())
            : option.Unwrap().Then(ob.Some);
    }

    /// <summary>
    /// Transposes a result of a task into a task of a result.
    /// </summary>
    public static Task<IResult<T, E>> ToTaskResult<T, E>(this IResult<Task<T>, E> result)
    {
        var rb = Result.Builder<T, E>();
        return result.IsErr()
            ? Task.FromResult(rb.Err(result.UnwrapErr()))
            : result.Unwrap().Then(rb.Ok);
    }
}