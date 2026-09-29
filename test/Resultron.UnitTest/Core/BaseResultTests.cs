using FluentAssertions;
using Xunit;

namespace Resultron.UnitTest;

public sealed class BaseResultTests
{
    private static readonly Error TestError =
        new(
            "test.error",
            "Test error.");

    #region Status

    [Fact]
    public void Constructor_Should_Set_IsSuccess_To_True()
    {
        // Act
        var result =
            new TestResult(
                isSuccess: true,
                error: Error.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_Set_IsFailure_To_True_When_Not_Successful()
    {
        // Act
        var result =
            new TestResult(
                isSuccess: false,
                error: TestError);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void IsFailure_Should_Always_Be_Inverse_Of_IsSuccess(
        bool isSuccess,
        bool expectedIsFailure)
    {
        // Arrange
        Error error =
            isSuccess
                ? Error.None
                : TestError;

        // Act
        var result =
            new TestResult(
                isSuccess,
                error);

        // Assert
        result.IsFailure.Should()
            .Be(expectedIsFailure);

        result.IsFailure.Should()
            .Be(!result.IsSuccess);
    }

    #endregion

    #region Error

    [Fact]
    public void Successful_Result_Should_Have_Error_None()
    {
        // Act
        var result =
            new TestResult(
                isSuccess: true,
                error: Error.None);

        // Assert
        result.Error.Should().BeSameAs(Error.None);
    }

    [Fact]
    public void Failed_Result_Should_Preserve_Error()
    {
        // Act
        var result =
            new TestResult(
                isSuccess: false,
                error: TestError);

        // Assert
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void Failed_Result_Should_Preserve_Error_Code()
    {
        // Act
        var result =
            new TestResult(
                isSuccess: false,
                error: TestError);

        // Assert
        result.Error.Code.Should().Be("test.error");
    }

    [Fact]
    public void Failed_Result_Should_Preserve_Error_Description()
    {
        // Act
        var result =
            new TestResult(
                isSuccess: false,
                error: TestError);

        // Assert
        result.Error.Description.Should().Be("Test error.");
    }

    #endregion

    #region Invalid State

    [Fact]
    public void Constructor_Should_Throw_When_Successful_Result_Has_Error()
    {
        // Act
        Action action = () =>
            new TestResult(
                isSuccess: true,
                error: TestError);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("error");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Failed_Result_Has_Error_None()
    {
        // Act
        Action action = () =>
            new TestResult(
                isSuccess: false,
                error: Error.None);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("error");
    }

    #endregion

    #region IBaseResult

    [Fact]
    public void BaseResult_Should_Implement_IBaseResult()
    {
        // Arrange
        BaseResult result =
            new TestResult(
                isSuccess: true,
                error: Error.None);

        // Act
        IBaseResult baseResult = result;

        // Assert
        baseResult.Should().NotBeNull();
        baseResult.IsSuccess.Should().BeTrue();
        baseResult.IsFailure.Should().BeFalse();
        baseResult.Error.Should().BeSameAs(Error.None);
    }

    [Fact]
    public void IBaseResult_Should_Expose_Error()
    {
        // Arrange
        IBaseResult result =
            new TestResult(
                isSuccess: false,
                error: TestError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    #endregion

    private sealed class TestResult : BaseResult
    {
        public TestResult(
            bool isSuccess,
            Error error)
            : base(
                isSuccess,
                error)
        {
        }
    }
}