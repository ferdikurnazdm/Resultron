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

    public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return IsSuccess
            ? onSuccess()
            : onFailure(Error);
    }




    public Result<T> Map<T>(Func<T> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return IsSuccess
            ? Result<T>.Success(func())
            : Result<T>.Failure(Error);
    }



    public Result Bind(Func<Result> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return IsSuccess
            ? func()
            : Result.Failure(Error);
    }

    public Result<T> Bind<T>(Func<Result<T>> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return IsSuccess
            ? func()
            : Result<T>.Failure(Error);
    }
}
