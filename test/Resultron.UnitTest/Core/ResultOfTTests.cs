using FluentAssertions;
using Xunit;

namespace Resultron.UnitTest;

public sealed class ResultOfTTests
{
    private static readonly Error FirstError =
        new(
            "first.error",
            "First error.");

    private static readonly Error SecondError =
        new(
            "second.error",
            "Second error.");

    private static readonly Success FirstSuccess =
        new("First success.");

    private static readonly Success SecondSuccess =
        new("Second success.");

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
    public void Success_Should_Have_No_Reasons_When_None_Are_Provided()
    {
        // Act
        Result<int> result =
            Result<int>.Success(42);

        // Assert
        result.Reasons.Should().BeEmpty();
        result.Errors.Should().BeEmpty();
        result.Successes.Should().BeEmpty();
    }

    [Fact]
    public void Success_Should_Return_Error_None_When_No_Error_Exists()
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

    #region Success With Success Reason

    [Fact]
    public void Success_With_Success_Reason_Should_Create_Successful_Result()
    {
        // Act
        Result<int> result =
            Result<int>.Success(
                42,
                FirstSuccess);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Success_With_Success_Reason_Should_Add_Reason()
    {
        // Act
        Result<int> result =
            Result<int>.Success(
                42,
                FirstSuccess);

        // Assert
        result.Reasons.Should()
            .ContainSingle()
            .Which.Should()
            .Be(FirstSuccess);
    }

    [Fact]
    public void Success_With_Success_Reason_Should_Add_Success()
    {
        // Act
        Result<int> result =
            Result<int>.Success(
                42,
                FirstSuccess);

        // Assert
        result.Successes.Should()
            .ContainSingle()
            .Which.Should()
            .Be(FirstSuccess);

        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Success_With_Success_Reason_Should_Preserve_Value()
    {
        // Act
        Result<int> result =
            Result<int>.Success(
                42,
                FirstSuccess);

        // Assert
        result.Value.Should().Be(42);
    }

    #endregion

    #region Success With Reasons

    [Fact]
    public void Success_With_Reasons_Should_Preserve_All_Reasons()
    {
        // Arrange
        IReason[] reasons =
        [
            FirstSuccess,
            FirstError,
            SecondSuccess
        ];

        // Act
        Result<int> result =
            Result<int>.Success(
                42,
                reasons);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);

        result.Reasons.Should().Equal(
            FirstSuccess,
            FirstError,
            SecondSuccess);
    }

    [Fact]
    public void Success_With_Reasons_Should_Filter_Errors()
    {
        // Arrange
        IReason[] reasons =
        [
            FirstSuccess,
            FirstError,
            SecondSuccess,
            SecondError
        ];

        // Act
        Result<int> result =
            Result<int>.Success(
                42,
                reasons);

        // Assert
        result.Errors.Should().Equal(
            FirstError,
            SecondError);
    }

    [Fact]
    public void Success_With_Reasons_Should_Filter_Successes()
    {
        // Arrange
        IReason[] reasons =
        [
            FirstSuccess,
            FirstError,
            SecondSuccess,
            SecondError
        ];

        // Act
        Result<int> result =
            Result<int>.Success(
                42,
                reasons);

        // Assert
        result.Successes.Should().Equal(
            FirstSuccess,
            SecondSuccess);
    }

    [Fact]
    public void Success_With_Reasons_Should_Preserve_Reason_Order()
    {
        // Arrange
        IReason[] reasons =
        [
            SecondSuccess,
            FirstError,
            FirstSuccess,
            SecondError
        ];

        // Act
        Result<int> result =
            Result<int>.Success(
                42,
                reasons);

        // Assert
        result.Reasons.Should().Equal(
            SecondSuccess,
            FirstError,
            FirstSuccess,
            SecondError);
    }

    [Fact]
    public void Success_With_Reasons_Should_Use_First_Error_As_Primary_Error()
    {
        // Arrange
        IReason[] reasons =
        [
            FirstSuccess,
            FirstError,
            SecondError
        ];

        // Act
        Result<int> result =
            Result<int>.Success(
                42,
                reasons);

        // Assert
        result.Error.Should().Be(FirstError);
    }

    #endregion

    #region Failure With Error

    [Fact]
    public void Failure_With_Error_Should_Create_Failed_Result()
    {
        // Act
        Result<int> result =
            Result<int>.Failure(FirstError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Failure_With_Error_Should_Add_Error_To_Reasons()
    {
        // Act
        Result<int> result =
            Result<int>.Failure(FirstError);

        // Assert
        result.Reasons.Should()
            .ContainSingle()
            .Which.Should()
            .Be(FirstError);
    }

    [Fact]
    public void Failure_With_Error_Should_Add_Error_To_Errors()
    {
        // Act
        Result<int> result =
            Result<int>.Failure(FirstError);

        // Assert
        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .Be(FirstError);

        result.Successes.Should().BeEmpty();
    }

    [Fact]
    public void Failure_With_Error_Should_Set_Primary_Error()
    {
        // Act
        Result<int> result =
            Result<int>.Failure(FirstError);

        // Assert
        result.Error.Should().Be(FirstError);
    }

    #endregion

    #region Failure With Errors

    [Fact]
    public void Failure_With_Errors_Should_Create_Failed_Result()
    {
        // Arrange
        Error[] errors =
        [
            FirstError,
            SecondError
        ];

        // Act
        Result<int> result =
            Result<int>.Failure(errors);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Failure_With_Errors_Should_Preserve_All_Errors()
    {
        // Arrange
        Error[] errors =
        [
            FirstError,
            SecondError
        ];

        // Act
        Result<int> result =
            Result<int>.Failure(errors);

        // Assert
        result.Errors.Should().Equal(
            FirstError,
            SecondError);

        result.Reasons.Should().Equal(
            FirstError,
            SecondError);
    }

    [Fact]
    public void Failure_With_Errors_Should_Preserve_Error_Order()
    {
        // Arrange
        Error[] errors =
        [
            SecondError,
            FirstError
        ];

        // Act
        Result<int> result =
            Result<int>.Failure(errors);

        // Assert
        result.Errors.Should().Equal(
            SecondError,
            FirstError);
    }

    [Fact]
    public void Failure_With_Errors_Should_Use_First_Error_As_Primary_Error()
    {
        // Arrange
        Error[] errors =
        [
            FirstError,
            SecondError
        ];

        // Act
        Result<int> result =
            Result<int>.Failure(errors);

        // Assert
        result.Error.Should().Be(FirstError);
    }

    [Fact]
    public void Failure_With_Empty_Errors_Should_Still_Be_Failure()
    {
        // Arrange
        Error[] errors = [];

        // Act
        Result<int> result =
            Result<int>.Failure(errors);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().BeEmpty();

        result.Error.Should()
            .BeSameAs(Error.None);
    }

    #endregion

    #region Failure With Reasons

    [Fact]
    public void Failure_With_Reasons_Should_Create_Failed_Result()
    {
        // Arrange
        IReason[] reasons =
        [
            FirstError,
            FirstSuccess
        ];

        // Act
        Result<int> result =
            Result<int>.Failure(reasons);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Failure_With_Reasons_Should_Preserve_All_Reasons()
    {
        // Arrange
        IReason[] reasons =
        [
            FirstError,
            FirstSuccess,
            SecondError,
            SecondSuccess
        ];

        // Act
        Result<int> result =
            Result<int>.Failure(reasons);

        // Assert
        result.Reasons.Should().Equal(
            FirstError,
            FirstSuccess,
            SecondError,
            SecondSuccess);
    }

    [Fact]
    public void Failure_With_Reasons_Should_Filter_Errors()
    {
        // Arrange
        IReason[] reasons =
        [
            FirstSuccess,
            FirstError,
            SecondSuccess,
            SecondError
        ];

        // Act
        Result<int> result =
            Result<int>.Failure(reasons);

        // Assert
        result.Errors.Should().Equal(
            FirstError,
            SecondError);
    }

    [Fact]
    public void Failure_With_Reasons_Should_Filter_Successes()
    {
        // Arrange
        IReason[] reasons =
        [
            FirstSuccess,
            FirstError,
            SecondSuccess,
            SecondError
        ];

        // Act
        Result<int> result =
            Result<int>.Failure(reasons);

        // Assert
        result.Successes.Should().Equal(
            FirstSuccess,
            SecondSuccess);
    }

    [Fact]
    public void Failure_With_Reasons_Should_Use_First_Error_As_Primary_Error()
    {
        // Arrange
        IReason[] reasons =
        [
            FirstSuccess,
            FirstError,
            SecondError
        ];

        // Act
        Result<int> result =
            Result<int>.Failure(reasons);

        // Assert
        result.Error.Should().Be(FirstError);
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
    public void Value_Should_Throw_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> result =
            Result<int>.Failure(FirstError);

        // Act
        Action act = () =>
        {
            _ = result.Value;
        };

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "Cannot access value of a failed result.");
    }

    [Fact]
    public void Value_Should_Throw_When_Failure_Has_No_Error()
    {
        // Arrange
        Result<int> result =
            Result<int>.Failure(
                Array.Empty<Error>());

        // Act
        Action act = () =>
        {
            _ = result.Value;
        };

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "Cannot access value of a failed result.");
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
        Result<int> result = FirstError;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Implicit_Error_Conversion_Should_Preserve_Error()
    {
        // Act
        Result<int> result = FirstError;

        // Assert
        result.Error.Should().Be(FirstError);

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .Be(FirstError);
    }

    [Fact]
    public void Implicit_Error_Conversion_Should_Prevent_Value_Access()
    {
        // Arrange
        Result<int> result = FirstError;

        // Act
        Action act = () =>
        {
            _ = result.Value;
        };

        // Assert
        act.Should()
            .Throw<InvalidOperationException>();
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
        baseResult.IsSuccess.Should().BeTrue();
        baseResult.Reasons.Should().BeEmpty();
    }

    #endregion

    #region Source Collection Isolation

    [Fact]
    public void Success_Should_Copy_Reasons_From_Source_Collection()
    {
        // Arrange
        var reasons = new List<IReason>
        {
            FirstSuccess
        };

        Result<int> result =
            Result<int>.Success(
                42,
                reasons);

        // Act
        reasons.Add(SecondSuccess);

        // Assert
        result.Reasons.Should()
            .ContainSingle()
            .Which.Should()
            .Be(FirstSuccess);

        result.Value.Should().Be(42);
    }

    [Fact]
    public void Failure_Should_Copy_Errors_From_Source_Collection()
    {
        // Arrange
        var errors = new List<Error>
        {
            FirstError
        };

        Result<int> result =
            Result<int>.Failure(errors);

        // Act
        errors.Add(SecondError);

        // Assert
        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .Be(FirstError);
    }

    [Fact]
    public void Failure_Should_Copy_Reasons_From_Source_Collection()
    {
        // Arrange
        var reasons = new List<IReason>
        {
            FirstError
        };

        Result<int> result =
            Result<int>.Failure(reasons);

        // Act
        reasons.Add(FirstSuccess);

        // Assert
        result.Reasons.Should()
            .ContainSingle()
            .Which.Should()
            .Be(FirstError);
    }

    #endregion
}