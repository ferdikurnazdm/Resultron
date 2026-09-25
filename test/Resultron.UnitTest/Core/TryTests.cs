using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Resultron.UnitTest;

public sealed class TryTests
{
    #region Result.Try - Default Error

    [Fact]
    public void Try_Should_Return_Success_When_Action_Completes()
    {
        // Arrange
        Action action =
            Substitute.For<Action>();

        // Act
        Result result =
            Result.Try(action);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();

        action.Received(1).Invoke();
    }

    [Fact]
    public void Try_Should_Return_Failure_When_Action_Throws()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Operation failed.");

        Action action = () =>
            throw exception;

        // Act
        Result result =
            Result.Try(action);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Try_Should_Use_Exception_Type_As_Error_Code()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Operation failed.");

        // Act
        Result result = Result.Try(
            () => throw exception);

        // Assert
        result.Error.Code.Should()
            .Be(nameof(InvalidOperationException));
    }

    [Fact]
    public void Try_Should_Use_Exception_Message_As_Error_Description()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Operation failed.");

        // Act
        Result result = Result.Try(
            () => throw exception);

        // Assert
        result.Error.Description.Should()
            .Be("Operation failed.");
    }

    [Fact]
    public void Try_Should_Preserve_Original_Exception()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Operation failed.");

        // Act
        Result result = Result.Try(
            () => throw exception);

        // Assert
        result.Error.Exception.Should()
            .BeSameAs(exception);
    }

    [Fact]
    public void Try_Should_Contain_Single_Error_When_Action_Throws()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Operation failed.");

        // Act
        Result result = Result.Try(
            () => throw exception);

        // Assert
        result.Errors.Should().ContainSingle();
        result.Reasons.Should().ContainSingle();
    }

    #endregion

    #region Result.Try - Custom Error Handler

    [Fact]
    public void Try_With_ErrorHandler_Should_Return_Success_When_Action_Completes()
    {
        // Arrange
        Action action =
            Substitute.For<Action>();

        Func<Exception, Error> errorHandler =
            Substitute.For<Func<Exception, Error>>();

        // Act
        Result result =
            Result.Try(
                action,
                errorHandler);

        // Assert
        result.IsSuccess.Should().BeTrue();

        action.Received(1).Invoke();

        errorHandler.DidNotReceive()
            .Invoke(Arg.Any<Exception>());
    }

    [Fact]
    public void Try_With_ErrorHandler_Should_Invoke_Handler_When_Action_Throws()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Operation failed.");

        var mappedError =
            new Error(
                "custom.error",
                "Custom error.");

        Func<Exception, Error> errorHandler =
            Substitute.For<Func<Exception, Error>>();

        errorHandler(exception)
            .Returns(mappedError);

        // Act
        Result result = Result.Try(
            () => throw exception,
            errorHandler);

        // Assert
        result.IsFailure.Should().BeTrue();

        errorHandler.Received(1)
            .Invoke(exception);
    }

    [Fact]
    public void Try_With_ErrorHandler_Should_Use_Mapped_Error()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Operation failed.");

        var mappedError =
            new Error(
                "custom.error",
                "Custom error.");

        // Act
        Result result = Result.Try(
            () => throw exception,
            _ => mappedError);

        // Assert
        result.Error.Code.Should()
            .Be("custom.error");

        result.Error.Description.Should()
            .Be("Custom error.");
    }

    [Fact]
    public void Try_With_ErrorHandler_Should_Attach_Original_Exception()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Operation failed.");

        var mappedError =
            new Error(
                "custom.error",
                "Custom error.");

        // Act
        Result result = Result.Try(
            () => throw exception,
            _ => mappedError);

        // Assert
        result.Error.Exception.Should()
            .BeSameAs(exception);
    }

    [Fact]
    public void Try_With_ErrorHandler_Should_Replace_ErrorHandler_Exception_With_Caught_Exception()
    {
        // Arrange
        var caughtException =
            new InvalidOperationException(
                "Caught exception.");

        var existingException =
            new ArgumentException(
                "Existing exception.");

        var mappedError =
            new Error(
                "custom.error",
                "Custom error.",
                existingException);

        // Act
        Result result = Result.Try(
            () => throw caughtException,
            _ => mappedError);

        // Assert
        result.Error.Exception.Should()
            .BeSameAs(caughtException);
    }

    #endregion

    #region Result.TryAsync - Default Error

    [Fact]
    public async Task TryAsync_Should_Return_Success_When_Action_Completes()
    {
        // Arrange
        Func<Task> action =
            Substitute.For<Func<Task>>();

        action()
            .Returns(Task.CompletedTask);

        // Act
        Result result =
            await Result.TryAsync(action);

        // Assert
        result.IsSuccess.Should().BeTrue();

        await action.Received(1).Invoke();
    }

    [Fact]
    public async Task TryAsync_Should_Return_Failure_When_Action_Throws()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Async operation failed.");

        // Act
        Result result = await Result.TryAsync(
            async () =>
            {
                await Task.Yield();

                throw exception;
            });

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Code.Should()
            .Be(nameof(InvalidOperationException));

        result.Error.Description.Should()
            .Be("Async operation failed.");

        result.Error.Exception.Should()
            .BeSameAs(exception);
    }

    [Fact]
    public async Task TryAsync_Should_Await_Action()
    {
        // Arrange
        var completed = false;

        // Act
        Result result = await Result.TryAsync(
            async () =>
            {
                await Task.Yield();

                completed = true;
            });

        // Assert
        completed.Should().BeTrue();
        result.IsSuccess.Should().BeTrue();
    }

    #endregion

    #region Result.TryAsync - Custom Error Handler

    [Fact]
    public async Task TryAsync_With_ErrorHandler_Should_Not_Invoke_Handler_When_Action_Succeeds()
    {
        // Arrange
        Func<Exception, Error> errorHandler =
            Substitute.For<Func<Exception, Error>>();

        // Act
        Result result = await Result.TryAsync(
            () => Task.CompletedTask,
            errorHandler);

        // Assert
        result.IsSuccess.Should().BeTrue();

        errorHandler.DidNotReceive()
            .Invoke(Arg.Any<Exception>());
    }

    [Fact]
    public async Task TryAsync_With_ErrorHandler_Should_Invoke_Handler_When_Action_Throws()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Async operation failed.");

        var mappedError =
            new Error(
                "custom.async.error",
                "Custom async error.");

        Func<Exception, Error> errorHandler =
            Substitute.For<Func<Exception, Error>>();

        errorHandler(exception)
            .Returns(mappedError);

        // Act
        Result result = await Result.TryAsync(
            async () =>
            {
                await Task.Yield();

                throw exception;
            },
            errorHandler);

        // Assert
        result.IsFailure.Should().BeTrue();

        errorHandler.Received(1)
            .Invoke(exception);

        result.Error.Code.Should()
            .Be("custom.async.error");

        result.Error.Exception.Should()
            .BeSameAs(exception);
    }

    #endregion

    #region Result<T>.Try - Default Error

    [Fact]
    public void Generic_Try_Should_Return_Value_When_Function_Succeeds()
    {
        // Act
        Result<int> result =
            Result<int>.Try(() => 42);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Generic_Try_Should_Invoke_Function_Once()
    {
        // Arrange
        Func<int> func =
            Substitute.For<Func<int>>();

        func()
            .Returns(42);

        // Act
        Result<int> result =
            Result<int>.Try(func);

        // Assert
        result.Value.Should().Be(42);

        func.Received(1).Invoke();
    }

    [Fact]
    public void Generic_Try_Should_Return_Failure_When_Function_Throws()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Value operation failed.");

        // Act
        Result<int> result =
            Result<int>.Try(
                () => throw exception);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Code.Should()
            .Be(nameof(InvalidOperationException));

        result.Error.Description.Should()
            .Be("Value operation failed.");

        result.Error.Exception.Should()
            .BeSameAs(exception);
    }

    [Fact]
    public void Generic_Try_Should_Prevent_Value_Access_When_Function_Throws()
    {
        // Arrange
        Result<int> result =
            Result<int>.Try(
                () => throw new InvalidOperationException());

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

    #region Result<T>.Try - Custom Error Handler

    [Fact]
    public void Generic_Try_With_ErrorHandler_Should_Return_Value_When_Function_Succeeds()
    {
        // Arrange
        Func<Exception, Error> errorHandler =
            Substitute.For<Func<Exception, Error>>();

        // Act
        Result<int> result =
            Result<int>.Try(
                () => 42,
                errorHandler);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);

        errorHandler.DidNotReceive()
            .Invoke(Arg.Any<Exception>());
    }

    [Fact]
    public void Generic_Try_With_ErrorHandler_Should_Map_Exception()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Value operation failed.");

        var mappedError =
            new Error(
                "value.error",
                "Custom value error.");

        // Act
        Result<int> result =
            Result<int>.Try(
                () => throw exception,
                _ => mappedError);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Code.Should()
            .Be("value.error");

        result.Error.Description.Should()
            .Be("Custom value error.");

        result.Error.Exception.Should()
            .BeSameAs(exception);
    }

    [Fact]
    public void Generic_Try_With_ErrorHandler_Should_Pass_Caught_Exception_To_Handler()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Value operation failed.");

        Func<Exception, Error> errorHandler =
            Substitute.For<Func<Exception, Error>>();

        errorHandler(exception)
            .Returns(
                new Error(
                    "value.error",
                    "Value error."));

        // Act
        _ = Result<int>.Try(
            () => throw exception,
            errorHandler);

        // Assert
        errorHandler.Received(1)
            .Invoke(exception);
    }

    #endregion

    #region Result<T>.TryAsync - Default Error

    [Fact]
    public async Task Generic_TryAsync_Should_Return_Value_When_Function_Succeeds()
    {
        // Act
        Result<int> result =
            await Result<int>.TryAsync(
                async () =>
                {
                    await Task.Yield();

                    return 42;
                });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task Generic_TryAsync_Should_Return_Failure_When_Function_Throws()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Async value operation failed.");

        // Act
        Result<int> result =
            await Result<int>.TryAsync(
                async () =>
                {
                    await Task.Yield();

                    throw exception;

#pragma warning disable CS0162
                    return 0;
#pragma warning restore CS0162
                });

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Code.Should()
            .Be(nameof(InvalidOperationException));

        result.Error.Description.Should()
            .Be("Async value operation failed.");

        result.Error.Exception.Should()
            .BeSameAs(exception);
    }

    [Fact]
    public async Task Generic_TryAsync_Should_Await_Function()
    {
        // Arrange
        var completed = false;

        // Act
        Result<int> result =
            await Result<int>.TryAsync(
                async () =>
                {
                    await Task.Yield();

                    completed = true;

                    return 42;
                });

        // Assert
        completed.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    #endregion

    #region Result<T>.TryAsync - Custom Error Handler

    [Fact]
    public async Task Generic_TryAsync_With_ErrorHandler_Should_Return_Value_When_Function_Succeeds()
    {
        // Arrange
        Func<Exception, Error> errorHandler =
            Substitute.For<Func<Exception, Error>>();

        // Act
        Result<int> result =
            await Result<int>.TryAsync(
                () => Task.FromResult(42),
                errorHandler);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);

        errorHandler.DidNotReceive()
            .Invoke(Arg.Any<Exception>());
    }

    [Fact]
    public async Task Generic_TryAsync_With_ErrorHandler_Should_Map_Exception()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Async value operation failed.");

        var mappedError =
            new Error(
                "async.value.error",
                "Custom async value error.");

        // Act
        Result<int> result =
            await Result<int>.TryAsync(
                () => Task.FromException<int>(
                    exception),
                _ => mappedError);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Code.Should()
            .Be("async.value.error");

        result.Error.Description.Should()
            .Be("Custom async value error.");

        result.Error.Exception.Should()
            .BeSameAs(exception);
    }

    [Fact]
    public async Task Generic_TryAsync_With_ErrorHandler_Should_Pass_Exception_To_Handler()
    {
        // Arrange
        var exception =
            new InvalidOperationException(
                "Async value operation failed.");

        Func<Exception, Error> errorHandler =
            Substitute.For<Func<Exception, Error>>();

        errorHandler(exception)
            .Returns(
                new Error(
                    "async.value.error",
                    "Async value error."));

        // Act
        _ = await Result<int>.TryAsync(
            () => Task.FromException<int>(
                exception),
            errorHandler);

        // Assert
        errorHandler.Received(1)
            .Invoke(exception);
    }

    #endregion

    #region Null Guards - Result

    [Fact]
    public void Try_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Action action = null!;

        // Act
        Action act = () =>
            Result.Try(action);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public void Try_With_ErrorHandler_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Action action = null!;

        // Act
        Action act = () =>
            Result.Try(
                action,
                _ => new Error(
                    "test.error",
                    "Test error."));

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public void Try_With_ErrorHandler_Should_Throw_When_ErrorHandler_Is_Null()
    {
        // Arrange
        Func<Exception, Error> errorHandler =
            null!;

        // Act
        Action act = () =>
            Result.Try(
                () => { },
                errorHandler);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("errorHandler");
    }

    [Fact]
    public async Task TryAsync_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Func<Task> action = null!;

        // Act
        Func<Task> act = async () =>
            await Result.TryAsync(action);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public async Task TryAsync_With_ErrorHandler_Should_Throw_When_ErrorHandler_Is_Null()
    {
        // Arrange
        Func<Exception, Error> errorHandler =
            null!;

        // Act
        Func<Task> act = async () =>
            await Result.TryAsync(
                () => Task.CompletedTask,
                errorHandler);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("errorHandler");
    }

    #endregion

    #region Null Guards - Result<T>

    [Fact]
    public void Generic_Try_Should_Throw_When_Function_Is_Null()
    {
        // Arrange
        Func<int> func = null!;

        // Act
        Action act = () =>
            Result<int>.Try(func);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("func");
    }

    [Fact]
    public void Generic_Try_With_ErrorHandler_Should_Throw_When_Function_Is_Null()
    {
        // Arrange
        Func<int> func = null!;

        Func<Exception, Error> errorHandler =
            _ => new Error(
                "test.error",
                "Test error.");

        // Act
        Action act = () =>
            Result<int>.Try(
                func,
                errorHandler);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("func");
    }

    [Fact]
    public void Generic_Try_With_ErrorHandler_Should_Throw_When_ErrorHandler_Is_Null()
    {
        // Arrange
        Func<Exception, Error> errorHandler =
            null!;

        // Act
        Action act = () =>
            Result<int>.Try(
                () => 42,
                errorHandler);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("errorHandler");
    }

    [Fact]
    public async Task Generic_TryAsync_Should_Throw_When_Function_Is_Null()
    {
        // Arrange
        Func<Task<int>> func = null!;

        // Act
        Func<Task> act = async () =>
            await Result<int>.TryAsync(func);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("func");
    }

    [Fact]
    public async Task Generic_TryAsync_With_ErrorHandler_Should_Throw_When_Function_Is_Null()
    {
        // Arrange
        Func<Task<int>> func = null!;

        Func<Exception, Error> errorHandler =
            _ => new Error(
                "test.error",
                "Test error.");

        // Act
        Func<Task> act = async () =>
            await Result<int>.TryAsync(
                func,
                errorHandler);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("func");
    }

    [Fact]
    public async Task Generic_TryAsync_With_ErrorHandler_Should_Throw_When_ErrorHandler_Is_Null()
    {
        // Arrange
        Func<Exception, Error> errorHandler =
            null!;

        // Act
        Func<Task> act = async () =>
            await Result<int>.TryAsync(
                () => Task.FromResult(42),
                errorHandler);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("errorHandler");
    }

    #endregion
}