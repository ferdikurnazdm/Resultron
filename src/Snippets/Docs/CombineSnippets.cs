using Resultron;

namespace Snippets.Docs;

public static class CombineSnippets
{
    public static void CombineResults()
    {
        Result[] results =
        [
            Result.Success(),
            Result.Success(),
            Result.Success()
        ];

        Result combined = results.Combine();

        Console.WriteLine(combined.IsSuccess);
    }

    public static void CombineResultsWithFailure()
    {
        Result[] results =
        [
            Result.Success(),

            Result.Failure(
                new Error(
                    "user.not_found",
                    "The requested user was not found.")),

            Result.Failure(
                new Error(
                    "order.not_found",
                    "The requested order was not found."))
        ];

        Result combined = results.Combine();

        Console.WriteLine(combined.IsFailure);
        Console.WriteLine(combined.Errors.Count);
    }

    public static void CombineTypedResults()
    {
        Result<int>[] results =
        [
            Result<int>.Success(10),
            Result<int>.Success(20),
            Result<int>.Success(30)
        ];

        Result<IReadOnlyList<int>> combined =
            results.Combine();

        foreach (int value in combined.Value)
        {
            Console.WriteLine(value);
        }
    }

    public static void CombineTypedResultsWithFailure()
    {
        Result<int>[] results =
        [
            Result<int>.Success(10),

            Result<int>.Failure(
                new Error(
                    "value.invalid",
                    "The value is invalid.")),

            Result<int>.Failure(
                new Error(
                    "value.out_of_range",
                    "The value is outside the allowed range."))
        ];

        Result<IReadOnlyList<int>> combined =
            results.Combine();

        Console.WriteLine(combined.IsFailure);
        Console.WriteLine(combined.Errors.Count);
    }

    public static async Task CombineResultsAsync()
    {
        Task<Result>[] tasks =
        [
            Task.FromResult(Result.Success()),
            Task.FromResult(Result.Success()),
            Task.FromResult(Result.Success())
        ];

        Result combined =
            await tasks.CombineAsync();

        Console.WriteLine(combined.IsSuccess);
    }

    public static async Task CombineTypedResultsAsync()
    {
        Task<Result<int>>[] tasks =
        [
            Task.FromResult(Result<int>.Success(10)),
            Task.FromResult(Result<int>.Success(20)),
            Task.FromResult(Result<int>.Success(30))
        ];

        Result<IReadOnlyList<int>> combined =
            await tasks.CombineAsync();

        foreach (int value in combined.Value)
        {
            Console.WriteLine(value);
        }
    }

    public static Result ValidateUser()
    {
        Result[] validations =
        [
            ValidateName(),
            ValidateEmail(),
            ValidateAge()
        ];

        return validations.Combine();
    }

    private static Result ValidateName() =>
        Result.Success();

    private static Result ValidateEmail() =>
        Result.Success();

    private static Result ValidateAge() =>
        Result.Success();

    public static Result<IReadOnlyList<int>> LoadValues()
    {
        Result<int>[] results =
        [
            Result<int>.Success(10),
            Result<int>.Success(20),
            Result<int>.Success(30)
        ];

        return results.Combine();
    }

    public static async Task CombineResultTasks()
    {
        Task<Result>[] tasks =
        [
            Task.FromResult(Result.Success()),
            Task.FromResult(Result.Success())
        ];

        Result combined =
            await tasks.CombineAsync();

        Console.WriteLine(combined.IsSuccess);
    }
}