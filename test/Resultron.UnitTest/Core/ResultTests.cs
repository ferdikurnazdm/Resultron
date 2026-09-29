using FluentAssertions;
using Xunit;

namespace Resultron.UnitTest;

public sealed class ResultTests
{
    private static readonly Error TestError =
        new(
            "test.error",
            "Test error.");

    #region Success

    [Fact]
    public void Success_Should_Create_Successful_Result()
    {
        // Act
        Result result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Success_Should_Have_Error_None()
    {
        // Act
        Result result = Result.Success();

        // Assert
        result.Error.Should().BeSameAs(Error.None);
    }

    #endregion

    #region Failure

    [Fact]
    public void Failure_Should_Create_Failed_Result()
    {
        // Act
        Result result = Result.Failure(TestError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Failure_Should_Preserve_Error()
    {
        // Act
        Result result = Result.Failure(TestError);

        // Assert
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void Failure_Should_Preserve_Error_Code()
    {
        // Act
        Result result = Result.Failure(TestError);

        // Assert
        result.Error.Code.Should().Be("test.error");
    }

    [Fact]
    public void Failure_Should_Preserve_Error_Description()
    {
        // Act
        Result result = Result.Failure(TestError);

        // Assert
        result.Error.Description.Should().Be("Test error.");
    }

    [Fact]
    public void Failure_With_Error_None_Should_Throw_ArgumentException()
    {
        // Act
        Action action = () =>
            Result.Failure(Error.None);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("error");
    }

    #endregion

    #region Implicit Error Conversion

    [Fact]
    public void Implicit_Error_Conversion_Should_Create_Failed_Result()
    {
        // Act
        Result result = TestError;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Implicit_Error_Conversion_Should_Preserve_Error()
    {
        // Act
        Result result = TestError;

        // Assert
        result.Error.Should().Be(TestError);
    }

    #endregion

    #region BaseResult Contract

    [Fact]
    public void Result_Should_Inherit_From_BaseResult()
    {
        // Arrange
        Result result = Result.Success();

        // Act
        BaseResult baseResult = result;

        // Assert
        baseResult.Should().NotBeNull();
        baseResult.IsSuccess.Should().BeTrue();
        baseResult.IsFailure.Should().BeFalse();
        baseResult.Error.Should().BeSameAs(Error.None);
    }

    [Fact]
    public void Result_Should_Implement_IBaseResult()
    {
        // Arrange
        Result result = Result.Success();

        // Act
        IBaseResult baseResult = result;

        // Assert
        baseResult.Should().NotBeNull();
        baseResult.IsSuccess.Should().BeTrue();
        baseResult.IsFailure.Should().BeFalse();
        baseResult.Error.Should().BeSameAs(Error.None);
    }

    [Fact]
    public void Failed_Result_Should_Expose_Error_Through_BaseResult()
    {
        // Arrange
        Result result = Result.Failure(TestError);

        // Act
        BaseResult baseResult = result;

        // Assert
        baseResult.IsFailure.Should().BeTrue();
        baseResult.Error.Should().Be(TestError);
    }

    [Fact]
    public void Failed_Result_Should_Expose_Error_Through_IBaseResult()
    {
        // Arrange
        Result result = Result.Failure(TestError);

        // Act
        IBaseResult baseResult = result;

        // Assert
        baseResult.IsFailure.Should().BeTrue();
        baseResult.Error.Should().Be(TestError);
    }

    #endregion
}