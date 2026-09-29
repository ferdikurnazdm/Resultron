using FluentAssertions;
using Xunit;

namespace Resultron.UnitTest;

public sealed class ResultOfTTests
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
        Result<int> result =
            Result<int>.Success(42);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Success_Should_Contain_Value()
    {
        // Act
        Result<int> result =
            Result<int>.Success(42);

        // Assert
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Success_Should_Have_Error_None()
    {
        // Act
        Result<int> result =
            Result<int>.Success(42);

        // Assert
        result.Error.Should()
            .BeSameAs(Error.None);
    }

    [Fact]
    public void Success_Should_Support_Default_Value_Type_Value()
    {
        // Act
        Result<int> result =
            Result<int>.Success(0);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(0);
    }

    [Fact]
    public void Success_Should_Support_Null_Reference_Value()
    {
        // Arrange
        string? value = null;

        // Act
        Result<string?> result =
            Result<string?>.Success(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
    }

    #endregion

    #region Failure

    [Fact]
    public void Failure_Should_Create_Failed_Result()
    {
        // Act
        Result<int> result =
            Result<int>.Failure(TestError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Failure_Should_Preserve_Error()
    {
        // Act
        Result<int> result =
            Result<int>.Failure(TestError);

        // Assert
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void Failure_Should_Preserve_Error_Code()
    {
        // Act
        Result<int> result =
            Result<int>.Failure(TestError);

        // Assert
        result.Error.Code.Should().Be("test.error");
    }

    [Fact]
    public void Failure_Should_Preserve_Error_Description()
    {
        // Act
        Result<int> result =
            Result<int>.Failure(TestError);

        // Assert
        result.Error.Description.Should().Be("Test error.");
    }

    [Fact]
    public void Failure_With_Error_None_Should_Throw_ArgumentException()
    {
        // Act
        Action action = () =>
            Result<int>.Failure(Error.None);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("error");
    }

    #endregion

    #region Value Access

    [Fact]
    public void Value_Should_Return_Value_When_Result_Is_Success()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        // Act
        int value = result.Value;

        // Assert
        value.Should().Be(42);
    }


    [Fact]
    public void Value_Should_Return_Default_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> result =
            Result<int>.Failure(TestError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
        result.Value.Should().Be(default);
    }

    #endregion

    #region Implicit Value Conversion

    [Fact]
    public void Implicit_Value_Conversion_Should_Create_Successful_Result()
    {
        // Act
        Result<int> result = 42;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Implicit_Value_Conversion_Should_Preserve_Value()
    {
        // Act
        Result<int> result = 42;

        // Assert
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Implicit_Reference_Value_Conversion_Should_Preserve_Value()
    {
        // Act
        Result<string> result =
            "Resultron";

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Resultron");
    }

    #endregion

    #region Implicit Error Conversion

    [Fact]
    public void Implicit_Error_Conversion_Should_Create_Failed_Result()
    {
        // Act
        Result<int> result = TestError;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Implicit_Error_Conversion_Should_Preserve_Error()
    {
        // Act
        Result<int> result = TestError;

        // Assert
        result.Error.Should().Be(TestError);
    }


    [Fact]
    public void Implicit_Error_Conversion_Should_Return_Default_Value()
    {
        // Arrange
        Result<int> result = TestError;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
        result.Value.Should().Be(default);
    }

    #endregion

    #region BaseResult Contract

    [Fact]
    public void Result_Generic_Should_Inherit_From_BaseResult()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        // Act
        BaseResult baseResult = result;

        // Assert
        baseResult.Should().NotBeNull();
        baseResult.IsSuccess.Should().BeTrue();
        baseResult.IsFailure.Should().BeFalse();
        baseResult.Error.Should().BeSameAs(Error.None);
    }

    [Fact]
    public void Result_Generic_Should_Implement_IBaseResult()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

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
        Result<int> result =
            Result<int>.Failure(TestError);

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
        Result<int> result =
            Result<int>.Failure(TestError);

        // Act
        IBaseResult baseResult = result;

        // Assert
        baseResult.IsFailure.Should().BeTrue();
        baseResult.Error.Should().Be(TestError);
    }

    #endregion
}