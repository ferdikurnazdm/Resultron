using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions
{
    /// <summary>
    /// Asserts that the result contains an error with an exception of type <typeparamref name="TException"/>.
    /// </summary>
    /// <typeparam name="TException">The expected exception type.</typeparam>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format the because parameter.</param>
    [CustomAssertion]
    public AndConstraint<ResultAssertions> HaveException<TException>(
        string because = "", 
        params object[] becauseArgs) where TException : Exception
    {
        Subject.Should().NotBeNull(because, becauseArgs);
        Subject.IsSuccess.Should().BeFalse("Expected result to be a failure, but it succeeded.");
        
        Subject.Error.Should().NotBeNull("Expected result to have an error, but Error was null.");
        
        Subject.Error!.Exception.Should().NotBeNull(
            "Expected error to contain an exception of type {0}, but exception was null.", 
            typeof(TException).Name);

        Subject.Error.Exception.Should().BeOfType<TException>(because, becauseArgs);

        return new AndConstraint<ResultAssertions>(this);
    }

    /// <summary>
    /// Asserts that the result contains an error with the specified metadata key and value.
    /// </summary>
    /// <param name="key">The metadata key.</param>
    /// <param name="expectedValue">The expected value associated with the metadata key.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format the because parameter.</param>
    [CustomAssertion]
    public AndConstraint<ResultAssertions> HaveMetadata(
        string key,
        object expectedValue,
        string because = "", 
        params object[] becauseArgs)
    {
        Subject.Should().NotBeNull(because, becauseArgs);
        Subject.IsSuccess.Should().BeFalse("Expected result to be a failure, but it succeeded.");
        
        Subject.Error.Should().NotBeNull("Expected result to have an error, but Error was null.");
        
        Subject.Error!.Metadata.Should().NotBeNull("Expected error metadata to be initialized, but it was null.");
        Subject.Error.Metadata.Should().ContainKey(key, because, becauseArgs);
        Subject.Error.Metadata[key].Should().Be(expectedValue, because, becauseArgs);

        return new AndConstraint<ResultAssertions>(this);
    }
}
