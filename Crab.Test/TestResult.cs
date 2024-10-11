
using Crab.Errors;

namespace Crab.Test;

public class TestResult
{
    private IResult<string, Exception> Greet(string input)
    {
        if (string.IsNullOrEmpty(input))
            return Result<string, Exception>.CreateErr(new ArgumentNullException(nameof(input)));

        return Result<string, Exception>.CreateOk($"Hello, {input}");
    }

    [Fact]
    public void TestResultErr()
    {
        var greetErr = Greet(string.Empty);
        Assert.True(greetErr.IsErr());
        Assert.False(greetErr.IsOk());
        try { greetErr.Expect("Expected a value"); }
        catch (UnwrapException e) { Assert.Equal("Expected a value", e.Message); }
        Assert.True(greetErr.ExpectErr("Should be error") is ArgumentNullException);
        try { greetErr.Unwrap(); }
        catch (UnwrapException) { }
        Assert.True(greetErr.UnwrapErr() is ArgumentNullException);
        Assert.Equal("fallback", greetErr.UnwrapOr("fallback"));
        Assert.Equal(default, greetErr.UnwrapOrDefault());
        Assert.Equal("fallback", greetErr.UnwrapOrElse((ex) =>
        {
            Assert.True(ex is ArgumentNullException);
            return "fallback";
        }));
        Assert.Equal("fallback", greetErr.MapErr(ex =>
        {
            Assert.True(ex is ArgumentNullException);
            return "fallback";
        }).UnwrapErr());
        Assert.Equal("fallback", greetErr.MapOr("fallback", (value) =>
        {
            Assert.Fail("Should not be called");
            return "fail";
        }));
        Assert.Equal("fallback", greetErr.MapOrElse((err) =>
        {
            Assert.True(err is ArgumentNullException);
            return "fallback";
        }, (value) =>
        {
            Assert.Fail("Should not be called");
            return "fail";
        }));

        var oks = 0;
        var errs = 0;
        greetErr.Inspect(_ => oks++).InspectErr(_ => errs++);
        Assert.Equal(0, oks);
        Assert.Equal(1, errs);

        var optOk = greetErr.Ok();
        Assert.False(optOk.IsSome());
        Assert.True(optOk.IsNone());

        var optErr = greetErr.Err();
        Assert.True(optErr.IsSome());
        Assert.False(optErr.IsNone());
        Assert.True(optErr.Expect("Expected error") is ArgumentNullException);
    }

    [Fact]
    public void TestResultOk()
    {
        var greetOk = Greet("world");
        Assert.True(greetOk.IsOk());
        Assert.False(greetOk.IsErr());
        Assert.Equal("Hello, world", greetOk.Expect("Expected a value"));
        try { greetOk.ExpectErr("Should throw"); }
        catch (UnwrapException e) { Assert.Equal("Should throw", e.Message); }
        Assert.Equal("Hello, world", greetOk.Unwrap());
        try { greetOk.UnwrapErr(); }
        catch (UnwrapException) { }
        Assert.Equal("Hello, world", greetOk.UnwrapOr("fallback"));
        Assert.Equal("Hello, world", greetOk.UnwrapOrElse((ex) =>
        {
            Assert.Fail("Should not be called");
            return "fallback";
        }));
        Assert.Equal("hiii", greetOk.MapOr("fallback", (value) =>
        {
            Assert.Equal("Hello, world", value);
            return "hiii";
        }));
        Assert.Equal("mapped", greetOk.MapOrElse((err) =>
        {
            Assert.Fail("Should not be called");
            return "fallback";
        }, (value) =>
        {
            Assert.Equal("Hello, world", value);
            return "mapped";
        }));
        Assert.Equal("bleh", greetOk.Map((value) =>
        {
            Assert.Equal("Hello, world", value);
            return "bleh";
        }).Unwrap());
        Assert.Equal("Hello, world", greetOk.Inspect((value) =>
        {
            Assert.Equal("Hello, world", value);
        }).Unwrap());

        var oks = 0;
        var errs = 0;
        greetOk.Inspect(_ => oks++).InspectErr(_ => errs++);
        Assert.Equal(1, oks);
        Assert.Equal(0, errs);

        var optOk = greetOk.Ok();
        Assert.True(optOk.IsSome());
        Assert.False(optOk.IsNone());
        Assert.Equal("Hello, world", optOk.Expect("Expected value"));

        var optErr = greetOk.Err();
        Assert.False(optErr.IsSome());
        Assert.True(optErr.IsNone());
    }
}