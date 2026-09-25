using Resultron;

namespace Snippets.Docs;

public static class ConversionSnippets
{
    public static void ResultToUnitResult()
    {
        Result source = Result.Success();

        Result<Unit> result =
            source.ToUnitResult();

        Unit value = result.Value;

        Console.WriteLine(value);
    }

    public static void ResultOfTToUnitResult()
    {
        Result<int> source =
            Result<int>.Success(42);

        Result<Unit> result =
            source.ToUnitResult();

        Unit value = result.Value;

        Console.WriteLine(value);
    }

    public static void ResultOfTToNonGenericResult()
    {
        Result<int> source =
            Result<int>.Success(42);

        Result result =
            source.ToNonGenericResult();

        Console.WriteLine(result.IsSuccess);
    }

    public static async Task TaskResultToUnitResult()
    {
        Task<Result> source =
            Task.FromResult(Result.Success());

        Result<Unit> result =
            await source.ToUnitResultAsync();

        Console.WriteLine(result.Value);
    }

    public static async Task TaskResultOfTToUnitResult()
    {
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Success(42));

        Result<Unit> result =
            await source.ToUnitResultAsync();

        Console.WriteLine(result.Value);
    }

    public static async Task TaskResultOfTToNonGenericResult()
    {
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Success(42));

        Result result =
            await source.ToNonGenericResultAsync();

        Console.WriteLine(result.IsSuccess);
    }

    public static Result<Unit> DiscardValue()
    {
        Result<int> result =
            Result<int>.Success(42);

        return result.ToUnitResult();
    }

    public static Result DiscardValueAndType()
    {
        Result<int> result =
            Result<int>.Success(42);

        return result.ToNonGenericResult();
    }


}