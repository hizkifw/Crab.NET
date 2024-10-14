namespace Crab.Errors;

/// <summary>
/// A static class that provides methods for creating `Some` and `None` options.
/// </summary>
public static class Option
{
    /// <summary>
    /// Creates a new `Some` option.
    /// </summary>
    public static IOption<T> Some<T>(T value) => Option<T>.CreateSome(value);

    /// <summary>
    /// Creates a new `None` option.
    /// </summary>
    public static IOption<T> None<T>() => Option<T>.CreateNone();

    /// <summary>
    /// Creates a new `OptionBuilder` that can be used to create `Some` and `None`
    /// options.
    /// </summary>
    public static OptionBuilder<T> Builder<T>() => new();
}

/// <summary>
/// A builder that can be used to create `Some` and `None` options.
/// </summary>
/// <typeparam name="T">The type of the value that the option will contain.</typeparam>
public class OptionBuilder<T>
{
    internal OptionBuilder()
    {
    }

    /// <summary>
    /// Creates a new `Some` option.
    /// </summary>
    public IOption<T> Some(T value) => Option<T>.CreateSome(value);

    /// <summary>
    /// Creates a new `None` option.
    /// </summary>
    public IOption<T> None() => Option<T>.CreateNone();
}

/// <summary>
/// Represents a value that can be either `Some` or `None`.
/// </summary>
/// <typeparam name="T">The type of the value that the option will contain.</typeparam>
public class Option<T> : IOption<T>
{
    private readonly T? _value;
    private readonly bool _isSome;

    private Option(T value)
    {
        _value = value;
        _isSome = true;
    }

    private Option()
    {
        _value = default;
        _isSome = false;
    }

    internal static IOption<T> CreateSome(T value) => new Option<T>(value);
    internal static IOption<T> CreateNone() => new Option<T>();

    public bool IsSome() => _isSome;
    public bool IsNone() => !_isSome;

    public T Expect(string message) =>
        _isSome ? _value! : throw new UnwrapException(message);

    public IOption<T> Inspect(Action<T> action)
    {
        if (IsSome()) action(_value!);
        return this;
    }

    public IOption<U> Map<U>(Func<T, U> map) => IsSome()
        ? Option<U>.CreateSome(map(_value!)) : Option<U>.CreateNone();

    public U MapOr<U>(U defaultValue, Func<T, U> map) =>
        IsSome() ? map(_value!) : defaultValue;

    public U MapOrElse<U>(Func<U> defaultMap, Func<T, U> map) =>
        IsSome() ? map(_value!) : defaultMap();

    public IResult<T, E> OkOr<E>(E err) =>
        IsSome() ? Result<T, E>.CreateOk(_value!) : Result<T, E>.CreateErr(err);

    public IResult<T, E> OkOrElse<E>(Func<E> err) =>
        IsSome() ? Result<T, E>.CreateOk(_value!) : Result<T, E>.CreateErr(err());

    public T Unwrap() => Expect("Called `Unwrap` on a `None` value.");

    public T UnwrapOr(T defaultValue) =>
        IsSome() ? _value! : defaultValue;

    public T? UnwrapOrDefault() =>
        IsSome() ? _value! : default;

    public T UnwrapOrElse(Func<T> map) =>
        IsSome() ? _value! : map();

    public bool TryUnwrap(out T value)
    {
        value = _value!;
        return IsSome();
    }
}