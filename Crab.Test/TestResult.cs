using Crab.Errors;

namespace Crab.Test;

public class TestResult
{
    private static IResult<string, Exception> Greet(string input)
    {
        if (string.IsNullOrEmpty(input))
            return Result.Err<string, Exception>(new ArgumentNullException(nameof(input)));

        return Result.Ok<string, Exception>($"Hello, {input}");
    }

    private static IResult<string, Exception> GreetWithBuilder(string input)
    {
        var rb = Result.Builder<string, Exception>();

        if (string.IsNullOrEmpty(input))
            return rb.Err(new ArgumentNullException(nameof(input)));

        return rb.Ok($"Hello, {input}");
    }

    [Fact]
    public void TestResultErr()
    {
        IEnumerable<IResult<string, Exception>> values = [
            Greet(string.Empty),
            GreetWithBuilder(string.Empty)
        ];

        foreach (var valueErr in values)
        {
            Assert.True(valueErr.IsErr());
            Assert.False(valueErr.IsOk());
            try { valueErr.Expect("Expected a value"); Assert.Fail("Should throw"); }
            catch (UnwrapException e) { Assert.Equal("Expected a value", e.Message); }
            Assert.True(valueErr.ExpectErr("Should be error") is ArgumentNullException);
            try { valueErr.Unwrap(); Assert.Fail("Should throw"); }
            catch (UnwrapException) { }
            Assert.True(valueErr.UnwrapErr() is ArgumentNullException);
            Assert.Equal("fallback", valueErr.UnwrapOr("fallback"));
            Assert.Equal(default, valueErr.UnwrapOrDefault());
            Assert.Equal("fallback", valueErr.UnwrapOrElse((ex) =>
            {
                Assert.True(ex is ArgumentNullException);
                return "fallback";
            }));
            Assert.Equal("fallback", valueErr.MapErr(ex =>
            {
                Assert.True(ex is ArgumentNullException);
                return "fallback";
            }).UnwrapErr());
            Assert.Equal("fallback", valueErr.MapOr("fallback", (value) =>
            {
                Assert.Fail("Should not be called");
                return "fail";
            }));
            Assert.Equal("fallback", valueErr.MapOrElse((err) =>
            {
                Assert.True(err is ArgumentNullException);
                return "fallback";
            }, (value) =>
            {
                Assert.Fail("Should not be called");
                return "fail";
            }));

            valueErr
                .Inspect(_ => Assert.Fail("Should not be called"))
                .InspectErr(err => Assert.True(err is ArgumentNullException));

            var optOk = valueErr.Ok();
            Assert.False(optOk.IsSome());
            Assert.True(optOk.IsNone());

            var optErr = valueErr.Err();
            Assert.True(optErr.IsSome());
            Assert.False(optErr.IsNone());
            Assert.True(optErr.Expect("Expected error") is ArgumentNullException);

            Assert.False(valueErr.TryUnwrap(out var value));
            Assert.Null(value);
            Assert.True(valueErr.TryUnwrapErr(out var err));
            Assert.True(err is ArgumentNullException);
        }
    }

    [Fact]
    public void TestResultOk()
    {
        IEnumerable<IResult<string, Exception>> values = [
            Greet("world"),
            GreetWithBuilder("world")
        ];

        foreach (var valueOk in values)
        {
            Assert.True(valueOk.IsOk());
            Assert.False(valueOk.IsErr());
            Assert.Equal("Hello, world", valueOk.Expect("Expected a value"));
            try { valueOk.ExpectErr("Should throw"); Assert.Fail("Should throw"); }
            catch (UnwrapException e) { Assert.Equal("Should throw", e.Message); }
            Assert.Equal("Hello, world", valueOk.Unwrap());
            try { valueOk.UnwrapErr(); Assert.Fail("Should throw"); }
            catch (UnwrapException) { }
            Assert.Equal("Hello, world", valueOk.UnwrapOr("fallback"));
            Assert.Equal("Hello, world", valueOk.UnwrapOrElse((ex) =>
            {
                Assert.Fail("Should not be called");
                return "fallback";
            }));
            Assert.Equal("hiii", valueOk.MapOr("fallback", (value) =>
            {
                Assert.Equal("Hello, world", value);
                return "hiii";
            }));
            Assert.Equal("mapped", valueOk.MapOrElse((err) =>
            {
                Assert.Fail("Should not be called");
                return "fallback";
            }, (value) =>
            {
                Assert.Equal("Hello, world", value);
                return "mapped";
            }));
            Assert.Equal("bleh", valueOk.Map((value) =>
            {
                Assert.Equal("Hello, world", value);
                return "bleh";
            }).Unwrap());
            Assert.Equal("Hello, world", valueOk.Inspect((value) =>
            {
                Assert.Equal("Hello, world", value);
            }).Unwrap());

            valueOk
                .Inspect(val => Assert.Equal("Hello, world", val))
                .InspectErr(_ => Assert.Fail("Should not be called"));

            var optOk = valueOk.Ok();
            Assert.True(optOk.IsSome());
            Assert.False(optOk.IsNone());
            Assert.Equal("Hello, world", optOk.Expect("Expected value"));

            var optErr = valueOk.Err();
            Assert.False(optErr.IsSome());
            Assert.True(optErr.IsNone());

            Assert.True(valueOk.TryUnwrap(out var value));
            Assert.Equal("Hello, world", value);
            Assert.False(valueOk.TryUnwrapErr(out var err));
            Assert.Null(err);
        }
    }
}