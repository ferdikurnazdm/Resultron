using FluentAssertions;
using Xunit;

namespace Resultron.UnitTest;

public sealed class ResultTests
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
        Result result =
            Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Success_Should_Create_Result_Without_Reasons()
    {
        // Act
        Result result =
            Result.Success();

        // Assert
        result.Reasons.Should().BeEmpty();
        result.Errors.Should().BeEmpty();
        result.Successes.Should().BeEmpty();
    }

    [Fact]
    public void Success_Should_Return_Error_None_When_No_Error_Exists()
    {
        // Act
        Result result =
            Result.Success();

        // Assert
        result.Error.Should()
            .BeSameAs(Error.None);
    }

    #endregion

    #region Success - Success Reason

    [Fact]
    public void Success_With_Success_Reason_Should_Create_Successful_Result()
    {
        // Act
        Result result =
            Result.Success(FirstSuccess);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Success_With_Success_Reason_Should_Add_Reason()
    {
        // Act
        Result result =
            Result.Success(FirstSuccess);

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
        Result result =
            Result.Success(FirstSuccess);

        // Assert
        result.Successes.Should()
            .ContainSingle()
            .Which.Should()
            .Be(FirstSuccess);

        result.Errors.Should().BeEmpty();
    }

    #endregion

    #region Success - Reasons

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
        Result result =
            Result.Success(reasons);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Reasons.Should().Equal(
            FirstSuccess,
            FirstError,
            SecondSuccess);
    }

    [Fact]
    public void Success_With_Reasons_Should_Filter_Errors_And_Successes()
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
        Result result =
            Result.Success(reasons);

        // Assert
        result.Errors.Should().Equal(
            FirstError,
            SecondError);

        result.Successes.Should().Equal(
            FirstSuccess,
            SecondSuccess);
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
        Result result =
            Result.Success(reasons);

        // Assert
        result.Error.Should().Be(FirstError);
    }

    [Fact]
    public void Success_With_Empty_Reasons_Should_Create_Successful_Result()
    {
        // Arrange
        IReason[] reasons = [];

        // Act
        Result result =
            Result.Success(reasons);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Reasons.Should().BeEmpty();
    }

    #endregion

    #region Failure - Error

    [Fact]
    public void Failure_With_Error_Should_Create_Failed_Result()
    {
        // Act
        Result result =
            Result.Failure(FirstError);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Failure_With_Error_Should_Add_Error_As_Reason()
    {
        // Act
        Result result =
            Result.Failure(FirstError);

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
        Result result =
            Result.Failure(FirstError);

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
        Result result =
            Result.Failure(FirstError);

        // Assert
        result.Error.Should().Be(FirstError);
    }

    #endregion

    #region Failure - Errors

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
        Result result =
            Result.Failure(errors);

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
        Result result =
            Result.Failure(errors);

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
        Result result =
            Result.Failure(errors);

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
        Result result =
            Result.Failure(errors);

        // Assert
        result.Error.Should().Be(FirstError);
    }

    [Fact]
    public void Failure_With_Empty_Error_Collection_Should_Create_Failed_Result()
    {
        // Arrange
        Error[] errors = [];

        // Act
        Result result =
            Result.Failure(errors);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().BeEmpty();

        result.Error.Should()
            .BeSameAs(Error.None);
    }

    #endregion

    #region Failure - Reasons

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
        Result result =
            Result.Failure(reasons);

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
        Result result =
            Result.Failure(reasons);

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
        Result result =
            Result.Failure(reasons);

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
        Result result =
            Result.Failure(reasons);

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
        Result result =
            Result.Failure(reasons);

        // Assert
        result.Error.Should().Be(FirstError);
    }

    [Fact]
    public void Failure_With_Only_Success_Reasons_Should_Return_Error_None()
    {
        // Arrange
        IReason[] reasons =
        [
            FirstSuccess,
            SecondSuccess
        ];

        // Act
        Result result =
            Result.Failure(reasons);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Errors.Should().BeEmpty();

        result.Successes.Should().Equal(
            FirstSuccess,
            SecondSuccess);

        result.Error.Should()
            .BeSameAs(Error.None);
    }

    #endregion

    #region Implicit Error Conversion

    [Fact]
    public void Implicit_Error_Conversion_Should_Create_Failed_Result()
    {
        // Act
        Result result = FirstError;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Implicit_Error_Conversion_Should_Preserve_Error()
    {
        // Act
        Result result = FirstError;

        // Assert
        result.Error.Should().Be(FirstError);

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .Be(FirstError);
    }

    [Fact]
    public void Implicit_Error_Conversion_Should_Add_Error_To_Reasons()
    {
        // Act
        Result result = FirstError;

        // Assert
        result.Reasons.Should()
            .ContainSingle()
            .Which.Should()
            .Be(FirstError);
    }

    #endregion

    #region BaseResult Contract

    [Fact]
    public void Result_Should_Inherit_From_BaseResult()
    {
        // Arrange
        Result result =
            Result.Success();

        // Act
        BaseResult baseResult = result;

        // Assert
        baseResult.Should().NotBeNull();
        baseResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Result_Should_Implement_IBaseResult()
    {
        // Arrange
        Result result =
            Result.Success();

        // Act
        IBaseResult baseResult = result;

        // Assert
        baseResult.Should().NotBeNull();
        baseResult.IsSuccess.Should().BeTrue();
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

        Result result =
            Result.Success(reasons);

        // Act
        reasons.Add(SecondSuccess);

        // Assert
        result.Reasons.Should()
            .ContainSingle()
            .Which.Should()
            .Be(FirstSuccess);
    }

    [Fact]
    public void Failure_Should_Copy_Errors_From_Source_Collection()
    {
        // Arrange
        var errors = new List<Error>
        {
            FirstError
        };

        Result result =
            Result.Failure(errors);

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

        Result result =
            Result.Failure(reasons);

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