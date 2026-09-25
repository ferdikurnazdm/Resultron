using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Resultron.UnitTest;

public sealed class LinqTests
{
    private static readonly Error TestError =
        new("test.error", "Test error.");

    #region Select - Result

    [Fact]
    public void Select_Should_Project_Successful_Result()
    {
        // Arrange
        Result source = Result.Success();

        // Act
        Result<int> result = source.Select(
            () => 42);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Select_Should_Not_Invoke_Selector_When_Result_Is_Failure()
    {
        // Arrange
        Result source =
            Result.Failure(TestError);

        Func<int> selector =
            Substitute.For<Func<int>>();

        // Act
        Result<int> result =
            source.Select(selector);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        selector.DidNotReceive().Invoke();
    }

    [Fact]
    public void Select_Should_Propagate_Reasons_When_Result_Is_Failure()
    {
        // Arrange
        var context =
            new Success("Context.");

        Result source = Result.Failure(
            new IReason[]
            {
                TestError,
                context
            });

        // Act
        Result<int> result =
            source.Select(() => 42);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should()
            .BeEquivalentTo(source.Reasons);
    }

    #endregion

    #region Select - Result<T>

    [Fact]
    public void Select_Generic_Should_Project_Value()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<string> result = source.Select(
            value => $"Value: {value}");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public void Select_Generic_Should_Pass_Value_To_Selector()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        Func<int, string> selector =
            Substitute.For<Func<int, string>>();

        selector(42)
            .Returns("mapped");

        // Act
        Result<string> result =
            source.Select(selector);

        // Assert
        result.Value.Should().Be("mapped");

        selector.Received(1)
            .Invoke(42);
    }

    [Fact]
    public void Select_Generic_Should_Not_Invoke_Selector_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<int, string> selector =
            Substitute.For<Func<int, string>>();

        // Act
        Result<string> result =
            source.Select(selector);

        // Assert
        result.IsFailure.Should().BeTrue();

        selector.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    #endregion

    #region SelectMany - Result

    [Fact]
    public void SelectMany_Should_Invoke_Binder_When_Result_Is_Success()
    {
        // Arrange
        Result source = Result.Success();

        Func<Result<int>> binder =
            Substitute.For<Func<Result<int>>>();

        binder()
            .Returns(Result<int>.Success(42));

        // Act
        Result<int> result =
            source.SelectMany(binder);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);

        binder.Received(1).Invoke();
    }

    [Fact]
    public void SelectMany_Should_Not_Invoke_Binder_When_Result_Is_Failure()
    {
        // Arrange
        Result source =
            Result.Failure(TestError);

        Func<Result<int>> binder =
            Substitute.For<Func<Result<int>>>();

        // Act
        Result<int> result =
            source.SelectMany(binder);

        // Assert
        result.IsFailure.Should().BeTrue();

        binder.DidNotReceive().Invoke();
    }

    [Fact]
    public void SelectMany_With_Projector_Should_Project_Intermediate_Value()
    {
        // Arrange
        Result source = Result.Success();

        // Act
        Result<string> result = source.SelectMany(
            binder: () => Result<int>.Success(42),
            projector: (unit, value) =>
                $"Value: {value}");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public void SelectMany_With_Projector_Should_Propagate_Source_Reasons()
    {
        // Arrange
        var context =
            new Success("Context.");

        Result source = Result.Failure(
            new IReason[]
            {
                TestError,
                context
            });

        // Act
        Result<string> result = source.SelectMany(
            binder: () => Result<int>.Success(42),
            projector: (unit, value) =>
                value.ToString());

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should()
            .BeEquivalentTo(source.Reasons);
    }

    [Fact]
    public void SelectMany_With_Projector_Should_Propagate_Intermediate_Reasons()
    {
        // Arrange
        Result source = Result.Success();

        var context =
            new Success("Intermediate context.");

        Result<int> intermediate =
            Result<int>.Failure(
                new IReason[]
                {
                    TestError,
                    context
                });

        // Act
        Result<string> result = source.SelectMany(
            binder: () => intermediate,
            projector: (unit, value) =>
                value.ToString());

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should()
            .BeEquivalentTo(intermediate.Reasons);
    }

    #endregion

    #region SelectMany - Result<T>

    [Fact]
    public void SelectMany_Generic_Should_Bind_Value()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<string> result = source.SelectMany(
            value => Result<string>.Success(
                $"Value: {value}"));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public void SelectMany_Generic_Should_Pass_Value_To_Binder()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        Func<int, Result<string>> binder =
            Substitute.For<Func<int, Result<string>>>();

        binder(42)
            .Returns(Result<string>.Success("mapped"));

        // Act
        Result<string> result =
            source.SelectMany(binder);

        // Assert
        result.Value.Should().Be("mapped");

        binder.Received(1)
            .Invoke(42);
    }

    [Fact]
    public void SelectMany_Generic_With_Projector_Should_Combine_Values()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(10);

        // Act
        Result<int> result = source.SelectMany(
            binder: first =>
                Result<int>.Success(20),
            projector: (first, second) =>
                first + second);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(30);
    }

    [Fact]
    public void SelectMany_Generic_With_Projector_Should_Not_Invoke_Binder_When_Source_Fails()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<int, Result<int>> binder =
            Substitute.For<Func<int, Result<int>>>();

        // Act
        Result<int> result = source.SelectMany(
            binder,
            (first, second) => first + second);

        // Assert
        result.IsFailure.Should().BeTrue();

        binder.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    [Fact]
    public void SelectMany_Generic_With_Projector_Should_Not_Invoke_Projector_When_Intermediate_Fails()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(10);

        Func<int, int, int> projector =
            Substitute.For<Func<int, int, int>>();

        // Act
        Result<int> result = source.SelectMany(
            binder: _ =>
                Result<int>.Failure(TestError),
            projector: projector);

        // Assert
        result.IsFailure.Should().BeTrue();

        projector.DidNotReceive()
            .Invoke(
                Arg.Any<int>(),
                Arg.Any<int>());
    }

    [Fact]
    public void SelectMany_Generic_With_Projector_Should_Propagate_Intermediate_Reasons()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(10);

        var context =
            new Success("Intermediate context.");

        Result<int> intermediate =
            Result<int>.Failure(
                new IReason[]
                {
                    TestError,
                    context
                });

        // Act
        Result<int> result = source.SelectMany(
            _ => intermediate,
            (first, second) => first + second);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should()
            .BeEquivalentTo(intermediate.Reasons);
    }

    #endregion

    #region Where With Explicit Error

    [Fact]
    public void Where_Should_Return_Original_Result_When_Predicate_Passes()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> result = source.Where(
            value => value > 0,
            TestError);

        // Assert
        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Where_Should_Return_Provided_Error_When_Predicate_Fails()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> result = source.Where(
            value => value < 0,
            TestError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void Where_Should_Not_Invoke_Predicate_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<int, bool> predicate =
            Substitute.For<Func<int, bool>>();

        // Act
        Result<int> result = source.Where(
            predicate,
            new Error(
                "other.error",
                "Other error."));

        // Assert
        result.Should().BeSameAs(source);

        predicate.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    #endregion

    #region Where With Default Error

    [Fact]
    public void Where_Default_Should_Return_Original_Result_When_Predicate_Passes()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> result =
            source.Where(value => value > 0);

        // Assert
        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Where_Default_Should_Return_Default_Error_When_Predicate_Fails()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> result =
            source.Where(value => value < 0);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Code.Should()
            .Be("result.predicate_failed");

        result.Error.Description.Should()
            .Be(
                "The result did not satisfy the specified condition.");
    }

    #endregion

    #region Query Syntax

    [Fact]
    public void Query_Select_Should_Project_Result_Value()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<string> result =
            from value in source
            select $"Value: {value}";

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public void Query_Select_Should_Propagate_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        // Act
        Result<string> result =
            from value in source
            select $"Value: {value}";

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void Query_SelectMany_Should_Combine_Results()
    {
        // Arrange
        Result<int> first =
            Result<int>.Success(10);

        Result<int> second =
            Result<int>.Success(20);

        // Act
        Result<int> result =
            from firstValue in first
            from secondValue in second
            select firstValue + secondValue;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(30);
    }

    [Fact]
    public void Query_SelectMany_Should_Short_Circuit_When_First_Result_Fails()
    {
        // Arrange
        Result<int> first =
            Result<int>.Failure(TestError);

        Result<int> second =
            Result<int>.Success(20);

        // Act
        Result<int> result =
            from firstValue in first
            from secondValue in second
            select firstValue + secondValue;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void Query_SelectMany_Should_Propagate_Second_Result_Failure()
    {
        // Arrange
        Result<int> first =
            Result<int>.Success(10);

        Result<int> second =
            Result<int>.Failure(TestError);

        // Act
        Result<int> result =
            from firstValue in first
            from secondValue in second
            select firstValue + secondValue;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void Query_Where_Should_Return_Value_When_Predicate_Passes()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> result =
            from value in source
            where value > 0
            select value;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Query_Where_Should_Return_Default_Error_When_Predicate_Fails()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> result =
            from value in source
            where value < 0
            select value;

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Code.Should()
            .Be("result.predicate_failed");

        result.Error.Description.Should()
            .Be(
                "The result did not satisfy the specified condition.");
    }

    [Fact]
    public void Query_Multiple_From_And_Where_Should_Work_Together()
    {
        // Arrange
        Result<int> first =
            Result<int>.Success(10);

        Result<int> second =
            Result<int>.Success(20);

        // Act
        Result<int> result =
            from firstValue in first
            from secondValue in second
            where firstValue > 0 && secondValue > 0
            select firstValue + secondValue;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(30);
    }

    [Fact]
    public void Query_Multiple_From_And_Where_Should_Return_Failure_When_Predicate_Fails()
    {
        // Arrange
        Result<int> first =
            Result<int>.Success(10);

        Result<int> second =
            Result<int>.Success(20);

        // Act
        Result<int> result =
            from firstValue in first
            from secondValue in second
            where firstValue < 0 && secondValue > 0
            select firstValue + secondValue;

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Code.Should()
            .Be("result.predicate_failed");

        result.Error.Description.Should()
            .Be(
                "The result did not satisfy the specified condition.");
    }

    [Fact]
    public void Query_Multiple_From_Should_Preserve_Reasons_When_Source_Fails()
    {
        // Arrange
        var context =
            new Success("Context.");

        Result<int> first =
            Result<int>.Failure(
                new IReason[]
                {
                    TestError,
                    context
                });

        Result<int> second =
            Result<int>.Success(20);

        // Act
        Result<int> result =
            from firstValue in first
            from secondValue in second
            select firstValue + secondValue;

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should()
            .BeEquivalentTo(first.Reasons);
    }

    #endregion

    #region Null Guards

    [Fact]
    public void Select_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result result = null!;

        // Act
        Action act = () =>
            result.Select(() => 42);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Select_Should_Throw_When_Selector_Is_Null()
    {
        // Arrange
        Result result = Result.Success();
        Func<int> selector = null!;

        // Act
        Action act = () =>
            result.Select(selector);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("selector");
    }

    [Fact]
    public void Select_Generic_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Action act = () =>
            result.Select(value => value.ToString());

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Select_Generic_Should_Throw_When_Selector_Is_Null()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Func<int, string> selector = null!;

        // Act
        Action act = () =>
            result.Select(selector);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("selector");
    }

    [Fact]
    public void SelectMany_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result result = null!;

        // Act
        Action act = () =>
            result.SelectMany(
                () => Result<int>.Success(42));

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void SelectMany_Should_Throw_When_Binder_Is_Null()
    {
        // Arrange
        Result result = Result.Success();

        Func<Result<int>> binder = null!;

        // Act
        Action act = () =>
            result.SelectMany(binder);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("binder");
    }

    [Fact]
    public void SelectMany_With_Projector_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result result = null!;

        // Act
        Action act = () =>
            result.SelectMany(
                () => Result<int>.Success(42),
                (unit, value) => value);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void SelectMany_With_Projector_Should_Throw_When_Binder_Is_Null()
    {
        // Arrange
        Result result = Result.Success();

        Func<Result<int>> binder = null!;

        // Act
        Action act = () =>
            result.SelectMany(
                binder,
                (unit, value) => value);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("binder");
    }

    [Fact]
    public void SelectMany_With_Projector_Should_Throw_When_Projector_Is_Null()
    {
        // Arrange
        Result result = Result.Success();

        Func<Unit, int, int> projector = null!;

        // Act
        Action act = () =>
            result.SelectMany(
                () => Result<int>.Success(42),
                projector);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("projector");
    }

    [Fact]
    public void SelectMany_Generic_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Action act = () =>
            result.SelectMany(
                value => Result<string>.Success(
                    value.ToString()));

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void SelectMany_Generic_Should_Throw_When_Binder_Is_Null()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Func<int, Result<string>> binder = null!;

        // Act
        Action act = () =>
            result.SelectMany(binder);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("binder");
    }

    [Fact]
    public void SelectMany_Generic_With_Projector_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Action act = () =>
            result.SelectMany(
                value => Result<int>.Success(value),
                (first, second) => first + second);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void SelectMany_Generic_With_Projector_Should_Throw_When_Binder_Is_Null()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Func<int, Result<int>> binder = null!;

        // Act
        Action act = () =>
            result.SelectMany(
                binder,
                (first, second) => first + second);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("binder");
    }

    [Fact]
    public void SelectMany_Generic_With_Projector_Should_Throw_When_Projector_Is_Null()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Func<int, int, int> projector = null!;

        // Act
        Action act = () =>
            result.SelectMany(
                value => Result<int>.Success(value),
                projector);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("projector");
    }

    [Fact]
    public void Where_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Action act = () =>
            result.Where(value => value > 0);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Where_Should_Throw_When_Predicate_Is_Null()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Func<int, bool> predicate = null!;

        // Act
        Action act = () =>
            result.Where(predicate);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("predicate");
    }

    [Fact]
    public void Where_With_Error_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Action act = () =>
            result.Where(
                value => value > 0,
                TestError);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Where_With_Error_Should_Throw_When_Predicate_Is_Null()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Func<int, bool> predicate = null!;

        // Act
        Action act = () =>
            result.Where(
                predicate,
                TestError);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("predicate");
    }

    [Fact]
    public void Where_With_Error_Should_Throw_When_Error_Is_Null()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Error error = null!;

        // Act
        Action act = () =>
            result.Where(
                value => value > 0,
                error);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("error");
    }

    #endregion
}