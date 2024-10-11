
namespace Crab.Errors;

public class Result<T, E> : IResult<T, E>
{
    private readonly T? _okValue;
    private readonly E? _errValue;
    private readonly bool _isOk;

    private Result(T okValue)
    {
        _okValue = okValue;
        _errValue = default;
        _isOk = true;
    }

    private Result(E errValue)
    {
        _okValue = default;
        _errValue = errValue;
        _isOk = false;
    }

    public static IResult<T, E> CreateOk(T value) => new Result<T, E>(value);
    public static IResult<T, E> CreateErr(E value) => new Result<T, E>(value);

    public T Expect(string message) =>
        IsOk() ? _okValue! : throw new UnwrapException(message);

    public E ExpectErr(string message) =>
        IsErr() ? _errValue! : throw new UnwrapException(message);

    public IResult<T, E> Inspect(Action<T> action)
    {
        if (IsOk()) action(_okValue!);
        return this;
    }

    public IResult<T, E> InspectErr(Action<E> action)
    {
        if (IsErr()) action(_errValue!);
        return this;
    }

    public bool IsErr() => !_isOk;

    public bool IsOk() => _isOk;

    public IResult<U, E> Map<U>(Func<T, U> map) => IsOk()
        ? Result<U, E>.CreateOk(map(_okValue!))
        : Result<U, E>.CreateErr(_errValue!);

    public IResult<T, F> MapErr<F>(Func<E, F> map) => IsErr()
        ? Result<T, F>.CreateErr(map(_errValue!))
        : Result<T, F>.CreateOk(_okValue!);

    public U MapOr<U>(U defaultValue, Func<T, U> map) =>
        IsOk() ? map(_okValue!) : defaultValue;

    public U MapOrElse<U>(Func<E, U> defaultMap, Func<T, U> map) =>
        IsOk() ? map(_okValue!) : defaultMap(_errValue!);

    public T Unwrap() =>
        Expect("Called `Unwrap` on an `Err` value");

    public E UnwrapErr() =>
        ExpectErr("Called `UnwrapErr` on an `Ok` value");

    public T UnwrapOr(T defaultValue) =>
        IsOk() ? _okValue! : defaultValue;

    public T? UnwrapOrDefault() =>
        IsOk() ? _okValue! : default;

    public T UnwrapOrElse(Func<E, T> map) =>
        IsOk() ? _okValue! : map(_errValue!);

    public IOption<T> Ok() =>
        IsOk() ? Option<T>.CreateSome(_okValue!) : Option<T>.CreateNone();

    public IOption<E> Err() =>
        IsErr() ? Option<E>.CreateSome(_errValue!) : Option<E>.CreateNone();
}