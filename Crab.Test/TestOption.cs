using Crab.Errors;

namespace Crab.Test;

public class TestOption
{
    private IOption<string> Greet(string input)
    {
        if (string.IsNullOrEmpty(input))
            return Option.None<string>();

        return Option.Some($"Hello, {input}");
    }

    [Fact]
    public void TestOptionNone()
    {
        var greetNone = Greet(string.Empty);
        Assert.True(greetNone.IsNone());
        Assert.False(greetNone.IsSome());
        try { greetNone.Expect("Expected a value"); Assert.Fail("Should throw"); }
        catch (UnwrapException e) { Assert.Equal("Expected a value", e.Message); }
        try { greetNone.Unwrap(); Assert.Fail("Should throw"); }
        catch (UnwrapException) { }
        Assert.Equal("fallback", greetNone.UnwrapOr("fallback"));
        Assert.Equal(default, greetNone.UnwrapOrDefault());
        Assert.Equal("fallback", greetNone.UnwrapOrElse(() => "fallback"));
        Assert.True(greetNone.Map(_ => { Assert.Fail("Should not be called"); return 1; }).IsNone());
        Assert.Equal("fallback", greetNone.MapOr("fallback", _ => { Assert.Fail("Should not be called"); return "a"; }));
        Assert.Equal("fallback", greetNone.MapOrElse(() => "fallback", _ => { Assert.Fail("Should not be called"); return "a"; }));
        Assert.True(greetNone.Inspect(_ => Assert.Fail("Should not be called")).IsNone());
        Assert.Equal("none", greetNone.OkOr("none").UnwrapErr());
        Assert.Equal("none", greetNone.OkOrElse(() => "none").UnwrapErr());
        Assert.False(greetNone.TryUnwrap(out var value));
    }

    [Fact]
    public void TestOptionSome()
    {
        var greetSome = Greet("world");
        Assert.True(greetSome.IsSome());
        Assert.False(greetSome.IsNone());
        Assert.Equal("Hello, world", greetSome.Expect("Expected a value"));
        Assert.Equal("Hello, world", greetSome.Unwrap());
        Assert.Equal("Hello, world", greetSome.UnwrapOr("fallback"));
        Assert.Equal("Hello, world", greetSome.UnwrapOrDefault());
        Assert.Equal("Hello, world", greetSome.UnwrapOrElse(() => "fallback"));
        Assert.Equal(1, greetSome.Map(value => { Assert.Equal("Hello, world", value); return 1; }).Unwrap());
        Assert.Equal(1, greetSome.MapOr(0, value => { Assert.Equal("Hello, world", value); return 1; }));
        Assert.Equal(1, greetSome.MapOrElse(() => 0, value => { Assert.Equal("Hello, world", value); return 1; }));
        Assert.Equal("Hello, world", greetSome.Inspect(value => Assert.Equal("Hello, world", value)).Unwrap());
        Assert.Equal("Hello, world", greetSome.OkOr("none").Unwrap());
        Assert.Equal("Hello, world", greetSome.OkOrElse(() => "none").Unwrap());
        Assert.True(greetSome.TryUnwrap(out var value));
        Assert.Equal("Hello, world", value);
    }
}