using BenchmarkDotNet.Attributes;

namespace Resultron.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[RankColumn]
public class ResultAsyncBenchmarks
{

    [Benchmark(Baseline = true)]
    public Task<Result> TryAsync_CompletedTask()
    {
        return Result.TryAsync(
            () => Task.CompletedTask);
    }

    [Benchmark]
    public Task<Result> TryAsync_TaskYield()
    {
        return Result.TryAsync(async () =>
        {
            await Task.Yield();
        });
    }

    [Benchmark]
    public Task<Result> TryAsync_Failure()
    {
        return Result.TryAsync(async () =>
        {
            await Task.Yield();

            throw new InvalidOperationException(
                "Simulated error.");
        });
    }

    [Benchmark]
    public Task<Result<int>> GenericTryAsync_CompletedTask()
    {
        return Result<int>.TryAsync(
            () => Task.FromResult(42));
    }

    [Benchmark]
    public Task<Result<int>> GenericTryAsync_TaskYield()
    {
        return Result<int>.TryAsync(async () =>
        {
            await Task.Yield();

            return 42;
        });
    }

    [Benchmark]
    public Task<Result<int>> GenericTryAsync_Failure()
    {
        return Result<int>.TryAsync(async () =>
        {
            await Task.Yield();

            throw new InvalidOperationException(
                "Simulated error.");
        });
    }
}
