namespace Crab.Errors;

public interface IOption<T>
{
    bool IsSome();
    bool IsNone();
    T Expect(string message);
    T Unwrap();
    T UnwrapOr(T defaultValue);
    T? UnwrapOrDefault();
    T UnwrapOrElse(Func<T> map);
    bool TryUnwrap(out T value);

    IOption<U> Map<U>(Func<T, U> map);
    U MapOr<U>(U defaultValue, Func<T, U> map);
    U MapOrElse<U>(Func<U> defaultMap, Func<T, U> map);

    IOption<T> Inspect(Action<T> action);

    IResult<T, E> OkOr<E>(E err);
    IResult<T, E> OkOrElse<E>(Func<E> err);
}
