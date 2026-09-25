using FluentAssertions;
using Xunit;

namespace Resultron.UnitTest;

public sealed class BaseResultTests
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

    #region Status

    [Fact]
    public void Constructor_Should_Set_IsSuccess_To_True()
    {
        // Act
        var result =
            new TestResult(true);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_Set_IsFailure_To_True_When_Not_Successful()
    {
        // Act
        var result =
            new TestResult(false);

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
        // Act
        var result =
            new TestResult(isSuccess);

        // Assert
        result.IsFailure.Should()
            .Be(expectedIsFailure);

        result.IsFailure.Should()
            .Be(!result.IsSuccess);
    }

    #endregion

    #region Reasons

    [Fact]
    public void Reasons_Should_Be_Empty_When_No_Reasons_Are_Provided()
    {
        // Act
        var result =
            new TestResult(true);

        // Assert
        result.Reasons.Should().BeEmpty();
    }

    [Fact]
    public void Reasons_Should_Be_Empty_When_Reasons_Are_Null()
    {
        // Act
        var result =
            new TestResult(
                true,
                null);

        // Assert
        result.Reasons.Should().BeEmpty();
    }

    [Fact]
    public void Reasons_Should_Contain_All_Provided_Reasons()
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
        var result =
            new TestResult(
                false,
                reasons);

        // Assert
        result.Reasons.Should().Equal(
            FirstSuccess,
            FirstError,
            SecondSuccess,
            SecondError);
    }

    [Fact]
    public void Reasons_Should_Preserve_Insertion_Order()
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
        var result =
            new TestResult(
                false,
                reasons);

        // Assert
        result.Reasons.Should().Equal(
            FirstError,
            FirstSuccess,
            SecondError,
            SecondSuccess);
    }

    [Fact]
    public void Constructor_Should_Copy_Reasons_From_Source_Enumerable()
    {
        // Arrange
        var reasons = new List<IReason>
        {
            FirstError
        };

        var result =
            new TestResult(
                false,
                reasons);

        // Act
        reasons.Add(SecondError);

        // Assert
        result.Reasons.Should()
            .ContainSingle();

        result.Reasons.Should()
            .Contain(FirstError);

        result.Reasons.Should()
            .NotContain(SecondError);
    }

    #endregion

    #region Errors

    [Fact]
    public void Errors_Should_Be_Empty_When_No_Errors_Exist()
    {
        // Arrange
        var result = new TestResult(
            true,
            [
                FirstSuccess,
                SecondSuccess
            ]);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Errors_Should_Contain_Only_Error_Reasons()
    {
        // Arrange
        var result = new TestResult(
            false,
            [
                FirstSuccess,
                FirstError,
                SecondSuccess,
                SecondError
            ]);

        // Assert
        result.Errors.Should().Equal(
            FirstError,
            SecondError);
    }

    [Fact]
    public void Errors_Should_Preserve_Error_Order()
    {
        // Arrange
        var result = new TestResult(
            false,
            [
                SecondError,
                FirstSuccess,
                FirstError
            ]);

        // Assert
        result.Errors.Should().Equal(
            SecondError,
            FirstError);
    }

    #endregion

    #region Successes

    [Fact]
    public void Successes_Should_Be_Empty_When_No_Success_Reasons_Exist()
    {
        // Arrange
        var result = new TestResult(
            false,
            [
                FirstError,
                SecondError
            ]);

        // Assert
        result.Successes.Should().BeEmpty();
    }

    [Fact]
    public void Successes_Should_Contain_Only_Success_Reasons()
    {
        // Arrange
        var result = new TestResult(
            false,
            [
                FirstSuccess,
                FirstError,
                SecondSuccess,
                SecondError
            ]);

        // Assert
        result.Successes.Should().Equal(
            FirstSuccess,
            SecondSuccess);
    }

    [Fact]
    public void Successes_Should_Preserve_Success_Order()
    {
        // Arrange
        var result = new TestResult(
            true,
            [
                SecondSuccess,
                FirstError,
                FirstSuccess
            ]);

        // Assert
        result.Successes.Should().Equal(
            SecondSuccess,
            FirstSuccess);
    }

    #endregion

    #region Primary Error

    [Fact]
    public void Error_Should_Return_First_Error()
    {
        // Arrange
        var result = new TestResult(
            false,
            [
                FirstSuccess,
                FirstError,
                SecondError
            ]);

        // Act
        Error error = result.Error;

        // Assert
        error.Should().Be(FirstError);
    }

    [Fact]
    public void Error_Should_Return_Error_None_When_No_Error_Exists()
    {
        // Arrange
        var result = new TestResult(
            true,
            [
                FirstSuccess
            ]);

        // Act
        Error error = result.Error;

        // Assert
        error.Should().BeSameAs(Error.None);
    }

    [Fact]
    public void Error_Should_Return_Error_None_When_Reasons_Are_Empty()
    {
        // Arrange
        var result =
            new TestResult(false);

        // Act
        Error error = result.Error;

        // Assert
        error.Should().BeSameAs(Error.None);
    }

    #endregion

    #region Mixed Reasons

    [Fact]
    public void Mixed_Reasons_Should_Be_Exposed_Through_Correct_Collections()
    {
        // Arrange
        var result = new TestResult(
            false,
            [
                FirstSuccess,
                FirstError,
                SecondSuccess,
                SecondError
            ]);

        // Assert
        result.Reasons.Should().HaveCount(4);

        result.Errors.Should().Equal(
            FirstError,
            SecondError);

        result.Successes.Should().Equal(
            FirstSuccess,
            SecondSuccess);

        result.Error.Should().Be(FirstError);
    }

    [Fact]
    public void Status_Should_Not_Be_Inferred_From_Reasons()
    {
        // Arrange
        var result = new TestResult(
            true,
            [
                FirstError
            ]);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();

        result.Errors.Should()
            .ContainSingle()
            .Which.Should()
            .Be(FirstError);
    }

    [Fact]
    public void Failure_Status_Should_Not_Require_Error_Reason()
    {
        // Arrange
        var result = new TestResult(
            false,
            [
                FirstSuccess
            ]);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Errors.Should().BeEmpty();
        result.Successes.Should().ContainSingle();

        result.Error.Should()
            .BeSameAs(Error.None);
    }

    #endregion

    #region Read Only Collections

    [Fact]
    public void Reasons_Should_Be_Exposed_As_ReadOnly_List()
    {
        // Arrange
        var result = new TestResult(
            false,
            [
                FirstError
            ]);

        // Act
        IReadOnlyList<IReason> reasons =
            result.Reasons;

        // Assert
        reasons.Should()
            .BeAssignableTo<IReadOnlyList<IReason>>();
    }

    [Fact]
    public void Reasons_Should_Not_Allow_External_Modification()
    {
        // Arrange
        var result = new TestResult(
            false,
            [
                FirstError
            ]);

        // Act
        IList<IReason> reasons =
            (IList<IReason>)result.Reasons;

        Action act = () =>
            reasons.Add(SecondError);

        // Assert
        act.Should()
            .Throw<NotSupportedException>();

        result.Reasons.Should()
            .ContainSingle();
    }

    [Fact]
    public void Errors_Should_Not_Allow_External_Modification()
    {
        // Arrange
        var result = new TestResult(
            false,
            [
                FirstError
            ]);

        // Act
        IList<Error> errors =
            (IList<Error>)result.Errors;

        Action act = () =>
            errors.Add(SecondError);

        // Assert
        act.Should()
            .Throw<NotSupportedException>();

        result.Errors.Should()
            .ContainSingle();
    }

    [Fact]
    public void Successes_Should_Not_Allow_External_Modification()
    {
        // Arrange
        var result = new TestResult(
            true,
            [
                FirstSuccess
            ]);

        // Act
        IList<Success> successes =
            (IList<Success>)result.Successes;

        Action act = () =>
            successes.Add(SecondSuccess);

        // Assert
        act.Should()
            .Throw<NotSupportedException>();

        result.Successes.Should()
            .ContainSingle();
    }

    #endregion

    #region IBaseResult

    [Fact]
    public void BaseResult_Should_Implement_IBaseResult()
    {
        // Arrange
        BaseResult result =
            new TestResult(true);

        // Act
        IBaseResult baseResult = result;

        // Assert
        baseResult.Should().NotBeNull();
        baseResult.IsSuccess.Should().BeTrue();
        baseResult.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void IBaseResult_Should_Expose_Reasons()
    {
        // Arrange
        IBaseResult result = new TestResult(
            false,
            [
                FirstError,
                FirstSuccess
            ]);

        // Assert
        result.Reasons.Should().Equal(
            FirstError,
            FirstSuccess);
    }

    [Fact]
    public void IBaseResult_Should_Expose_Filtered_Errors()
    {
        // Arrange
        IBaseResult result = new TestResult(
            false,
            [
                FirstSuccess,
                FirstError,
                SecondSuccess,
                SecondError
            ]);

        // Assert
        result.Errors.Should().Equal(
            FirstError,
            SecondError);
    }

    [Fact]
    public void IBaseResult_Should_Expose_Filtered_Successes()
    {
        // Arrange
        IBaseResult result = new TestResult(
            true,
            [
                FirstSuccess,
                FirstError,
                SecondSuccess
            ]);

        // Assert
        result.Successes.Should().Equal(
            FirstSuccess,
            SecondSuccess);
    }

    #endregion

    private sealed class TestResult : BaseResult
    {
        public TestResult(
            bool isSuccess,
            IEnumerable<IReason>? reasons = null)
            : base(
                isSuccess,
                reasons)
        {
        }
    }
}