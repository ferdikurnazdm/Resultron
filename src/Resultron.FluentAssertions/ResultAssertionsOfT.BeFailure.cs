using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions<T>
{
    /// <summary>
    /// Asserts that the generic result is a failure.
    /// </summary>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format the because parameter.</param>
    /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain more assertions.</returns>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> BeFailure(
        string because = "", 
        params object[] becauseArgs)
    {
        Subject.Should().NotBeNull(because, becauseArgs);

        Subject.IsSuccess.Should().BeFalse(
            "Expected result to be a failure, but it succeeded with value: {0}",
            Subject.Value); // İsteğe bağlı olarak taşınan değeri de hata mesajına ekleyebiliriz

        return new AndConstraint<ResultAssertions<T>>(this);
    }
}
