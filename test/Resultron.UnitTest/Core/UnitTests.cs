using FluentAssertions;
using Xunit;

namespace Resultron.UnitTest;

public sealed class UnitTests
{
    #region Value

    [Fact]
    public void Value_Should_Be_Valid_Unit_Instance()
    {
        // Act
        Unit unit = Unit.Value;

        // Assert
        unit.Should().Be(default(Unit));
    }

    [Fact]
    public void Default_Unit_Should_Equal_Value()
    {
        // Arrange
        Unit unit = default;

        // Assert
        unit.Should().Be(Unit.Value);
    }

    [Fact]
    public void All_Unit_Instances_Should_Be_Equal()
    {
        // Arrange
        Unit first = new();
        Unit second = new();

        // Assert
        first.Should().Be(second);
        first.Should().Be(Unit.Value);
        second.Should().Be(Unit.Value);
    }

    #endregion

    #region Equals

    [Fact]
    public void Equals_Unit_Should_Return_True()
    {
        // Arrange
        Unit first = Unit.Value;
        Unit second = default;

        // Act
        bool result = first.Equals(second);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_Object_Should_Return_True_When_Object_Is_Unit()
    {
        // Arrange
        Unit unit = Unit.Value;
        object other = new Unit();

        // Act
        bool result = unit.Equals(other);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_Object_Should_Return_False_When_Object_Is_Not_Unit()
    {
        // Arrange
        Unit unit = Unit.Value;
        object other = "not-unit";

        // Act
        bool result = unit.Equals(other);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_Object_Should_Return_False_When_Object_Is_Null()
    {
        // Arrange
        Unit unit = Unit.Value;

        // Act
        bool result = unit.Equals(null);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Equality Operators

    [Fact]
    public void Equality_Operator_Should_Return_True()
    {
        // Arrange
        Unit first = Unit.Value;
        Unit second = default;

        // Act
        bool result = first == second;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Inequality_Operator_Should_Return_False()
    {
        // Arrange
        Unit first = Unit.Value;
        Unit second = default;

        // Act
        bool result = first != second;

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region GetHashCode

    [Fact]
    public void GetHashCode_Should_Return_Zero()
    {
        // Arrange
        Unit unit = Unit.Value;

        // Act
        int hashCode = unit.GetHashCode();

        // Assert
        hashCode.Should().Be(0);
    }

    [Fact]
    public void Equal_Unit_Instances_Should_Have_Same_Hash_Code()
    {
        // Arrange
        Unit first = Unit.Value;
        Unit second = default;

        // Assert
        first.GetHashCode()
            .Should()
            .Be(second.GetHashCode());
    }

    #endregion

    #region IEquatable

    [Fact]
    public void IEquatable_Equals_Should_Return_True()
    {
        // Arrange
        IEquatable<Unit> first = Unit.Value;
        Unit second = default;

        // Act
        bool result = first.Equals(second);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region IComparable<Unit>

    [Fact]
    public void CompareTo_Unit_Should_Return_Zero()
    {
        // Arrange
        Unit first = Unit.Value;
        Unit second = default;

        // Act
        int result = first.CompareTo(second);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void Generic_Comparable_Should_Return_Zero()
    {
        // Arrange
        IComparable<Unit> comparable =
            Unit.Value;

        // Act
        int result =
            comparable.CompareTo(default);

        // Assert
        result.Should().Be(0);
    }

    #endregion

    #region IComparable

    [Fact]
    public void CompareTo_Object_Should_Return_Zero_When_Object_Is_Unit()
    {
        // Arrange
        Unit unit = Unit.Value;
        object other = new Unit();

        // Act
        int result =
            unit.CompareTo(other);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void NonGeneric_Comparable_Should_Return_Zero_When_Object_Is_Unit()
    {
        // Arrange
        IComparable comparable =
            Unit.Value;

        object other =
            new Unit();

        // Act
        int result =
            comparable.CompareTo(other);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void CompareTo_Object_Should_Throw_When_Object_Is_Not_Unit()
    {
        // Arrange
        Unit unit = Unit.Value;
        object other = "not-unit";

        // Act
        Action act = () =>
            unit.CompareTo(other);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Object must be of type Unit.*")
            .WithParameterName("obj");
    }

    [Fact]
    public void CompareTo_Object_Should_Throw_When_Object_Is_Null()
    {
        // Arrange
        Unit unit = Unit.Value;

        // Act
        Action act = () =>
            unit.CompareTo(null);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Object must be of type Unit.*")
            .WithParameterName("obj");
    }

    #endregion

    #region ToString

    [Fact]
    public void ToString_Should_Return_Unit_Representation()
    {
        // Arrange
        Unit unit = Unit.Value;

        // Act
        string result =
            unit.ToString();

        // Assert
        result.Should().Be("()");
    }

    [Fact]
    public void Default_Unit_ToString_Should_Return_Unit_Representation()
    {
        // Arrange
        Unit unit = default;

        // Act
        string result =
            unit.ToString();

        // Assert
        result.Should().Be("()");
    }

    #endregion

    #region Result Integration

    [Fact]
    public void Unit_Should_Be_Usable_As_Result_Value()
    {
        // Arrange
        Result<Unit> result =
            Result<Unit>.Success(Unit.Value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
    }

    [Fact]
    public void Successful_Unit_Result_Should_Contain_Unit_Representation()
    {
        // Arrange
        Result<Unit> result =
            Result<Unit>.Success(Unit.Value);

        // Act
        string value =
            result.Value.ToString();

        // Assert
        value.Should().Be("()");
    }

    #endregion
}