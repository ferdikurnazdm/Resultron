using BenchmarkDotNet.Attributes;

namespace Resultron.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[RankColumn]
public class ResultChainBenchmarks
{
    [Benchmark(Baseline = true)]
    public async Task FullChainSuccess()
    {
        var result = await Result<int>.TryAsync(
            () => Task.FromResult(42));

        var mapped = result.Map(
            value => value * 2);

        var bound = mapped.Bind(
            value => Result<int>.Success(value + 10));

        _ = await Task.FromResult(bound)
            .MatchAsync(
                onSuccess: _ => 0,
                onFailure: _ => 0);
    }

    [Benchmark]
    public async Task FullChainFailure()
    {
        var result = await Result<int>.TryAsync(
            () => Task.FromException<int>(
                new InvalidOperationException(
                    "Simulated error.")));

        var mapped = result.Map(
            value => value * 2);

        var bound = mapped.Bind(
            value => Result<int>.Success(value + 10));

        _ = await Task.FromResult(bound)
            .MatchAsync(
                onSuccess: _ => 0,
                onFailure: _ => 0);
    }
}
