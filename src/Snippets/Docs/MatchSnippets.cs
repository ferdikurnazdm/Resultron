using Resultron;

namespace Snippets.Docs;

public static class MatchSnippets
{
    public static void ResultMatch()
    {
        Result result = Result.Success();

        result.Match(
            onSuccess: () =>
                Console.WriteLine("Operation succeeded."),
            onFailure: error =>
                Console.WriteLine(error.Description));
    }

    public static void ResultOfTMatch()
    {
        Result<int> result = Result<int>.Success(42);

        result.Match(
            onSuccess: value =>
                Console.WriteLine($"Value: {value}"),
            onFailure: error =>
                Console.WriteLine(error.Description));
    }

    public static void ResultMatchWithReturnValue()
    {
        Result result = Result.Success();

        string message = result.Match(
            onSuccess: () => "Operation succeeded.",
            onFailure: error => error.Description);

        Console.WriteLine(message);
    }

    public static void ResultOfTMatchWithReturnValue()
    {
        Result<int> result = Result<int>.Success(42);

        string message = result.Match(
            onSuccess: value => $"Value: {value}",
            onFailure: error => error.Description);

        Console.WriteLine(message);
    }

    public static async Task ResultMatchAsync()
    {
        Result result = Result.Success();

        await result.MatchAsync(
            onSuccess: async () =>
            {
                await Task.Delay(10);

                Console.WriteLine("Operation succeeded.");
            },
            onFailure: async error =>
            {
                await Task.Delay(10);

                Console.WriteLine(error.Description);
            });
    }

    public static async Task ResultOfTMatchAsync()
    {
        Result<int> result = Result<int>.Success(42);

        await result.MatchAsync(
            onSuccess: async value =>
            {
                await Task.Delay(10);

                Console.WriteLine($"Value: {value}");
            },
            onFailure: async error =>
            {
                await Task.Delay(10);

                Console.WriteLine(error.Description);
            });
    }

    public static async Task ResultMatchAsyncWithReturnValue()
    {
        Result result = Result.Success();

        string message = await result.MatchAsync(
            onSuccess: async () =>
            {
                await Task.Delay(10);

                return "Operation succeeded.";
            },
            onFailure: async error =>
            {
                await Task.Delay(10);

                return error.Description;
            });

        Console.WriteLine(message);
    }

    public static async Task ResultOfTMatchAsyncWithReturnValue()
    {
        Result<int> result = Result<int>.Success(42);

        string message = await result.MatchAsync(
            onSuccess: async value =>
            {
                await Task.Delay(10);

                return $"Value: {value}";
            },
            onFailure: async error =>
            {
                await Task.Delay(10);

                return error.Description;
            });

        Console.WriteLine(message);
    }

    public static async Task TaskResultMatch()
    {
        Task<Result> resultTask =
            Task.FromResult(Result.Success());

        string message = await resultTask.MatchAsync(
            onSuccess: () => "Operation succeeded.",
            onFailure: error => error.Description);

        Console.WriteLine(message);
    }

    public static async Task TaskResultMatchAsync()
    {
        Task<Result> resultTask =
            Task.FromResult(Result.Success());

        string message = await resultTask.MatchAsync(
            onSuccess: async () =>
            {
                await Task.Delay(10);

                return "Operation succeeded.";
            },
            onFailure: async error =>
            {
                await Task.Delay(10);

                return error.Description;
            });

        Console.WriteLine(message);
    }

    public static async Task TaskResultOfTMatch()
    {
        Task<Result<int>> resultTask =
            Task.FromResult(Result<int>.Success(42));

        string message = await resultTask.MatchAsync(
            onSuccess: value => $"Value: {value}",
            onFailure: error => error.Description);

        Console.WriteLine(message);
    }

    public static async Task TaskResultOfTMatchAsync()
    {
        Task<Result<int>> resultTask =
            Task.FromResult(Result<int>.Success(42));

        string message = await resultTask.MatchAsync(
            onSuccess: async value =>
            {
                await Task.Delay(10);

                return $"Value: {value}";
            },
            onFailure: async error =>
            {
                await Task.Delay(10);

                return error.Description;
            });

        Console.WriteLine(message);
    }

    public static async Task TaskResultMatchActions()
    {
        Task<Result> resultTask =
            Task.FromResult(Result.Success());

        await resultTask.MatchAsync(
            onSuccess: () =>
                Console.WriteLine("Operation succeeded."),
            onFailure: error =>
                Console.WriteLine(error.Description));
    }

    public static async Task TaskResultMatchAsyncActions()
    {
        Task<Result> resultTask =
            Task.FromResult(Result.Success());

        await resultTask.MatchAsync(
            onSuccess: async () =>
            {
                await Task.Delay(10);
                Console.WriteLine("Operation succeeded.");
            },
            onFailure: async error =>
            {
                await Task.Delay(10);
                Console.WriteLine(error.Description);
            });
    }

    public static async Task TaskResultOfTMatchActions()
    {
        Task<Result<int>> resultTask =
            Task.FromResult(Result<int>.Success(42));

        await resultTask.MatchAsync(
            onSuccess: value =>
                Console.WriteLine($"Value: {value}"),
            onFailure: error =>
                Console.WriteLine(error.Description));
    }

    public static async Task TaskResultOfTMatchAsyncActions()
    {
        Task<Result<int>> resultTask =
            Task.FromResult(Result<int>.Success(42));

        await resultTask.MatchAsync(
            onSuccess: async value =>
            {
                await Task.Delay(10);
                Console.WriteLine($"Value: {value}");
            },
            onFailure: async error =>
            {
                await Task.Delay(10);
                Console.WriteLine(error.Description);
            });
    }

}