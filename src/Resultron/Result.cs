namespace Resultron;

public sealed class Result : BaseResult
{
    private Result(bool isSuccess, Error error) : base(isSuccess, error) { }


    public static Result Success()
    {
        ResultLogger.Debug(
            message: "Result created", 
            code: "SUCCESS");

        return new(true, Error.None);
    }

    public static Result Failure(Error error)
    {
        ResultLogger.Debug(
            message: "Result failed",
            code: error.Code,
            description: error.Description);

        return new Result(false, error);
    }




    public static implicit operator Result(Error error) => Failure(error);



    public static Result Try(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        ResultLogger.Debug("Try started");

        try
        {
            action();

            ResultLogger.Debug(
                message: "Try completed",
                code: "SUCCESS");

            return Success();
        }
        catch (Exception ex)
        {
            ResultLogger.Debug(
                message: "Try caught exception",
                code: ex.GetType().Name,
                description: ex.Message);

            return Failure(new Error(
                Code: ex.GetType().Name, 
                Description: ex.Message));
        }
    }

    public static async Task<Result> TryAsync(Func<Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        ResultLogger.Debug("TryAsync started");

        try
        {
            await action();

            ResultLogger.Debug(
                message: "TryAsync completed",
                code: "SUCCESS");

            return Success();
        }
        catch (Exception ex)
        {
            ResultLogger.Debug(
                message: "TryAsync caught exception",
                code: ex.GetType().Name,
                description: ex.Message);

            return Failure(new Error(
                Code: ex.GetType().Name, 
                Description: ex.Message));
        }
    }



    public void Match(Action onSuccess, Action<Error> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);

        ArgumentNullException.ThrowIfNull(onFailure);

        ResultLogger.Debug(
            message: "Match started",
            code: IsSuccess ? "SUCCESS" : Error.Code,
            description: IsSuccess ? null : Error.Description);

        if (IsSuccess)
        {
            onSuccess();

            ResultLogger.Debug(
                message: "Match success branch completed",
                code: "SUCCESS");

            return;
        }

        onFailure(Error);

        ResultLogger.Debug(
            message: "Match failure branch completed",
            code: Error.Code,
            description: Error.Description);
    }

    public async Task MatchAsync(
        Func<Task> onSuccess, 
        Func<Error, Task> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);

        ArgumentNullException.ThrowIfNull(onFailure);

        ResultLogger.Debug(
            message: "MatchAsync started",
            code: IsSuccess ? "SUCCESS" : Error.Code,
            description: IsSuccess ? null : Error.Description);

        if (IsSuccess)
        {
            await onSuccess();

            ResultLogger.Debug(
                message: "MatchAsync success branch completed",
                code: "SUCCESS");

            return;
        }

        await onFailure(Error);

        ResultLogger.Debug(
            message: "MatchAsync failure branch completed",
            code: Error.Code,
            description: Error.Description);
    }

    public TResult Match<TResult>(
        Func<TResult> onSuccess, 
        Func<Error, TResult> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);

        ArgumentNullException.ThrowIfNull(onFailure);

        ResultLogger.Debug(
            message: "Match<TResult> started",
            code: IsSuccess ? "SUCCESS" : Error.Code,
            description: IsSuccess ? null : Error.Description);

        return IsSuccess
            ? onSuccess()
            : onFailure(Error);
    }

    public async Task<TResult> MatchAsync<TResult>(
        Func<Task<TResult>> onSuccess, 
        Func<Error, Task<TResult>> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);

        ArgumentNullException.ThrowIfNull(onFailure);

        ResultLogger.Debug(
            message: "MatchAsync<TResult> started",
            code: IsSuccess ? "SUCCESS" : Error.Code,
            description: IsSuccess ? null : Error.Description);

        return IsSuccess
            ? await onSuccess()
            : await onFailure(Error);
    }



    public Result<T> Map<T>(Func<T> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        ResultLogger.Debug(
            message: "Map started",
            code: IsSuccess ? "SUCCESS" : Error.Code,
            description: IsSuccess ? null : Error.Description);

        return IsSuccess
            ? Result<T>.Success(func())
            : Result<T>.Failure(Error);
    }

    public async Task<Result<T>> MapAsync<T>(Func<Task<T>> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        if (!IsSuccess)
            return Result<T>.Failure(Error);

        return Result<T>.Success(await func());
    }



    public Result Bind(Func<Result> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        ResultLogger.Debug(
            message: "Bind started",
            code: IsSuccess ? "SUCCESS" : Error.Code,
            description: IsSuccess ? null : Error.Description);

        return IsSuccess
            ? func()
            : Result.Failure(Error);
    }

    public async Task<Result> BindAsync(Func<Task<Result>> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return IsSuccess
            ? await func()
            : Result.Failure(Error);
    }

    public Result<T> Bind<T>(Func<Result<T>> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return IsSuccess
            ? func()
            : Result<T>.Failure(Error);
    }

    public async Task<Result<T>> BindAsync<T>(Func<Task<Result<T>>> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return IsSuccess
            ? await func()
            : Result<T>.Failure(Error);
    }

}
