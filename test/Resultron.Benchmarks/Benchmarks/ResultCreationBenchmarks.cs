using BenchmarkDotNet.Attributes;

namespace Resultron.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[RankColumn]
public class ResultCreationBenchmarks
{
    private readonly Error _error =
        new("ERR_CODE", "An error occurred.");

    [Benchmark(Baseline = true)]
    public Result Success() => Result.Success();

    [Benchmark]
    public Result Failure() => Result.Failure(_error);

    [Benchmark]
    public Result<int> GenericSuccess() => Result<int>.Success(42);

    [Benchmark]
    public Result<int> GenericFailure() => Result<int>.Failure(_error);
}
