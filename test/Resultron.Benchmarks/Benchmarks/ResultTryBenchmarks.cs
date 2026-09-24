using BenchmarkDotNet.Attributes;

namespace Resultron.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[RankColumn]
public class ResultTryBenchmarks
{
    [Benchmark(Baseline = true)]
    public Result TrySuccess()
    {
        return Result.Try(() =>
        {
        });
    }

    [Benchmark]
    public Result TryFailure()
    {
        return Result.Try(() =>
        {
            throw new InvalidOperationException(
                "Simulated error.");
        });
    }

    [Benchmark]
    public Result<int> GenericTrySuccess() => Result<int>.Try(() => 42);

    [Benchmark]
    public Result<int> GenericTryFailure()
    {
        return Result<int>.Try(() =>
        {
            throw new InvalidOperationException(
                "Simulated error.");
        });
    }
}
