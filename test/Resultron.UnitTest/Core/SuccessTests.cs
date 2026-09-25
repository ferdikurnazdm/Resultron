using FluentAssertions;
using Xunit;

namespace Resultron.UnitTest;

public sealed class SuccessTests
{
    #region Constructor

    [Fact]
    public void Constructor_Should_Set_Message()
    {
        // Act
        var success =
            new Success("Operation completed.");

        // Assert
        success.Message.Should()
            .Be("Operation completed.");
    }

    [Fact]
    public void Constructor_Should_Initialize_Empty_Metadata()
    {
        // Act
        var success =
            new Success("Operation completed.");

        // Assert
        success.Metadata.Should().NotBeNull();
        success.Metadata.Should().BeEmpty();
    }

    #endregion

    #region IReason

    [Fact]
    public void Success_Should_Implement_IReason()
    {
        // Arrange
        var success =
            new Success("Operation completed.");

        // Act
        IReason reason = success;

        // Assert
        reason.Message.Should()
            .Be("Operation completed.");
    }

    [Fact]
    public void IReason_Metadata_Should_Return_Success_Metadata()
    {
        // Arrange
        Success success = new Success(
            "Operation completed.")
            .WithMetadata(
                "TraceId",
                "ABC-123");

        // Act
        IReason reason = success;

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
    public void None_Should_Have_Empty_Message()
    {
        // Assert
        Success.None.Message.Should().BeEmpty();
    }

    [Fact]
    public void None_Should_Have_Empty_Metadata()
    {
        // Assert
        Success.None.Metadata.Should().BeEmpty();
    }

    [Fact]
    public void None_Should_Expose_Empty_Message_Through_IReason()
    {
        // Arrange
        IReason reason = Success.None;

        // Assert
        reason.Message.Should().BeEmpty();
    }

    #endregion

    #region WithMetadata

    [Fact]
    public void WithMetadata_Should_Add_Metadata()
    {
        // Arrange
        var source =
            new Success("Operation completed.");

        // Act
        Success result = source.WithMetadata(
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
    public void WithMetadata_Should_Return_New_Success_Instance()
    {
        // Arrange
        var source =
            new Success("Operation completed.");

        // Act
        Success result = source.WithMetadata(
            "TraceId",
            "ABC-123");

        // Assert
        result.Should().NotBeSameAs(source);
    }

    [Fact]
    public void WithMetadata_Should_Not_Modify_Original_Success()
    {
        // Arrange
        var source =
            new Success("Operation completed.");

        // Act
        Success result = source.WithMetadata(
            "TraceId",
            "ABC-123");

        // Assert
        source.Metadata.Should().BeEmpty();

        result.Metadata.Should()
            .ContainKey("TraceId");
    }

    [Fact]
    public void WithMetadata_Should_Preserve_Message()
    {
        // Arrange
        var source =
            new Success("Operation completed.");

        // Act
        Success result = source.WithMetadata(
            "TraceId",
            "ABC-123");

        // Assert
        result.Message.Should()
            .Be("Operation completed.");
    }

    [Fact]
    public void WithMetadata_Should_Preserve_Existing_Metadata()
    {
        // Arrange
        Success source = new Success(
            "Operation completed.")
            .WithMetadata(
                "TraceId",
                "ABC-123");

        // Act
        Success result = source.WithMetadata(
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
        Success source = new Success(
            "Operation completed.")
            .WithMetadata(
                "TraceId",
                "ABC-123");

        // Act
        Success result = source.WithMetadata(
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
        Success source = new Success(
            "Operation completed.")
            .WithMetadata(
                "TraceId",
                "ABC-123");

        // Act
        Success result = source.WithMetadata(
            "traceid",
            "XYZ-456");

        // Assert
        result.Metadata.Should().ContainSingle();

        result.Metadata["TRACEID"]
            .Should()
            .Be("XYZ-456");
    }

    [Fact]
    public void WithMetadata_Should_Support_Chaining()
    {
        // Arrange
        var source =
            new Success("Operation completed.");

        // Act
        Success result = source
            .WithMetadata(
                "TraceId",
                "ABC-123")
            .WithMetadata(
                "UserId",
                42)
            .WithMetadata(
                "ElapsedMilliseconds",
                125);

        // Assert
        result.Metadata.Should().HaveCount(3);

        result.Metadata["TraceId"]
            .Should()
            .Be("ABC-123");

        result.Metadata["UserId"]
            .Should()
            .Be(42);

        result.Metadata["ElapsedMilliseconds"]
            .Should()
            .Be(125);
    }

    [Fact]
    public void WithMetadata_Should_Preserve_Previous_Instances()
    {
        // Arrange
        var source =
            new Success("Operation completed.");

        Success first = source.WithMetadata(
            "TraceId",
            "ABC-123");

        // Act
        Success second = first.WithMetadata(
            "UserId",
            42);

        // Assert
        source.Metadata.Should().BeEmpty();

        first.Metadata.Should().ContainSingle();
        first.Metadata.Should().ContainKey("TraceId");
        first.Metadata.Should().NotContainKey("UserId");

        second.Metadata.Should().HaveCount(2);
        second.Metadata.Should().ContainKey("TraceId");
        second.Metadata.Should().ContainKey("UserId");
    }

    [Fact]
    public void WithMetadata_Should_Allow_Null_Value()
    {
        // Arrange
        var source =
            new Success("Operation completed.");

        // Act
        Success result = source.WithMetadata(
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

    #region Record Semantics

    [Fact]
    public void Success_With_Same_Message_Should_Have_Equivalent_Properties()
    {
        // Arrange
        var first =
            new Success("Operation completed.");

        var second =
            new Success("Operation completed.");

        // Assert
        first.Message.Should()
            .Be(second.Message);

        first.Metadata.Should()
            .BeEquivalentTo(second.Metadata);
    }

    [Fact]
    public void Success_With_Different_Message_Should_Not_Be_Equal()
    {
        // Arrange
        var first =
            new Success("First message.");

        var second =
            new Success("Second message.");

        // Assert
        first.Should().NotBe(second);
    }

    [Fact]
    public void With_Expression_Should_Create_New_Instance()
    {
        // Arrange
        var source =
            new Success("Original message.");

        // Act
        Success result = source with
        {
            Message = "Updated message."
        };

        // Assert
        result.Should().NotBeSameAs(source);

        source.Message.Should()
            .Be("Original message.");

        result.Message.Should()
            .Be("Updated message.");
    }

    #endregion

    #region Null Guards

    [Fact]
    public void WithMetadata_Should_Throw_When_Key_Is_Null()
    {
        // Arrange
        var success =
            new Success("Operation completed.");

        string key = null!;

        // Act
        Action act = () =>
            success.WithMetadata(
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
        var success =
            new Success("Operation completed.");

        // Act
        Action act = () =>
            success.WithMetadata(
                string.Empty,
                "value");

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("key");
    }

    #endregion
}