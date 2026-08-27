namespace Resultron;

public sealed class Result : BaseResult
{
    private Result(bool isSuccess, Error error) : base(isSuccess, error) { }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    public static implicit operator Result(Error error) => Failure(error);



    public static Result Try(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        try
        {
            action();
            return Success();
        }
        catch (Exception ex)
        {
            return Failure(new Error(Code: ex.GetType().Name, Description: ex.Message));
        }
    }

    public static async Task<Result> TryAsync(Func<Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        try
        {
            await action();
            return Success();
        }
        catch (Exception ex)
        {
            return Failure(new Error(Code: ex.GetType().Name, Description: ex.Message));
        }
    }



    public void Match(Action onSuccess, Action<Error> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);

        ArgumentNullException.ThrowIfNull(onFailure);

        if (IsSuccess)
        {
            onSuccess();
            return;
        }

        onFailure(Error);
    }

    public async Task MatchAsync(Func<Task> onSuccess, Func<Error, Task> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);

        ArgumentNullException.ThrowIfNull(onFailure);

        if (IsSuccess)
        {
            await onSuccess();

            return;
        }

        await onFailure(Error);
    }

    public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);

        ArgumentNullException.ThrowIfNull(onFailure);

        return IsSuccess
            ? onSuccess()
            : onFailure(Error);
    }

    public async Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);

        ArgumentNullException.ThrowIfNull(onFailure);

        return IsSuccess
            ? await onSuccess()
            : await onFailure(Error);
    }



    public Result<T> Map<T>(Func<T> func)
    {
        ArgumentNullException.ThrowIfNull(func);

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
