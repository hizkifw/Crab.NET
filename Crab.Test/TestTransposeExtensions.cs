using Crab.Errors;

namespace Crab.Test;

public class TestTransposeExtensions
{
    [Fact]
    public async Task TestTransposeToTaskOption()
    {
        var option = Option.Some(Task.FromResult(1));
        var optionValue = await option.ToTaskOption();
        Assert.Equal(1, optionValue.Unwrap());

        var optionNone = Option.None<Task<int>>();
        var optionNoneValue = await optionNone.ToTaskOption();
        Assert.True(optionNoneValue.IsNone());
    }

    [Fact]
    public async Task TestTransposeToTaskResult()
    {
        var rb = Result.Builder<Task<int>, Exception>();
        var result = rb.Ok(Task.FromResult(1));
        var resultValue = await result.ToTaskResult();
        Assert.Equal(1, resultValue.Unwrap());

        var resultErr = rb.Err(new Exception());
        var resultErrValue = await resultErr.ToTaskResult();
        Assert.True(resultErrValue.IsErr());
    }

    [Fact]
    public void TestFlatten()
    {
        var option = Option.Some(Option.Some(1));
        var optionValue = option.Flatten();
        Assert.True(optionValue.IsSome());

        var optionNone = Option.None<IOption<int>>();
        var optionNoneValue = optionNone.Flatten();
        Assert.True(optionNoneValue.IsNone());

        var optionInnerNone = Option.Some(Option.None<int>());
        var optionInnerNoneValue = optionInnerNone.Flatten();
        Assert.True(optionInnerNoneValue.IsNone());
    }
}