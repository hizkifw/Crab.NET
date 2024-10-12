namespace Crab.Errors;

/// <summary>
/// <c>IResult</c> is a type that represents either success (Ok) or failure (Err).
/// </summary>
/// <typeparam name="T">The type of the Ok value.</typeparam>
/// <typeparam name="E">The type of the Err value.</typeparam>
public interface IResult<T, E>
{
    /// <summary>
    /// Returns <c>true</c> if the result is Ok.
    /// </summary>
    bool IsOk();

    /// <summary>
    /// Returns <c>true</c> if the result is Err.
    /// </summary>
    bool IsErr();

    /// <summary>
    /// Returns the Ok value if the result is Ok, otherwise throws an exception.
    /// </summary>
    /// <param name="message">The message to include in the exception.</param>
    /// <exception cref="UnwrapException"></exception>
    T Expect(string message);

    /// <summary>
    /// Returns the Err value if the result is Err, otherwise throws an exception.
    /// </summary>
    /// <param name="message">The message to include in the exception.</param>
    /// <exception cref="UnwrapException"></exception>
    E ExpectErr(string message);

    /// <summary>
    /// Unwraps the result, yielding the Ok value.
    /// </summary>
    /// <exception cref="UnwrapException"></exception>
    T Unwrap();

    /// <summary>
    /// Unwraps the result, yielding the Err value.
    /// </summary>
    /// <exception cref="UnwrapException"></exception>
    E UnwrapErr();

    /// <summary>
    /// Unwraps the result, yielding the Ok value or a default.
    /// </summary>
    /// <param name="defaultValue">The default value to return if the result is Err.</param>
    T UnwrapOr(T defaultValue);

    /// <summary>
    /// Unwraps the result, yielding the Ok value or a default.
    /// </summary>
    T? UnwrapOrDefault();

    /// <summary>
    /// Unwraps the result, yielding the Ok value or the result of a default function.
    /// </summary>
    /// <param name="map">Function to call if the result is Err.</param>
    T UnwrapOrElse(Func<E, T> map);

    /// <summary>
    /// Attempts to unwrap the Ok value, returning <c>true</c> if successful.
    /// </summary>
    bool TryUnwrap(out T value);

    /// <summary>
    /// Attempts to unwrap the Err value, returning <c>true</c> if successful.
    /// </summary>
    bool TryUnwrapErr(out E value);

    /// <summary>
    /// Maps the Ok value to a new value, returning a new Result.
    /// </summary>
    IResult<U, E> Map<U>(Func<T, U> map);

    /// <summary>
    /// Maps the Err value to a new value, returning a new Result.
    /// </summary>
    IResult<T, F> MapErr<F>(Func<E, F> map);

    /// <summary>
    /// Maps the Ok value to a new value, returning the new value or a default.
    /// </summary>
    U MapOr<U>(U defaultValue, Func<T, U> map);

    /// <summary>
    /// Maps the Ok value to a new value, returning the new value or the result
    /// of a default function.
    /// </summary>
    U MapOrElse<U>(Func<E, U> defaultMap, Func<T, U> map);

    /// <summary>
    /// Inspects the result, calling the action if the result is Ok.
    /// </summary>
    IResult<T, E> Inspect(Action<T> action);

    /// <summary>
    /// Inspects the result, calling the action if the result is Err.
    /// </summary>
    IResult<T, E> InspectErr(Action<E> action);

    /// <summary>
    /// Transforms the result into an Option. If the result is Ok, the Option
    /// will be Some, otherwise None.
    /// </summary>
    IOption<T> Ok();

    /// <summary>
    /// Transforms the result into an Option. If the result is Err, the Option
    /// will be Some, otherwise None.
    /// </summary>
    IOption<E> Err();
}