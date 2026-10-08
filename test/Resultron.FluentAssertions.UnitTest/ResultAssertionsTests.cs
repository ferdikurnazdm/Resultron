namespace Resultron.FluentAssertions.UnitTest;

public sealed class ResultAssertionsTests
{
    private static readonly Error ExpectedError = new("VALIDATION", "Invalid value");

    [Fact]
    public void BeSuccessful_Should_Pass_When_ResultIsSuccessful()
    {
        var result = Result.Success();
        result.Should().BeSuccessful();
    }

    [Fact]
    public void BeSuccessful_Should_Throw_When_ResultIsFailure()
    {
        var result = Result.Failure(ExpectedError);
        Action act = () => result.Should().BeSuccessful();
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void BeFailure_Should_Pass_When_ResultIsFailure()
    {
        var result = Result.Failure(ExpectedError);
        result.Should().BeFailure();
    }

    [Fact]
    public void BeFailure_Should_Throw_When_ResultIsSuccessful()
    {
        var result = Result.Success();
        Action act = () => result.Should().BeFailure();
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void BeFailureWithError_Should_Pass_When_ErrorMatches()
    {
        var result = Result.Failure(ExpectedError);
        result.Should().BeFailureWithError(ExpectedError);
    }

    [Fact]
    public void BeFailureWithError_Should_Throw_When_ErrorDiffers()
    {
        var result = Result.Failure(ExpectedError);
        Action act = () => result.Should().BeFailureWithError(new Error("OTHER", "Other value"));
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void HaveError_Should_Pass_When_FailureContainsError()
    {
        Result.Failure(ExpectedError).Should().HaveError();
    }

    [Fact]
    public void HaveError_Should_Throw_When_ResultIsSuccessful()
    {
        Action act = () => Result.Success().Should().HaveError();
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void HaveErrorCode_Should_Pass_When_CodeMatches()
    {
        Result.Failure(ExpectedError).Should().HaveErrorCode("VALIDATION");
    }

    [Fact]
    public void HaveErrorCode_Should_Throw_When_CodeDiffers()
    {
        Action act = () => Result.Failure(ExpectedError).Should().HaveErrorCode("MISSING");
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void NotHaveError_Should_Pass_When_ResultIsSuccessful()
    {
        Result.Success().Should().NotHaveError();
    }

    [Fact]
    public void NotHaveError_Should_Throw_When_ResultIsFailure()
    {
        Action act = () => Result.Failure(ExpectedError).Should().NotHaveError();
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void HaveMetadata_Should_Pass_When_KeyAndValueMatch()
    {
        var error = ExpectedError.WithMetadata("requestId", "req-123");
        Result.Failure(error).Should().HaveMetadata("requestId", "req-123");
    }

    [Fact]
    public void HaveMetadata_Should_Throw_When_KeyIsMissing()
    {
        Action act = () => Result.Failure(ExpectedError).Should().HaveMetadata("missing", "x");
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void HaveException_Should_Pass_When_ExceptionTypeMatches()
    {
        var error = ExpectedError.CausedBy(new InvalidOperationException("failure"));
        Result.Failure(error).Should().HaveException<InvalidOperationException>();
    }

    [Fact]
    public void HaveCausedBy_Should_Pass_When_CauseTypeMatches()
    {
        var error = ExpectedError.CausedBy(new InvalidOperationException("failure"));
        Result.Failure(error).Should().HaveCausedBy<InvalidOperationException>();
    }

    [Fact]
    public void HaveCausedBy_Should_Throw_When_ExceptionTypeDiffers()
    {
        var error = ExpectedError.CausedBy(new InvalidOperationException("failure"));
        Action act = () => Result.Failure(error).Should().HaveCausedBy<ArgumentException>();
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void BeSuccessful_Should_Pass_When_DependencyCompletesWithoutError()
    {
        var dependency = Substitute.For<IWorkDependency>();
        var result = Result.Try(() => dependency.Run());
        result.Should().BeSuccessful();
        dependency.Received(1).Run();
    }

    [Fact]
    public void BeFailure_Should_Pass_When_DependencyThrows()
    {
        var dependency = Substitute.For<IWorkDependency>();
        dependency.When(x => x.Run()).Do(_ => throw new InvalidOperationException("Unavailable"));
        var result = Result.Try(() => dependency.Run());
        result.Should().BeFailure();
        dependency.Received(1).Run();
    }

    public interface IWorkDependency
    {
        void Run();
    }
}

