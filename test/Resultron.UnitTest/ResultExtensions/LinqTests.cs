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
        Result source = Result.Success();

        Result<int> result = source.Select(
            () => 42);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Select_Should_Not_Invoke_Selector_When_Result_Is_Failure()
    {
        Result source =
            Result.Failure(TestError);

        Func<int> selector =
            Substitute.For<Func<int>>();

        Result<int> result =
            source.Select(selector);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        selector.DidNotReceive()
            .Invoke();
    }

    [Fact]
    public void Select_Should_Propagate_Error_When_Result_Is_Failure()
    {
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        Result source =
            Result.Failure(error);

        Result<int> result =
            source.Select(() => 42);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    #endregion

    #region Select - Result<T>

    [Fact]
    public void Select_Generic_Should_Project_Value()
    {
        Result<int> source =
            Result<int>.Success(42);

        Result<string> result = source.Select(
            value => $"Value: {value}");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public void Select_Generic_Should_Pass_Value_To_Selector()
    {
        Result<int> source =
            Result<int>.Success(42);

        Func<int, string> selector =
            Substitute.For<Func<int, string>>();

        selector(42)
            .Returns("mapped");

        Result<string> result =
            source.Select(selector);

        result.Value.Should().Be("mapped");

        selector.Received(1)
            .Invoke(42);
    }

    [Fact]
    public void Select_Generic_Should_Not_Invoke_Selector_When_Result_Is_Failure()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<int, string> selector =
            Substitute.For<Func<int, string>>();

        Result<string> result =
            source.Select(selector);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        selector.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    #endregion

    #region SelectMany - Result

    [Fact]
    public void SelectMany_Should_Invoke_Binder_When_Result_Is_Success()
    {
        Result source = Result.Success();

        Func<Result<int>> binder =
            Substitute.For<Func<Result<int>>>();

        binder()
            .Returns(Result<int>.Success(42));

        Result<int> result =
            source.SelectMany(binder);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);

        binder.Received(1)
            .Invoke();
    }

    [Fact]
    public void SelectMany_Should_Not_Invoke_Binder_When_Result_Is_Failure()
    {
        Result source =
            Result.Failure(TestError);

        Func<Result<int>> binder =
            Substitute.For<Func<Result<int>>>();

        Result<int> result =
            source.SelectMany(binder);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        binder.DidNotReceive()
            .Invoke();
    }

    [Fact]
    public void SelectMany_With_Projector_Should_Project_Intermediate_Value()
    {
        Result source = Result.Success();

        Result<string> result = source.SelectMany(
            binder: () => Result<int>.Success(42),
            projector: (unit, value) =>
                $"Value: {value}");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public void SelectMany_With_Projector_Should_Propagate_Source_Error()
    {
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        Result source =
            Result.Failure(error);

        Result<string> result = source.SelectMany(
            binder: () => Result<int>.Success(42),
            projector: (unit, value) =>
                value.ToString());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void SelectMany_With_Projector_Should_Propagate_Intermediate_Error()
    {
        Result source = Result.Success();

        var error =
            new Error(
                "intermediate.failed",
                "Intermediate operation failed.");

        Result<int> intermediate =
            Result<int>.Failure(error);

        Result<string> result = source.SelectMany(
            binder: () => intermediate,
            projector: (unit, value) =>
                value.ToString());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    #endregion

    #region SelectMany - Result<T>

    [Fact]
    public void SelectMany_Generic_Should_Bind_Value()
    {
        Result<int> source =
            Result<int>.Success(42);

        Result<string> result = source.SelectMany(
            value => Result<string>.Success(
                $"Value: {value}"));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public void SelectMany_Generic_Should_Pass_Value_To_Binder()
    {
        Result<int> source =
            Result<int>.Success(42);

        Func<int, Result<string>> binder =
            Substitute.For<Func<int, Result<string>>>();

        binder(42)
            .Returns(Result<string>.Success("mapped"));

        Result<string> result =
            source.SelectMany(binder);

        result.Value.Should().Be("mapped");

        binder.Received(1)
            .Invoke(42);
    }

    [Fact]
    public void SelectMany_Generic_With_Projector_Should_Combine_Values()
    {
        Result<int> source =
            Result<int>.Success(10);

        Result<int> result = source.SelectMany(
            binder: first =>
                Result<int>.Success(20),
            projector: (first, second) =>
                first + second);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(30);
    }

    [Fact]
    public void SelectMany_Generic_With_Projector_Should_Not_Invoke_Binder_When_Source_Fails()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<int, Result<int>> binder =
            Substitute.For<Func<int, Result<int>>>();

        Result<int> result = source.SelectMany(
            binder,
            (first, second) => first + second);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        binder.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    [Fact]
    public void SelectMany_Generic_With_Projector_Should_Not_Invoke_Projector_When_Intermediate_Fails()
    {
        Result<int> source =
            Result<int>.Success(10);

        Func<int, int, int> projector =
            Substitute.For<Func<int, int, int>>();

        Result<int> result = source.SelectMany(
            binder: _ =>
                Result<int>.Failure(TestError),
            projector: projector);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        projector.DidNotReceive()
            .Invoke(
                Arg.Any<int>(),
                Arg.Any<int>());
    }

    [Fact]
    public void SelectMany_Generic_With_Projector_Should_Propagate_Intermediate_Error()
    {
        Result<int> source =
            Result<int>.Success(10);

        var error =
            new Error(
                "intermediate.failed",
                "Intermediate operation failed.");

        Result<int> intermediate =
            Result<int>.Failure(error);

        Result<int> result = source.SelectMany(
            _ => intermediate,
            (first, second) => first + second);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    #endregion

    #region Where With Explicit Error

    [Fact]
    public void Where_Should_Return_Original_Result_When_Predicate_Passes()
    {
        Result<int> source =
            Result<int>.Success(42);

        Result<int> result = source.Where(
            value => value > 0,
            TestError);

        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Where_Should_Return_Provided_Error_When_Predicate_Fails()
    {
        Result<int> source =
            Result<int>.Success(42);

        Result<int> result = source.Where(
            value => value < 0,
            TestError);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void Where_Should_Not_Invoke_Predicate_When_Result_Is_Failure()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<int, bool> predicate =
            Substitute.For<Func<int, bool>>();

        Result<int> result = source.Where(
            predicate,
            new Error(
                "other.error",
                "Other error."));

        result.Should().BeSameAs(source);
        result.Error.Should().Be(TestError);

        predicate.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    #endregion

    #region Where With Default Error

    [Fact]
    public void Where_Default_Should_Return_Original_Result_When_Predicate_Passes()
    {
        Result<int> source =
            Result<int>.Success(42);

        Result<int> result =
            source.Where(value => value > 0);

        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Where_Default_Should_Return_Default_Error_When_Predicate_Fails()
    {
        Result<int> source =
            Result<int>.Success(42);

        Result<int> result =
            source.Where(value => value < 0);

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
        Result<int> source =
            Result<int>.Success(42);

        Result<string> result =
            from value in source
            select $"Value: {value}";

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public void Query_Select_Should_Propagate_Failure()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        Result<string> result =
            from value in source
            select $"Value: {value}";

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void Query_SelectMany_Should_Combine_Results()
    {
        Result<int> first =
            Result<int>.Success(10);

        Result<int> second =
            Result<int>.Success(20);

        Result<int> result =
            from firstValue in first
            from secondValue in second
            select firstValue + secondValue;

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(30);
    }

    [Fact]
    public void Query_SelectMany_Should_Short_Circuit_When_First_Result_Fails()
    {
        Result<int> first =
            Result<int>.Failure(TestError);

        Result<int> second =
            Result<int>.Success(20);

        Result<int> result =
            from firstValue in first
            from secondValue in second
            select firstValue + secondValue;

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void Query_SelectMany_Should_Propagate_Second_Result_Failure()
    {
        Result<int> first =
            Result<int>.Success(10);

        Result<int> second =
            Result<int>.Failure(TestError);

        Result<int> result =
            from firstValue in first
            from secondValue in second
            select firstValue + secondValue;

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void Query_Where_Should_Return_Value_When_Predicate_Passes()
    {
        Result<int> source =
            Result<int>.Success(42);

        Result<int> result =
            from value in source
            where value > 0
            select value;

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Query_Where_Should_Return_Default_Error_When_Predicate_Fails()
    {
        Result<int> source =
            Result<int>.Success(42);

        Result<int> result =
            from value in source
            where value < 0
            select value;

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
        Result<int> first =
            Result<int>.Success(10);

        Result<int> second =
            Result<int>.Success(20);

        Result<int> result =
            from firstValue in first
            from secondValue in second
            where firstValue > 0 && secondValue > 0
            select firstValue + secondValue;

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(30);
    }

    [Fact]
    public void Query_Multiple_From_And_Where_Should_Return_Failure_When_Predicate_Fails()
    {
        Result<int> first =
            Result<int>.Success(10);

        Result<int> second =
            Result<int>.Success(20);

        Result<int> result =
            from firstValue in first
            from secondValue in second
            where firstValue < 0 && secondValue > 0
            select firstValue + secondValue;

        result.IsFailure.Should().BeTrue();

        result.Error.Code.Should()
            .Be("result.predicate_failed");

        result.Error.Description.Should()
            .Be(
                "The result did not satisfy the specified condition.");
    }

    [Fact]
    public void Query_Multiple_From_Should_Preserve_Error_When_Source_Fails()
    {
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        Result<int> first =
            Result<int>.Failure(error);

        Result<int> second =
            Result<int>.Success(20);

        Result<int> result =
            from firstValue in first
            from secondValue in second
            select firstValue + secondValue;

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    #endregion

    #region Null Guards

    [Fact]
    public void Select_Should_Throw_When_Result_Is_Null()
    {
        Result result = null!;

        Action act = () =>
            result.Select(() => 42);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Select_Should_Throw_When_Selector_Is_Null()
    {
        Result result = Result.Success();
        Func<int> selector = null!;

        Action act = () =>
            result.Select(selector);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("selector");
    }

    [Fact]
    public void Select_Generic_Should_Throw_When_Result_Is_Null()
    {
        Result<int> result = null!;

        Action act = () =>
            result.Select(
                value => value.ToString());

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Select_Generic_Should_Throw_When_Selector_Is_Null()
    {
        Result<int> result =
            Result<int>.Success(42);

        Func<int, string> selector = null!;

        Action act = () =>
            result.Select(selector);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("selector");
    }

    [Fact]
    public void SelectMany_Should_Throw_When_Result_Is_Null()
    {
        Result result = null!;

        Action act = () =>
            result.SelectMany(
                () => Result<int>.Success(42));

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void SelectMany_Should_Throw_When_Binder_Is_Null()
    {
        Result result = Result.Success();

        Func<Result<int>> binder = null!;

        Action act = () =>
            result.SelectMany(binder);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("binder");
    }

    [Fact]
    public void SelectMany_With_Projector_Should_Throw_When_Result_Is_Null()
    {
        Result result = null!;

        Action act = () =>
            result.SelectMany(
                () => Result<int>.Success(42),
                (unit, value) => value);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void SelectMany_With_Projector_Should_Throw_When_Binder_Is_Null()
    {
        Result result = Result.Success();

        Func<Result<int>> binder = null!;

        Action act = () =>
            result.SelectMany(
                binder,
                (unit, value) => value);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("binder");
    }

    [Fact]
    public void SelectMany_With_Projector_Should_Throw_When_Projector_Is_Null()
    {
        Result result = Result.Success();

        Func<Unit, int, int> projector = null!;

        Action act = () =>
            result.SelectMany(
                () => Result<int>.Success(42),
                projector);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("projector");
    }

    [Fact]
    public void SelectMany_Generic_Should_Throw_When_Result_Is_Null()
    {
        Result<int> result = null!;

        Action act = () =>
            result.SelectMany(
                value => Result<string>.Success(
                    value.ToString()));

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void SelectMany_Generic_Should_Throw_When_Binder_Is_Null()
    {
        Result<int> result =
            Result<int>.Success(42);

        Func<int, Result<string>> binder = null!;

        Action act = () =>
            result.SelectMany(binder);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("binder");
    }

    [Fact]
    public void SelectMany_Generic_With_Projector_Should_Throw_When_Result_Is_Null()
    {
        Result<int> result = null!;

        Action act = () =>
            result.SelectMany(
                value => Result<int>.Success(value),
                (first, second) => first + second);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void SelectMany_Generic_With_Projector_Should_Throw_When_Binder_Is_Null()
    {
        Result<int> result =
            Result<int>.Success(42);

        Func<int, Result<int>> binder = null!;

        Action act = () =>
            result.SelectMany(
                binder,
                (first, second) => first + second);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("binder");
    }

    [Fact]
    public void SelectMany_Generic_With_Projector_Should_Throw_When_Projector_Is_Null()
    {
        Result<int> result =
            Result<int>.Success(42);

        Func<int, int, int> projector = null!;

        Action act = () =>
            result.SelectMany(
                value => Result<int>.Success(value),
                projector);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("projector");
    }

    [Fact]
    public void Where_Should_Throw_When_Result_Is_Null()
    {
        Result<int> result = null!;

        Action act = () =>
            result.Where(value => value > 0);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Where_Should_Throw_When_Predicate_Is_Null()
    {
        Result<int> result =
            Result<int>.Success(42);

        Func<int, bool> predicate = null!;

        Action act = () =>
            result.Where(predicate);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("predicate");
    }

    [Fact]
    public void Where_With_Error_Should_Throw_When_Result_Is_Null()
    {
        Result<int> result = null!;

        Action act = () =>
            result.Where(
                value => value > 0,
                TestError);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Where_With_Error_Should_Throw_When_Predicate_Is_Null()
    {
        Result<int> result =
            Result<int>.Success(42);

        Func<int, bool> predicate = null!;

        Action act = () =>
            result.Where(
                predicate,
                TestError);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("predicate");
    }

    [Fact]
    public void Where_With_Error_Should_Throw_When_Error_Is_Null()
    {
        Result<int> result =
            Result<int>.Success(42);

        Error error = null!;

        Action act = () =>
            result.Where(
                value => value > 0,
                error);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("error");
    }

    #endregion
}