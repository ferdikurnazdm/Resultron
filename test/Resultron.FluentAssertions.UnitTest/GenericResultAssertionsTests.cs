namespace Resultron.FluentAssertions.UnitTest;

public sealed class GenericResultAssertionsTests
{
    private static readonly Error ExpectedError = new("NOT_FOUND", "Item not found");

    [Fact]
    public void BeSuccessful_Should_Pass_When_GenericResultIsSuccessful()
    {
        Result<int>.Success(42).Should().BeSuccessful();
    }

    [Fact]
    public void BeSuccessful_Should_Throw_When_GenericResultIsFailure()
    {
        Action act = () => Result<int>.Failure(ExpectedError).Should().BeSuccessful();
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void BeFailure_Should_Pass_When_GenericResultIsFailure()
    {
        Result<int>.Failure(ExpectedError).Should().BeFailure();
    }

    [Fact]
    public void BeFailure_Should_Throw_When_GenericResultIsSuccessful()
    {
        Action act = () => Result<int>.Success(42).Should().BeFailure();
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void BeFailureWithError_Should_Pass_When_GenericErrorMatches()
    {
        Result<int>.Failure(ExpectedError).Should().BeFailureWithError(ExpectedError);
    }

    [Fact]
    public void BeFailureWithError_Should_Throw_When_GenericErrorDiffers()
    {
        Action act = () => Result<int>.Failure(ExpectedError).Should()
            .BeFailureWithError(new Error("OTHER", "Different"));
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void BeSuccessfulWithValue_Should_Pass_When_ValueMatches()
    {
        Result<int>.Success(42).Should().BeSuccessfulWithValue(42);
    }

    [Fact]
    public void BeSuccessfulWithValue_Should_Throw_When_ValueDiffers()
    {
        Action act = () => Result<int>.Success(42).Should().BeSuccessfulWithValue(99);
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void HaveValue_Should_Pass_When_ValueMatches()
    {
        Result<string>.Success("ready").Should().HaveValue("ready");
    }

    [Fact]
    public void HaveValue_Should_Pass_When_ValueSatisfiesPredicateAssertion()
    {
        Result<int>.Success(42).Should().HaveValue(value => value.Should().BeGreaterThan(0));
    }

    [Fact]
    public void HaveValue_Should_Throw_When_ResultIsFailure()
    {
        Action act = () => Result<int>.Failure(ExpectedError).Should().HaveValue();
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void NotHaveValue_Should_Pass_When_ResultIsFailure()
    {
        Result<int>.Failure(ExpectedError).Should().NotHaveValue();
    }

    [Fact]
    public void NotHaveValue_Should_Throw_When_ResultIsSuccessful()
    {
        Action act = () => Result<int>.Success(42).Should().NotHaveValue();
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void HaveError_Should_Pass_When_GenericResultContainsError()
    {
        Result<int>.Failure(ExpectedError).Should().HaveError();
    }

    [Fact]
    public void HaveError_Should_Pass_When_DescriptionMatches()
    {
        Result<int>.Failure(ExpectedError).Should().HaveError("Item not found");
    }

    [Fact]
    public void HaveError_Should_Throw_When_DescriptionDiffers()
    {
        Action act = () => Result<int>.Failure(ExpectedError).Should().HaveError("Different");
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void HaveErrorCode_Should_Pass_When_GenericErrorCodeMatches()
    {
        Result<int>.Failure(ExpectedError).Should().HaveErrorCode("NOT_FOUND");
    }

    [Fact]
    public void HaveErrorCode_Should_Throw_When_GenericErrorCodeDiffers()
    {
        Action act = () => Result<int>.Failure(ExpectedError).Should().HaveErrorCode("OTHER");
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void NotHaveError_Should_Pass_When_GenericResultIsSuccessful()
    {
        Result<int>.Success(42).Should().NotHaveError();
    }

    [Fact]
    public void NotHaveError_Should_Throw_When_GenericResultIsFailure()
    {
        Action act = () => Result<int>.Failure(ExpectedError).Should().NotHaveError();
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void HaveMetadata_Should_Pass_When_GenericErrorMetadataMatches()
    {
        var error = ExpectedError.WithMetadata("status", 404);
        Result<int>.Failure(error).Should().HaveMetadata("status", 404);
    }

    [Fact]
    public void HaveException_Should_Pass_When_GenericErrorExceptionMatches()
    {
        var error = ExpectedError.CausedBy(new InvalidOperationException("oops"));
        Result<int>.Failure(error).Should().HaveException<InvalidOperationException>();
    }

    [Fact]
    public void HaveCausedBy_Should_Pass_When_GenericErrorCauseMatches()
    {
        var error = ExpectedError.CausedBy(new InvalidOperationException("oops"));
        Result<int>.Failure(error).Should().HaveCausedBy<InvalidOperationException>();
    }

    [Fact]
    public void BeSuccessfulWithValue_Should_Pass_When_SubstitutedServiceReturnsValue()
    {
        var service = Substitute.For<IValueProvider>();
        service.GetValue().Returns(42);
        var result = Result<int>.Try(() => service.GetValue());
        result.Should().BeSuccessfulWithValue(42);
        service.Received(1).GetValue();
    }

    [Fact]
    public void BeFailure_Should_Pass_When_SubstitutedServiceThrows()
    {
        var service = Substitute.For<IValueProvider>();
        service.GetValue().Returns(_ => throw new InvalidOperationException("broken"));
        var result = Result<int>.Try(() => service.GetValue());
        result.Should().BeFailure();
        service.Received(1).GetValue();
    }

    public interface IValueProvider
    {
        int GetValue();
    }
}

