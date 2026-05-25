namespace Resultron;

public abstract class BaseResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected BaseResult(bool isSuccess, Error error)
    {
        if (IsBadResultCondition(isSuccess, error))
            throw new ArgumentException("Invalid Error", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
    }


    private static bool IsBadResultCondition(bool isSuccess, Error error)
    {
        return (isSuccess && error != Error.None) ||
              (!isSuccess && error == Error.None);
    }
}
