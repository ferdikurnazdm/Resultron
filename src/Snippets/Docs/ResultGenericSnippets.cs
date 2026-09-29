using Resultron;

namespace Snippets.Docs;

public static class ResultGenericSnippets
{
    public static void Success()
    {
        Result<int> result = Result<int>.Success(42);

        Console.WriteLine(result.IsSuccess);
        Console.WriteLine(result.Value);
    }

    public static void Failure()
    {
        var error = new Error(
            "user.not_found",
            "The requested user was not found.");

        Result<int> result =
            Result<int>.Failure(error);

        Console.WriteLine(result.IsFailure);
        Console.WriteLine(result.Error.Code);
        Console.WriteLine(result.Error.Description);
    }

    public static void FailureWithMetadata()
    {
        var error = new Error(
            "operation.failed",
            "The operation could not be completed.")
            .WithMetadata("Operation", "GetUser");

        Result<int> result =
            Result<int>.Failure(error);

        Console.WriteLine(result.Error.Code);
        Console.WriteLine(result.Error.Description);

        if (result.Error.Metadata.TryGetValue(
            "Operation",
            out var operation))
        {
            Console.WriteLine(operation);
        }
    }

    public static void ImplicitConversionFromValue()
    {
        Result<int> result = 42;

        Console.WriteLine(result.IsSuccess);
        Console.WriteLine(result.Value);
    }

    public static void ImplicitConversionFromError()
    {
        var error = new Error(
            "user.not_found",
            "The requested user was not found.");

        Result<int> result = error;

        Console.WriteLine(result.IsFailure);
        Console.WriteLine(result.Error.Code);
    }

    public static Result<int> ReturnValue()
    {
        return 42;
    }

    public static Result<int> ReturnError()
    {
        return new Error(
            "user.not_found",
            "The requested user was not found.");
    }

    public static Result<int> ReturnSuccess()
    {
        return Result<int>.Success(42);
    }

    public static Result<int> ReturnFailure()
    {
        return Result<int>.Failure(
            new Error(
                "user.not_found",
                "The requested user was not found."));
    }

    public static void OnSuccess()
    {
        Result<int> result =
            Result<int>.Success(42);

        Result<int> afterChain = result.OnSuccess(
            value => Console.WriteLine($"Success: {value}"));

        int value = afterChain.Value;

        Console.WriteLine(value);
    }

    public static void OnFailure()
    {
        Result<int> result =
            Result<int>.Failure(
                new Error(
                    "operation.failed",
                    "The operation failed."));

        Result<int> afterChain = result.OnFailure(
            error => Console.WriteLine(error.Description));

        Console.WriteLine(afterChain.IsFailure);
    }
}