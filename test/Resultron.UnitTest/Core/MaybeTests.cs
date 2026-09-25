using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Resultron.UnitTest;

public sealed class MaybeTests
{
    #region Some / None

    [Fact]
    public void Some_Should_Create_Maybe_With_Value()
    {
        // Act
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        // Assert
        maybe.HasValue.Should().BeTrue();
        maybe.HasNoValue.Should().BeFalse();
        maybe.Value.Should().Be(42);
    }

    [Fact]
    public void None_Should_Create_Maybe_Without_Value()
    {
        // Act
        Maybe<int> maybe =
            Maybe<int>.None();

        // Assert
        maybe.HasValue.Should().BeFalse();
        maybe.HasNoValue.Should().BeTrue();
    }

    [Fact]
    public void Default_Maybe_Should_Represent_None()
    {
        // Act
        Maybe<int> maybe = default;

        // Assert
        maybe.HasValue.Should().BeFalse();
        maybe.HasNoValue.Should().BeTrue();
    }

    [Fact]
    public void Value_Should_Throw_When_Maybe_Has_No_Value()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        // Act
        Action act = () =>
        {
            _ = maybe.Value;
        };

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Maybe has no value.");
    }

    #endregion

    #region From

    [Fact]
    public void From_Should_Create_Some_When_Value_Is_Not_Null()
    {
        // Act
        Maybe<string> maybe =
            Maybe<string>.From("Resultron");

        // Assert
        maybe.HasValue.Should().BeTrue();
        maybe.Value.Should().Be("Resultron");
    }

    [Fact]
    public void From_Should_Create_None_When_Value_Is_Null()
    {
        // Arrange
        string? value = null;

        // Act
        Maybe<string> maybe =
            Maybe<string>.From(value);

        // Assert
        maybe.HasValue.Should().BeFalse();
        maybe.HasNoValue.Should().BeTrue();
    }

    [Fact]
    public void From_Should_Create_Some_For_Value_Type_Default_Value()
    {
        // Act
        Maybe<int> maybe =
            Maybe<int>.From(0);

        // Assert
        maybe.HasValue.Should().BeTrue();
        maybe.Value.Should().Be(0);
    }

    #endregion

    #region Implicit Conversion

    [Fact]
    public void Implicit_Conversion_Should_Create_Some_For_Non_Null_Value()
    {
        // Act
        Maybe<string> maybe =
            "Resultron";

        // Assert
        maybe.HasValue.Should().BeTrue();
        maybe.Value.Should().Be("Resultron");
    }

    [Fact]
    public void Implicit_Conversion_Should_Create_None_For_Null_Value()
    {
        // Arrange
        string? value = null;

        // Act
        Maybe<string> maybe = value;

        // Assert
        maybe.HasValue.Should().BeFalse();
        maybe.HasNoValue.Should().BeTrue();
    }

    [Fact]
    public void Implicit_Conversion_Should_Create_Some_For_Value_Type()
    {
        // Act
        Maybe<int> maybe = 42;

        // Assert
        maybe.HasValue.Should().BeTrue();
        maybe.Value.Should().Be(42);
    }

    #endregion

    #region GetValueOrDefault

    [Fact]
    public void GetValueOrDefault_Should_Return_Value_When_Some()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        // Act
        int value =
            maybe.GetValueOrDefault();

        // Assert
        value.Should().Be(42);
    }

    [Fact]
    public void GetValueOrDefault_Should_Return_Default_When_None()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        // Act
        int value =
            maybe.GetValueOrDefault();

        // Assert
        value.Should().Be(0);
    }

    [Fact]
    public void GetValueOrDefault_Reference_Type_Should_Return_Null_When_None()
    {
        // Arrange
        Maybe<string> maybe =
            Maybe<string>.None();

        // Act
        string? value =
            maybe.GetValueOrDefault();

        // Assert
        value.Should().BeNull();
    }

    [Fact]
    public void GetValueOrDefault_With_Fallback_Should_Return_Value_When_Some()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        // Act
        int value =
            maybe.GetValueOrDefault(100);

        // Assert
        value.Should().Be(42);
    }

    [Fact]
    public void GetValueOrDefault_With_Fallback_Should_Return_Fallback_When_None()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        // Act
        int value =
            maybe.GetValueOrDefault(100);

        // Assert
        value.Should().Be(100);
    }

    #endregion

    #region GetValueOrElse

    [Fact]
    public void GetValueOrElse_Should_Return_Value_When_Some()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Func<int> factory =
            Substitute.For<Func<int>>();

        factory()
            .Returns(100);

        // Act
        int value =
            maybe.GetValueOrElse(factory);

        // Assert
        value.Should().Be(42);

        factory.DidNotReceive().Invoke();
    }

    [Fact]
    public void GetValueOrElse_Should_Invoke_Factory_When_None()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        Func<int> factory =
            Substitute.For<Func<int>>();

        factory()
            .Returns(100);

        // Act
        int value =
            maybe.GetValueOrElse(factory);

        // Assert
        value.Should().Be(100);

        factory.Received(1).Invoke();
    }

    #endregion

    #region Map

    [Fact]
    public void Map_Should_Transform_Value_When_Some()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        // Act
        Maybe<string> result =
            maybe.Map(
                value => $"Value: {value}");

        // Assert
        result.HasValue.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public void Map_Should_Pass_Value_To_Mapper()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Func<int, string> mapper =
            Substitute.For<Func<int, string>>();

        mapper(42)
            .Returns("mapped");

        // Act
        Maybe<string> result =
            maybe.Map(mapper);

        // Assert
        result.Value.Should().Be("mapped");

        mapper.Received(1)
            .Invoke(42);
    }

    [Fact]
    public void Map_Should_Return_None_When_Source_Is_None()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        // Act
        Maybe<string> result =
            maybe.Map(
                value => value.ToString());

        // Assert
        result.HasValue.Should().BeFalse();
        result.HasNoValue.Should().BeTrue();
    }

    [Fact]
    public void Map_Should_Not_Invoke_Mapper_When_Source_Is_None()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        Func<int, string> mapper =
            Substitute.For<Func<int, string>>();

        // Act
        Maybe<string> result =
            maybe.Map(mapper);

        // Assert
        result.HasNoValue.Should().BeTrue();

        mapper.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    [Fact]
    public void Map_Should_Return_None_When_Mapper_Returns_Null()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        // Act
        Maybe<string> result =
            maybe.Map<string>(
                _ => null!);

        // Assert
        result.HasValue.Should().BeFalse();
        result.HasNoValue.Should().BeTrue();
    }

    [Fact]
    public void Map_Should_Support_Chaining()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(21);

        // Act
        Maybe<string> result = maybe
            .Map(value => value * 2)
            .Map(value => $"Value: {value}");

        // Assert
        result.HasValue.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    #endregion

    #region Bind

    [Fact]
    public void Bind_Should_Return_Binder_Result_When_Some()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        // Act
        Maybe<string> result = maybe.Bind(
            value =>
                Maybe<string>.Some(
                    $"Value: {value}"));

        // Assert
        result.HasValue.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public void Bind_Should_Pass_Value_To_Binder()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Func<int, Maybe<string>> binder =
            Substitute.For<Func<int, Maybe<string>>>();

        binder(42)
            .Returns(
                Maybe<string>.Some("bound"));

        // Act
        Maybe<string> result =
            maybe.Bind(binder);

        // Assert
        result.Value.Should().Be("bound");

        binder.Received(1)
            .Invoke(42);
    }

    [Fact]
    public void Bind_Should_Return_None_When_Binder_Returns_None()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        // Act
        Maybe<string> result = maybe.Bind(
            _ => Maybe<string>.None());

        // Assert
        result.HasValue.Should().BeFalse();
        result.HasNoValue.Should().BeTrue();
    }

    [Fact]
    public void Bind_Should_Return_None_When_Source_Is_None()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        // Act
        Maybe<string> result = maybe.Bind(
            value =>
                Maybe<string>.Some(
                    value.ToString()));

        // Assert
        result.HasValue.Should().BeFalse();
        result.HasNoValue.Should().BeTrue();
    }

    [Fact]
    public void Bind_Should_Not_Invoke_Binder_When_Source_Is_None()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        Func<int, Maybe<string>> binder =
            Substitute.For<Func<int, Maybe<string>>>();

        // Act
        Maybe<string> result =
            maybe.Bind(binder);

        // Assert
        result.HasNoValue.Should().BeTrue();

        binder.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    [Fact]
    public void Bind_Should_Support_Chaining()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(10);

        // Act
        Maybe<string> result = maybe
            .Bind(value =>
                Maybe<int>.Some(value + 10))
            .Bind(value =>
                Maybe<string>.Some(
                    $"Value: {value}"));

        // Assert
        result.HasValue.Should().BeTrue();
        result.Value.Should().Be("Value: 20");
    }

    #endregion

    #region Match

    [Fact]
    public void Match_Should_Invoke_Some_Function_When_Value_Exists()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Func<int, string> some =
            Substitute.For<Func<int, string>>();

        Func<string> none =
            Substitute.For<Func<string>>();

        some(42)
            .Returns("some");

        // Act
        string result =
            maybe.Match(
                some,
                none);

        // Assert
        result.Should().Be("some");

        some.Received(1)
            .Invoke(42);

        none.DidNotReceive().Invoke();
    }

    [Fact]
    public void Match_Should_Invoke_None_Function_When_Value_Does_Not_Exist()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        Func<int, string> some =
            Substitute.For<Func<int, string>>();

        Func<string> none =
            Substitute.For<Func<string>>();

        none()
            .Returns("none");

        // Act
        string result =
            maybe.Match(
                some,
                none);

        // Assert
        result.Should().Be("none");

        some.DidNotReceive()
            .Invoke(Arg.Any<int>());

        none.Received(1).Invoke();
    }

    [Fact]
    public void Match_Should_Return_Some_Result_When_Value_Exists()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        // Act
        string result = maybe.Match(
            value => $"Value: {value}",
            () => "No value");

        // Assert
        result.Should().Be("Value: 42");
    }

    [Fact]
    public void Match_Should_Return_None_Result_When_Value_Does_Not_Exist()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        // Act
        string result = maybe.Match(
            value => $"Value: {value}",
            () => "No value");

        // Assert
        result.Should().Be("No value");
    }

    #endregion

    #region IfSome

    [Fact]
    public void IfSome_Should_Invoke_Action_When_Value_Exists()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Action<int> action =
            Substitute.For<Action<int>>();

        // Act
        maybe.IfSome(action);

        // Assert
        action.Received(1)
            .Invoke(42);
    }

    [Fact]
    public void IfSome_Should_Pass_Value_To_Action()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        var receivedValue = 0;

        // Act
        maybe.IfSome(
            value => receivedValue = value);

        // Assert
        receivedValue.Should().Be(42);
    }

    [Fact]
    public void IfSome_Should_Not_Invoke_Action_When_None()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        Action<int> action =
            Substitute.For<Action<int>>();

        // Act
        maybe.IfSome(action);

        // Assert
        action.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    #endregion

    #region IfNone

    [Fact]
    public void IfNone_Should_Invoke_Action_When_None()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        Action action =
            Substitute.For<Action>();

        // Act
        maybe.IfNone(action);

        // Assert
        action.Received(1).Invoke();
    }

    [Fact]
    public void IfNone_Should_Not_Invoke_Action_When_Value_Exists()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Action action =
            Substitute.For<Action>();

        // Act
        maybe.IfNone(action);

        // Assert
        action.DidNotReceive().Invoke();
    }

    #endregion

    #region Some / None Interaction

    [Fact]
    public void IfSome_And_IfNone_Should_Invoke_Only_Some_For_Some_Value()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Action<int> some =
            Substitute.For<Action<int>>();

        Action none =
            Substitute.For<Action>();

        // Act
        maybe.IfSome(some);
        maybe.IfNone(none);

        // Assert
        some.Received(1)
            .Invoke(42);

        none.DidNotReceive().Invoke();
    }

    [Fact]
    public void IfSome_And_IfNone_Should_Invoke_Only_None_For_None_Value()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        Action<int> some =
            Substitute.For<Action<int>>();

        Action none =
            Substitute.For<Action>();

        // Act
        maybe.IfSome(some);
        maybe.IfNone(none);

        // Assert
        some.DidNotReceive()
            .Invoke(Arg.Any<int>());

        none.Received(1).Invoke();
    }

    #endregion

    #region Null Guards

    [Fact]
    public void GetValueOrElse_Should_Throw_When_Factory_Is_Null()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Func<int> factory = null!;

        // Act
        Action act = () =>
            maybe.GetValueOrElse(factory);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("factory");
    }

    [Fact]
    public void GetValueOrElse_Should_Throw_When_Factory_Is_Null_Even_When_Value_Exists()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Func<int> factory = null!;

        // Act
        Action act = () =>
            maybe.GetValueOrElse(factory);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("factory");
    }

    [Fact]
    public void Map_Should_Throw_When_Mapper_Is_Null()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Func<int, string> mapper = null!;

        // Act
        Action act = () =>
            maybe.Map(mapper);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("func");
    }

    [Fact]
    public void Bind_Should_Throw_When_Binder_Is_Null()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Func<int, Maybe<string>> binder = null!;

        // Act
        Action act = () =>
            maybe.Bind(binder);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("func");
    }

    [Fact]
    public void Match_Should_Throw_When_Some_Is_Null()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Func<int, string> some = null!;

        // Act
        Action act = () =>
            maybe.Match(
                some,
                () => "none");

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("some");
    }

    [Fact]
    public void Match_Should_Throw_When_None_Is_Null()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Func<string> none = null!;

        // Act
        Action act = () =>
            maybe.Match(
                value => value.ToString(),
                none);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("none");
    }

    [Fact]
    public void Match_Should_Throw_When_Some_Is_Null_Even_When_Maybe_Is_None()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        Func<int, string> some = null!;

        // Act
        Action act = () =>
            maybe.Match(
                some,
                () => "none");

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("some");
    }

    [Fact]
    public void Match_Should_Throw_When_None_Is_Null_Even_When_Maybe_Is_Some()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Func<string> none = null!;

        // Act
        Action act = () =>
            maybe.Match(
                value => value.ToString(),
                none);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("none");
    }

    [Fact]
    public void IfSome_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Action<int> action = null!;

        // Act
        Action act = () =>
            maybe.IfSome(action);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public void IfSome_Should_Throw_When_Action_Is_Null_Even_When_Maybe_Is_None()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        Action<int> action = null!;

        // Act
        Action act = () =>
            maybe.IfSome(action);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public void IfNone_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.None();

        Action action = null!;

        // Act
        Action act = () =>
            maybe.IfNone(action);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public void IfNone_Should_Throw_When_Action_Is_Null_Even_When_Maybe_Is_Some()
    {
        // Arrange
        Maybe<int> maybe =
            Maybe<int>.Some(42);

        Action action = null!;

        // Act
        Action act = () =>
            maybe.IfNone(action);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("action");
    }

    #endregion
}