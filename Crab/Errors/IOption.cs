namespace Crab.Errors;

/// <summary>
/// <c>IOption</c> represents a value that may or may not be present.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public interface IOption<T>
{
    /// <summary>
    /// Returns <c>true</c> if the value is present.
    /// </summary>
    bool IsSome();

    /// <summary>
    /// Returns <c>true</c> if the value is not present.
    /// </summary>
    bool IsNone();

    /// <summary>
    /// Returns the value if it is present, otherwise throws an exception with
    /// a custom message.
    /// </summary>
    /// <exception cref="UnwrapException"></exception>
    /// <param name="message"></param>
    T Expect(string message);

    /// <summary>
    /// Returns the value if it is present, otherwise throws an exception.
    /// </summary>
    /// <exception cref="UnwrapException"></exception>
    T Unwrap();

    /// <summary>
    /// Returns the value if it is present, otherwise returns a default value.
    /// </summary>
    T UnwrapOr(T defaultValue);

    /// <summary>
    /// Returns the value if it is present, otherwise returns <c>default(T)</c>.
    /// </summary>
    T? UnwrapOrDefault();

    /// <summary>
    /// Returns the value if it is present, otherwise returns the result of a
    /// function.
    /// </summary>
    T UnwrapOrElse(Func<T> map);

    /// <summary>
    /// Attempts to unwrap the value. Returns <c>true</c> if successful.
    /// </summary>
    bool TryUnwrap(out T value);

    /// <summary>
    /// Convert the value contained within the <c>IOption</c> to a new value.
    /// </summary>
    /// <typeparam name="U">The type of the new value.</typeparam>
    /// <param name="map">The function to convert the value.</param>
    IOption<U> Map<U>(Func<T, U> map);

    /// <summary>
    /// Convert the value contained within the <c>IOption</c> to a new value,
    /// or return a default value.
    /// </summary>
    /// <typeparam name="U">The type of the new value.</typeparam>
    /// <param name="defaultValue">The default value to return if the value is not present.</param>
    /// <param name="map">The function to convert the value.</param>
    U MapOr<U>(U defaultValue, Func<T, U> map);

    /// <summary>
    /// Convert the value contained within the <c>IOption</c> to a new value,
    /// or return the result of a function.
    /// </summary>
    /// <typeparam name="U">The type of the new value.</typeparam>
    /// <param name="defaultMap">The function to call if the value is not present.</param>
    /// <param name="map">The function to convert the value.</param>
    U MapOrElse<U>(Func<U> defaultMap, Func<T, U> map);

    /// <summary>
    /// Perform an action on the value if it is present.
    /// </summary>
    IOption<T> Inspect(Action<T> action);

    /// <summary>
    /// Convert the value to a <c>IResult</c> with the value as the Ok value.
    /// </summary>
    /// <typeparam name="E">The type of the error value.</typeparam>
    /// <param name="err">The error value to use if the value is not present.</param>
    IResult<T, E> OkOr<E>(E err);

    /// <summary>
    /// Convert the value to a <c>IResult</c> with the value as the Ok value.
    /// </summary>
    /// <typeparam name="E">The type of the error value.</typeparam>
    /// <param name="err">The function to call if the value is not present.</param>
    IResult<T, E> OkOrElse<E>(Func<E> err);
}
