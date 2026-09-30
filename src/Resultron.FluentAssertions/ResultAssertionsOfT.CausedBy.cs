using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions<T>
{
    /// <summary>
    /// Asserts that the generic result's error was caused by an exception of type <typeparamref name="TException"/>.
    /// </summary>
    /// <typeparam name="TException">The expected exception type.</typeparam>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format the because parameter.</param>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> HaveCausedBy<TException>(
        string because = "", 
        params object[] becauseArgs) where TException : Exception
    {
        Subject.Should().NotBeNull(because, becauseArgs);
        Subject.IsSuccess.Should().BeFalse(
            "Expected result to be a failure, but it succeeded with value: {0}", 
            Subject.Value);
        
        Subject.Error.Should().NotBeNull(
            "Expected result to have an error, but Error was null.", 
            becauseArgs);
        
        Subject.Error!.Exception.Should().NotBeNull(
            "Expected error to have an underlying exception (set via CausedBy), but Exception was null.", 
            becauseArgs);

        Subject.Error.Exception.Should().BeOfType<TException>(because, becauseArgs);

        return new AndConstraint<ResultAssertions<T>>(this);
    }
}
