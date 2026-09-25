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

    public static void SuccessWithReason()
    {
        var success = new Success(
            "User retrieved successfully.");

        Result<int> result =
            Result<int>.Success(42, success);

        Console.WriteLine(result.Value);
        Console.WriteLine(result.Successes[0].Message);
    }

    public static void SuccessWithReasons()
    {
        IReason[] reasons =
        [
            new Success("User retrieved successfully.")
                .WithMetadata("Source", "Database"),

            new Success("Result loaded from primary storage.")
        ];

        Result<int> result =
            Result<int>.Success(42, reasons);

        Console.WriteLine(result.Value);

        foreach (IReason reason in result.Reasons)
        {
            Console.WriteLine(reason.Message);
        }
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
    }

    public static void FailureWithErrors()
    {
        Error[] errors =
        [
            new(
                "validation.failed",
                "Validation failed."),

            new(
                "value.out_of_range",
                "The supplied value is outside the allowed range.")
        ];

        Result<int> result =
            Result<int>.Failure(errors);

        foreach (Error error in result.Errors)
        {
            Console.WriteLine(
                $"{error.Code}: {error.Description}");
        }
    }

    public static void FailureWithReasons()
    {
        IReason[] reasons =
        [
            new Error(
                "operation.failed",
                "The operation could not be completed.")
                .WithMetadata("Operation", "GetUser")
        ];

        Result<int> result =
            Result<int>.Failure(reasons);

        foreach (IReason reason in result.Reasons)
        {
            Console.WriteLine(reason.Message);
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
        Result<int> result = Result<int>.Success(42);

        Result<int> afterChain = result.OnSuccess(
            value => Console.WriteLine($"başarılı: {value}"));

        int value = afterChain.Value;

        Console.WriteLine(value);
    }

    public static void OnFailure()
    {
        Result<int> result = Result<int>.Failure(
            new Error(
                "operation.failed",
                "İşlem başarısız."));

        Result<int> afterChain = result.OnFailure(
            error => Console.WriteLine(error.Description));

        Console.WriteLine(afterChain.IsFailure);
    }
}