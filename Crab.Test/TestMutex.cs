using Crab.Sync;

namespace Crab.Test;

public class TestMutex
{
    [Fact]
    public async Task TestMutex1()
    {
        using var mutex = new Mutex<List<int>>([1, 2, 3]);

        var tasks = new List<Task>();
        for (var i = 0; i < 10; i++)
        {
            Thread.Sleep(1);
            tasks.Add(Task.Run(async () =>
            {
                for (var j = 0; j < 1000; j++)
                {
                    using (var guard = await mutex.LockAsync())
                    {
                        while (guard.Value.Count >= 10)
                            guard.Value.RemoveAt(0);
                        guard.Value.Add(j);
                    }
                    if (j % 10 == 0) Thread.Sleep(1);
                }
            }));
        }
        await Task.WhenAll(tasks);

        using (var guard = mutex.Lock())
        {
            Assert.Equal(10, guard.Value.Count);
            Assert.Equal(999, guard.Value[^1]);
        }

        var sum = await mutex.MapAsync((inp) => inp.Sum());
        Assert.True(sum > 9000);

        var list = await mutex.SetAsync(inp => { inp.Add(123); return inp; });
        Assert.Equal(11, list.Count);
        using (var guard = mutex.Lock())
        {
            Assert.Equal(11, guard.Value.Count);
            Assert.Equal(123, guard.Value[^1]);
        }
    }
}