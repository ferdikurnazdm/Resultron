using BenchmarkDotNet.Attributes;

namespace Resultron.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[RankColumn]
public class ResultGenericBenchmarks
{
    private Result<int> _success = null!;
    private Result<int> _failure = null!;

    [GlobalSetup]
    public void Setup()
    {
        _success = Result<int>.Success(42);

        _failure = Result<int>.Failure(
            new Error(
                "ERR_CODE",
                "An error occurred."));
    }

    [Benchmark(Baseline = true)]
    public Result<int> MapSuccess()
    {
        return _success.Map(
            value => value * 2);
    }

    [Benchmark]
    public Result<int> MapFailure()
    {
        return _failure.Map(
            value => value * 2);
    }

    [Benchmark]
    public Result<int> BindSuccess()
    {
        return _success.Bind(
            value => Result<int>.Success(value * 2));
    }

    [Benchmark]
    public Result<int> BindFailure()
    {
        return _failure.Bind(
            value => Result<int>.Success(value * 2));
    }
}
