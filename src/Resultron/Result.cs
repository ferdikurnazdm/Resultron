namespace Resultron;

/// <summary>
/// Represents the outcome of an operation that does not return a value, 
/// inheriting from <see cref="BaseResult"/> to provide success/failure states and reasons.
/// </summary>
public sealed partial class Result : BaseResult
{
    private Result(bool isSuccess, IEnumerable<IReason>? reasons = null)
        : base(isSuccess, reasons) { }

    /// <summary>
    /// Creates a new successful <see cref="Result"/> instance with no specific reasons.
    /// </summary>
    /// <returns>A successful <see cref="Result"/>.</returns>
    public static Result Success() => new(true);

    /// <summary>
    /// Creates a new successful <see cref="Result"/> instance containing the specified success reason.
    /// </summary>
    /// <param name="success">The success reason (<see cref="Resultron.Success"/>) to attach.</param>
    /// <returns>A successful <see cref="Result"/> with the given reason.</returns>
    public static Result Success(Success success) => new(true, [success]);

    /// <summary>
    /// Creates a new successful <see cref="Result"/> instance containing a collection of reasons.
    /// </summary>
    /// <param name="reasons">A collection of reasons (<see cref="IReason"/>) to attach.</param>
    /// <returns>A successful <see cref="Result"/> with the given reasons.</returns>
    public static Result Success(IEnumerable<IReason> reasons) => new(true, reasons);

    /// <summary>
    /// Creates a new failed <see cref="Result"/> instance containing the specified error.
    /// </summary>
    /// <param name="error">The error (<see cref="Error"/>) that caused the failure.</param>
    /// <returns>A failed <see cref="Result"/> containing the error.</returns>
    public static Result Failure(Error error) => new(false, [error]);

    /// <summary>
    /// Creates a new failed <see cref="Result"/> instance containing a collection of errors.
    /// </summary>
    /// <param name="errors">A collection of errors (<see cref="Error"/>) that caused the failure.</param>
    /// <returns>A failed <see cref="Result"/> containing the errors.</returns>
    public static Result Failure(IEnumerable<Error> errors) => new(false, errors);

    /// <summary>
    /// Creates a new failed <see cref="Result"/> instance containing a collection of failure reasons.
    /// </summary>
    /// <param name="reasons">A collection of reasons (<see cref="IReason"/>) associated with the failure.</param>
    /// <returns>A failed <see cref="Result"/> containing the reasons.</returns>
    public static Result Failure(IEnumerable<IReason> reasons) => new(false, reasons);

    public static implicit operator Result(Error error)
        => Failure(error);
}