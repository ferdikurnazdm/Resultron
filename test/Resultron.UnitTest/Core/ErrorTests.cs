using FluentAssertions;
using Xunit;

namespace Resultron.UnitTest;

public sealed class ErrorTests
{
    #region Constructor

    [Fact]
    public void Constructor_Should_Set_Code_And_Description()
    {
        // Act
        var error = new Error(
            "user.not_found",
            "User was not found.");

        // Assert
        error.Code.Should().Be("user.not_found");
        error.Description.Should().Be("User was not found.");
        error.Exception.Should().BeNull();
    }

    [Fact]
    public void Constructor_Should_Set_Exception()
    {
        // Arrange
        var exception =
            new InvalidOperationException("Something failed.");

        // Act
        var error = new Error(
            "operation.failed",
            "Operation failed.",
            exception);

        // Assert
        error.Exception.Should().BeSameAs(exception);
    }

    [Fact]
    public void Constructor_Should_Initialize_Empty_Metadata()
    {
        // Act
        var error = new Error(
            "test.error",
            "Test error.");

        // Assert
        error.Metadata.Should().NotBeNull();
        error.Metadata.Should().BeEmpty();
    }

    #endregion

    #region IReason

    [Fact]
    public void IReason_Message_Should_Return_Description()
    {
        // Arrange
        var error = new Error(
            "test.error",
            "Test error description.");

        // Act
        IReason reason = error;

        // Assert
        reason.Message.Should()
            .Be("Test error description.");
    }

    [Fact]
    public void IReason_Metadata_Should_Return_Error_Metadata()
    {
        // Arrange
        Error error = new Error(
            "test.error",
            "Test error.")
            .WithMetadata(
                "TraceId",
                "ABC-123");

        // Act
        IReason reason = error;

        // Assert
        reason.Metadata.Should()
            .ContainKey("TraceId");

        reason.Metadata["TraceId"]
            .Should()
            .Be("ABC-123");
    }

    #endregion

    #region None

    [Fact]
    public void None_Should_Have_Empty_Code()
    {
        // Assert
        Error.None.Code.Should().BeEmpty();
    }

    [Fact]
    public void None_Should_Have_Empty_Description()
    {
        // Assert
        Error.None.Description.Should().BeEmpty();
    }

    [Fact]
    public void None_Should_Have_No_Exception()
    {
        // Assert
        Error.None.Exception.Should().BeNull();
    }

    [Fact]
    public void None_Should_Have_Empty_Metadata()
    {
        // Assert
        Error.None.Metadata.Should().BeEmpty();
    }

    [Fact]
    public void None_Should_Expose_Empty_Message_Through_IReason()
    {
        // Arrange
        IReason reason = Error.None;

        // Assert
        reason.Message.Should().BeEmpty();
    }

    #endregion

    #region WithMetadata

    [Fact]
    public void WithMetadata_Should_Add_Metadata()
    {
        // Arrange
        var source = new Error(
            "test.error",
            "Test error.");

        // Act
        Error result = source.WithMetadata(
            "TraceId",
            "ABC-123");

        // Assert
        result.Metadata.Should()
            .ContainKey("TraceId");

        result.Metadata["TraceId"]
            .Should()
            .Be("ABC-123");
    }

    [Fact]
    public void WithMetadata_Should_Return_New_Error_Instance()
    {
        // Arrange
        var source = new Error(
            "test.error",
            "Test error.");

        // Act
        Error result = source.WithMetadata(
            "TraceId",
            "ABC-123");

        // Assert
        result.Should().NotBeSameAs(source);
    }

    [Fact]
    public void WithMetadata_Should_Not_Modify_Original_Error()
    {
        // Arrange
        var source = new Error(
            "test.error",
            "Test error.");

        // Act
        Error result = source.WithMetadata(
            "TraceId",
            "ABC-123");

        // Assert
        source.Metadata.Should().BeEmpty();

        result.Metadata.Should()
            .ContainKey("TraceId");
    }

    [Fact]
    public void WithMetadata_Should_Preserve_Existing_Metadata()
    {
        // Arrange
        Error source = new Error(
            "test.error",
            "Test error.")
            .WithMetadata(
                "TraceId",
                "ABC-123");

        // Act
        Error result = source.WithMetadata(
            "UserId",
            42);

        // Assert
        result.Metadata.Should().HaveCount(2);

        result.Metadata["TraceId"]
            .Should()
            .Be("ABC-123");

        result.Metadata["UserId"]
            .Should()
            .Be(42);
    }

    [Fact]
    public void WithMetadata_Should_Update_Existing_Key()
    {
        // Arrange
        Error source = new Error(
            "test.error",
            "Test error.")
            .WithMetadata(
                "TraceId",
                "ABC-123");

        // Act
        Error result = source.WithMetadata(
            "TraceId",
            "XYZ-456");

        // Assert
        result.Metadata.Should().ContainSingle();

        result.Metadata["TraceId"]
            .Should()
            .Be("XYZ-456");
    }

    [Fact]
    public void WithMetadata_Should_Treat_Keys_As_Case_Insensitive()
    {
        // Arrange
        Error source = new Error(
            "test.error",
            "Test error.")
            .WithMetadata(
                "TraceId",
                "ABC-123");

        // Act
        Error result = source.WithMetadata(
            "traceid",
            "XYZ-456");

        // Assert
        result.Metadata.Should().ContainSingle();

        result.Metadata["TRACEID"]
            .Should()
            .Be("XYZ-456");
    }

    [Fact]
    public void WithMetadata_Should_Preserve_Error_Properties()
    {
        // Arrange
        var exception =
            new InvalidOperationException("Failure.");

        var source = new Error(
            "test.error",
            "Test error.",
            exception);

        // Act
        Error result = source.WithMetadata(
            "TraceId",
            "ABC-123");

        // Assert
        result.Code.Should().Be(source.Code);
        result.Description.Should().Be(source.Description);
        result.Exception.Should().BeSameAs(exception);
    }

    [Fact]
    public void WithMetadata_Should_Support_Chaining()
    {
        // Arrange
        var source = new Error(
            "test.error",
            "Test error.");

        // Act
        Error result = source
            .WithMetadata(
                "TraceId",
                "ABC-123")
            .WithMetadata(
                "UserId",
                42)
            .WithMetadata(
                "RetryCount",
                3);

        // Assert
        result.Metadata.Should().HaveCount(3);

        result.Metadata["TraceId"]
            .Should()
            .Be("ABC-123");

        result.Metadata["UserId"]
            .Should()
            .Be(42);

        result.Metadata["RetryCount"]
            .Should()
            .Be(3);
    }

    [Fact]
    public void WithMetadata_Should_Allow_Null_Value()
    {
        // Arrange
        var source = new Error(
            "test.error",
            "Test error.");

        // Act
        Error result = source.WithMetadata(
            "OptionalValue",
            null!);

        // Assert
        result.Metadata.Should()
            .ContainKey("OptionalValue");

        result.Metadata["OptionalValue"]
            .Should()
            .BeNull();
    }

    #endregion

    #region CausedBy

    [Fact]
    public void CausedBy_Should_Set_Exception()
    {
        // Arrange
        var source = new Error(
            "operation.failed",
            "Operation failed.");

        var exception =
            new InvalidOperationException(
                "Database connection failed.");

        // Act
        Error result =
            source.CausedBy(exception);

        // Assert
        result.Exception.Should()
            .BeSameAs(exception);
    }

    [Fact]
    public void CausedBy_Should_Return_New_Error_Instance()
    {
        // Arrange
        var source = new Error(
            "operation.failed",
            "Operation failed.");

        var exception =
            new InvalidOperationException();

        // Act
        Error result =
            source.CausedBy(exception);

        // Assert
        result.Should().NotBeSameAs(source);
    }

    [Fact]
    public void CausedBy_Should_Not_Modify_Original_Error()
    {
        // Arrange
        var source = new Error(
            "operation.failed",
            "Operation failed.");

        var exception =
            new InvalidOperationException();

        // Act
        Error result =
            source.CausedBy(exception);

        // Assert
        source.Exception.Should().BeNull();

        result.Exception.Should()
            .BeSameAs(exception);
    }

    [Fact]
    public void CausedBy_Should_Replace_Existing_Exception()
    {
        // Arrange
        var firstException =
            new InvalidOperationException(
                "First exception.");

        var secondException =
            new ArgumentException(
                "Second exception.");

        var source = new Error(
            "operation.failed",
            "Operation failed.",
            firstException);

        // Act
        Error result =
            source.CausedBy(secondException);

        // Assert
        source.Exception.Should()
            .BeSameAs(firstException);

        result.Exception.Should()
            .BeSameAs(secondException);
    }

    [Fact]
    public void CausedBy_Should_Preserve_Metadata()
    {
        // Arrange
        Error source = new Error(
            "operation.failed",
            "Operation failed.")
            .WithMetadata(
                "TraceId",
                "ABC-123");

        var exception =
            new InvalidOperationException();

        // Act
        Error result =
            source.CausedBy(exception);

        // Assert
        result.Metadata.Should()
            .ContainKey("TraceId");

        result.Metadata["TraceId"]
            .Should()
            .Be("ABC-123");
    }

    [Fact]
    public void CausedBy_Should_Preserve_Code_And_Description()
    {
        // Arrange
        var source = new Error(
            "operation.failed",
            "Operation failed.");

        // Act
        Error result = source.CausedBy(
            new InvalidOperationException());

        // Assert
        result.Code.Should()
            .Be("operation.failed");

        result.Description.Should()
            .Be("Operation failed.");
    }

    #endregion

    #region Combined Operations

    [Fact]
    public void Error_Should_Support_Metadata_And_Exception_Chaining()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Database failed.");

        // Act
        Error result = new Error(
            "database.error",
            "Database operation failed.")
            .WithMetadata(
                "TraceId",
                "ABC-123")
            .WithMetadata(
                "RetryCount",
                3)
            .CausedBy(exception);

        // Assert
        result.Code.Should()
            .Be("database.error");

        result.Description.Should()
            .Be("Database operation failed.");

        result.Exception.Should()
            .BeSameAs(exception);

        result.Metadata.Should()
            .HaveCount(2);

        result.Metadata["TraceId"]
            .Should()
            .Be("ABC-123");

        result.Metadata["RetryCount"]
            .Should()
            .Be(3);
    }

    [Fact]
    public void WithMetadata_After_CausedBy_Should_Preserve_Exception()
    {
        // Arrange
        var exception =
            new InvalidOperationException();

        Error source = new Error(
            "test.error",
            "Test error.")
            .CausedBy(exception);

        // Act
        Error result = source.WithMetadata(
            "TraceId",
            "ABC-123");

        // Assert
        result.Exception.Should()
            .BeSameAs(exception);

        result.Metadata["TraceId"]
            .Should()
            .Be("ABC-123");
    }

    #endregion

    #region Record Semantics

    [Fact]
    public void Error_With_Same_Core_Values_Should_Have_Equivalent_Properties()
    {
        // Arrange
        var first = new Error(
            "test.error",
            "Test error.");

        var second = new Error(
            "test.error",
            "Test error.");

        // Assert
        first.Code.Should().Be(second.Code);
        first.Description.Should().Be(second.Description);
        first.Exception.Should().Be(second.Exception);

        first.Metadata.Should()
            .BeEquivalentTo(second.Metadata);
    }

    [Fact]
    public void Error_With_Different_Code_Should_Not_Be_Equal()
    {
        // Arrange
        var first = new Error(
            "first.error",
            "Test error.");

        var second = new Error(
            "second.error",
            "Test error.");

        // Assert
        first.Should().NotBe(second);
    }

    [Fact]
    public void Error_With_Different_Description_Should_Not_Be_Equal()
    {
        // Arrange
        var first = new Error(
            "test.error",
            "First description.");

        var second = new Error(
            "test.error",
            "Second description.");

        // Assert
        first.Should().NotBe(second);
    }

    #endregion

    #region Null Guards

    [Fact]
    public void WithMetadata_Should_Throw_When_Key_Is_Null()
    {
        // Arrange
        var error = new Error(
            "test.error",
            "Test error.");

        string key = null!;

        // Act
        Action act = () =>
            error.WithMetadata(
                key,
                "value");

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("key");
    }

    [Fact]
    public void WithMetadata_Should_Throw_When_Key_Is_Empty()
    {
        // Arrange
        var error = new Error(
            "test.error",
            "Test error.");

        // Act
        Action act = () =>
            error.WithMetadata(
                string.Empty,
                "value");

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("key");
    }

    [Fact]
    public void CausedBy_Should_Throw_When_Exception_Is_Null()
    {
        // Arrange
        var error = new Error(
            "test.error",
            "Test error.");

        Exception exception = null!;

        // Act
        Action act = () =>
            error.CausedBy(exception);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("exception");
    }

    #endregion
}