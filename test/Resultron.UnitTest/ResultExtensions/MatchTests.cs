using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Resultron.UnitTest;

public sealed class MatchTests
{
    private static readonly Error TestError =
        new("test.error", "Test error.");

    #region Result Actions

    [Fact]
    public void Match_Result_Should_Invoke_OnSuccess_When_Result_Is_Success()
    {
        // Arrange
        Result result = Result.Success();

        Action onSuccess =
            Substitute.For<Action>();

        Action<Error> onFailure =
            Substitute.For<Action<Error>>();

        // Act
        result.Match(
            onSuccess,
            onFailure);

        // Assert
        onSuccess.Received(1).Invoke();

        onFailure.DidNotReceive()
            .Invoke(Arg.Any<Error>());
    }

    [Fact]
    public void Match_Result_Should_Invoke_OnFailure_When_Result_Is_Failure()
    {
        // Arrange
        Result result =
            Result.Failure(TestError);

        Action onSuccess =
            Substitute.For<Action>();

        Action<Error> onFailure =
            Substitute.For<Action<Error>>();

        // Act
        result.Match(
            onSuccess,
            onFailure);

        // Assert
        onSuccess.DidNotReceive().Invoke();

        onFailure.Received(1)
            .Invoke(TestError);
    }

    #endregion

    #region Result<T> Actions

    [Fact]
    public void Match_Generic_Should_Pass_Value_To_OnSuccess()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Action<int> onSuccess =
            Substitute.For<Action<int>>();

        Action<Error> onFailure =
            Substitute.For<Action<Error>>();

        // Act
        result.Match(
            onSuccess,
            onFailure);

        // Assert
        onSuccess.Received(1)
            .Invoke(42);

        onFailure.DidNotReceive()
            .Invoke(Arg.Any<Error>());
    }

    [Fact]
    public void Match_Generic_Should_Pass_Error_To_OnFailure()
    {
        // Arrange
        Result<int> result =
            Result<int>.Failure(TestError);

        Action<int> onSuccess =
            Substitute.For<Action<int>>();

        Action<Error> onFailure =
            Substitute.For<Action<Error>>();

        // Act
        result.Match(
            onSuccess,
            onFailure);

        // Assert
        onSuccess.DidNotReceive()
            .Invoke(Arg.Any<int>());

        onFailure.Received(1)
            .Invoke(TestError);
    }

    #endregion

    #region Result Functions

    [Fact]
    public void Match_Result_Should_Return_OnSuccess_Value_When_Successful()
    {
        // Arrange
        Result result = Result.Success();

        // Act
        string response = result.Match(
            onSuccess: () => "success",
            onFailure: error => error.Description);

        // Assert
        response.Should().Be("success");
    }

    [Fact]
    public void Match_Result_Should_Return_OnFailure_Value_When_Failed()
    {
        // Arrange
        Result result =
            Result.Failure(TestError);

        // Act
        string response = result.Match(
            onSuccess: () => "success",
            onFailure: error => error.Description);

        // Assert
        response.Should().Be(TestError.Description);
    }

    [Fact]
    public void Match_Result_Should_Not_Invoke_Unselected_Function()
    {
        // Arrange
        Result result = Result.Success();

        Func<string> onSuccess =
            Substitute.For<Func<string>>();

        Func<Error, string> onFailure =
            Substitute.For<Func<Error, string>>();

        onSuccess()
            .Returns("success");

        // Act
        string response = result.Match(
            onSuccess,
            onFailure);

        // Assert
        response.Should().Be("success");

        onSuccess.Received(1).Invoke();

        onFailure.DidNotReceive()
            .Invoke(Arg.Any<Error>());
    }

    #endregion

    #region Result<T> Functions

    [Fact]
    public void Match_Generic_Should_Return_Value_From_OnSuccess()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        // Act
        string response = result.Match(
            onSuccess: value => $"Value: {value}",
            onFailure: error => error.Description);

        // Assert
        response.Should().Be("Value: 42");
    }

    [Fact]
    public void Match_Generic_Should_Return_Value_From_OnFailure()
    {
        // Arrange
        Result<int> result =
            Result<int>.Failure(TestError);

        // Act
        string response = result.Match(
            onSuccess: value => $"Value: {value}",
            onFailure: error => error.Description);

        // Assert
        response.Should().Be(TestError.Description);
    }

    [Fact]
    public void Match_Generic_Should_Invoke_Only_Selected_Function()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Func<int, string> onSuccess =
            Substitute.For<Func<int, string>>();

        Func<Error, string> onFailure =
            Substitute.For<Func<Error, string>>();

        onSuccess(42)
            .Returns("mapped");

        // Act
        string response = result.Match(
            onSuccess,
            onFailure);

        // Assert
        response.Should().Be("mapped");

        onSuccess.Received(1)
            .Invoke(42);

        onFailure.DidNotReceive()
            .Invoke(Arg.Any<Error>());
    }

    #endregion

    #region Result Async Actions

    [Fact]
    public async Task MatchAsync_Result_Should_Invoke_OnSuccess_When_Successful()
    {
        // Arrange
        Result result = Result.Success();

        Func<Task> onSuccess =
            Substitute.For<Func<Task>>();

        Func<Error, Task> onFailure =
            Substitute.For<Func<Error, Task>>();

        onSuccess()
            .Returns(Task.CompletedTask);

        // Act
        await result.MatchAsync(
            onSuccess,
            onFailure);

        // Assert
        await onSuccess.Received(1).Invoke();

        await onFailure.DidNotReceive()
            .Invoke(Arg.Any<Error>());
    }

    [Fact]
    public async Task MatchAsync_Result_Should_Invoke_OnFailure_When_Failed()
    {
        // Arrange
        Result result =
            Result.Failure(TestError);

        Func<Task> onSuccess =
            Substitute.For<Func<Task>>();

        Func<Error, Task> onFailure =
            Substitute.For<Func<Error, Task>>();

        onFailure(TestError)
            .Returns(Task.CompletedTask);

        // Act
        await result.MatchAsync(
            onSuccess,
            onFailure);

        // Assert
        await onSuccess.DidNotReceive().Invoke();

        await onFailure.Received(1)
            .Invoke(TestError);
    }

    #endregion

    #region Result<T> Async Actions

    [Fact]
    public async Task MatchAsync_Generic_Should_Pass_Value_To_OnSuccess()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Func<int, Task> onSuccess =
            Substitute.For<Func<int, Task>>();

        Func<Error, Task> onFailure =
            Substitute.For<Func<Error, Task>>();

        onSuccess(42)
            .Returns(Task.CompletedTask);

        // Act
        await result.MatchAsync(
            onSuccess,
            onFailure);

        // Assert
        await onSuccess.Received(1)
            .Invoke(42);

        await onFailure.DidNotReceive()
            .Invoke(Arg.Any<Error>());
    }

    [Fact]
    public async Task MatchAsync_Generic_Should_Pass_Error_To_OnFailure()
    {
        // Arrange
        Result<int> result =
            Result<int>.Failure(TestError);

        Func<int, Task> onSuccess =
            Substitute.For<Func<int, Task>>();

        Func<Error, Task> onFailure =
            Substitute.For<Func<Error, Task>>();

        onFailure(TestError)
            .Returns(Task.CompletedTask);

        // Act
        await result.MatchAsync(
            onSuccess,
            onFailure);

        // Assert
        await onSuccess.DidNotReceive()
            .Invoke(Arg.Any<int>());

        await onFailure.Received(1)
            .Invoke(TestError);
    }

    #endregion

    #region Result Async Functions

    [Fact]
    public async Task MatchAsync_Result_Should_Return_OnSuccess_Value()
    {
        // Arrange
        Result result = Result.Success();

        // Act
        string response = await result.MatchAsync(
            onSuccess: () =>
                Task.FromResult("success"),
            onFailure: error =>
                Task.FromResult(error.Description));

        // Assert
        response.Should().Be("success");
    }

    [Fact]
    public async Task MatchAsync_Result_Should_Return_OnFailure_Value()
    {
        // Arrange
        Result result =
            Result.Failure(TestError);

        // Act
        string response = await result.MatchAsync(
            onSuccess: () =>
                Task.FromResult("success"),
            onFailure: error =>
                Task.FromResult(error.Description));

        // Assert
        response.Should().Be(TestError.Description);
    }

    #endregion

    #region Result<T> Async Functions

    [Fact]
    public async Task MatchAsync_Generic_Should_Return_OnSuccess_Value()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        // Act
        string response = await result.MatchAsync(
            onSuccess: value =>
                Task.FromResult($"Value: {value}"),
            onFailure: error =>
                Task.FromResult(error.Description));

        // Assert
        response.Should().Be("Value: 42");
    }

    [Fact]
    public async Task MatchAsync_Generic_Should_Return_OnFailure_Value()
    {
        // Arrange
        Result<int> result =
            Result<int>.Failure(TestError);

        // Act
        string response = await result.MatchAsync(
            onSuccess: value =>
                Task.FromResult($"Value: {value}"),
            onFailure: error =>
                Task.FromResult(error.Description));

        // Assert
        response.Should().Be(TestError.Description);
    }

    #endregion

    #region Task<Result> Actions

    [Fact]
    public async Task MatchAsync_Task_Result_Should_Invoke_Synchronous_OnSuccess()
    {
        // Arrange
        Task<Result> resultTask =
            Task.FromResult(Result.Success());

        Action onSuccess =
            Substitute.For<Action>();

        Action<Error> onFailure =
            Substitute.For<Action<Error>>();

        // Act
        await resultTask.MatchAsync(
            onSuccess,
            onFailure);

        // Assert
        onSuccess.Received(1).Invoke();

        onFailure.DidNotReceive()
            .Invoke(Arg.Any<Error>());
    }

    [Fact]
    public async Task MatchAsync_Task_Result_Should_Invoke_Synchronous_OnFailure()
    {
        // Arrange
        Task<Result> resultTask =
            Task.FromResult(
                Result.Failure(TestError));

        Action onSuccess =
            Substitute.For<Action>();

        Action<Error> onFailure =
            Substitute.For<Action<Error>>();

        // Act
        await resultTask.MatchAsync(
            onSuccess,
            onFailure);

        // Assert
        onSuccess.DidNotReceive().Invoke();

        onFailure.Received(1)
            .Invoke(TestError);
    }

    [Fact]
    public async Task MatchAsync_Task_Result_Should_Invoke_Asynchronous_OnSuccess()
    {
        // Arrange
        Task<Result> resultTask =
            Task.FromResult(Result.Success());

        Func<Task> onSuccess =
            Substitute.For<Func<Task>>();

        Func<Error, Task> onFailure =
            Substitute.For<Func<Error, Task>>();

        onSuccess()
            .Returns(Task.CompletedTask);

        // Act
        await resultTask.MatchAsync(
            onSuccess,
            onFailure);

        // Assert
        await onSuccess.Received(1).Invoke();

        await onFailure.DidNotReceive()
            .Invoke(Arg.Any<Error>());
    }

    #endregion

    #region Task<Result<T>> Actions

    [Fact]
    public async Task MatchAsync_Task_Generic_Result_Should_Pass_Value_To_Synchronous_OnSuccess()
    {
        // Arrange
        Task<Result<int>> resultTask =
            Task.FromResult(
                Result<int>.Success(42));

        Action<int> onSuccess =
            Substitute.For<Action<int>>();

        Action<Error> onFailure =
            Substitute.For<Action<Error>>();

        // Act
        await resultTask.MatchAsync(
            onSuccess,
            onFailure);

        // Assert
        onSuccess.Received(1)
            .Invoke(42);

        onFailure.DidNotReceive()
            .Invoke(Arg.Any<Error>());
    }

    [Fact]
    public async Task MatchAsync_Task_Generic_Result_Should_Pass_Value_To_Asynchronous_OnSuccess()
    {
        // Arrange
        Task<Result<int>> resultTask =
            Task.FromResult(
                Result<int>.Success(42));

        Func<int, Task> onSuccess =
            Substitute.For<Func<int, Task>>();

        Func<Error, Task> onFailure =
            Substitute.For<Func<Error, Task>>();

        onSuccess(42)
            .Returns(Task.CompletedTask);

        // Act
        await resultTask.MatchAsync(
            onSuccess,
            onFailure);

        // Assert
        await onSuccess.Received(1)
            .Invoke(42);

        await onFailure.DidNotReceive()
            .Invoke(Arg.Any<Error>());
    }

    #endregion

    #region Task<Result> Functions

    [Fact]
    public async Task MatchAsync_Task_Result_Should_Return_Value_From_Synchronous_Handler()
    {
        // Arrange
        Task<Result> resultTask =
            Task.FromResult(Result.Success());

        // Act
        string response = await resultTask.MatchAsync(
            onSuccess: () => "success",
            onFailure: error => error.Description);

        // Assert
        response.Should().Be("success");
    }

    [Fact]
    public async Task MatchAsync_Task_Result_Should_Return_Value_From_Asynchronous_Handler()
    {
        // Arrange
        Task<Result> resultTask =
            Task.FromResult(Result.Success());

        // Act
        string response = await resultTask.MatchAsync(
            onSuccess: () =>
                Task.FromResult("success"),
            onFailure: error =>
                Task.FromResult(error.Description));

        // Assert
        response.Should().Be("success");
    }

    #endregion

    #region Task<Result<T>> Functions

    [Fact]
    public async Task MatchAsync_Task_Generic_Result_Should_Return_Value_From_Synchronous_Handler()
    {
        // Arrange
        Task<Result<int>> resultTask =
            Task.FromResult(
                Result<int>.Success(42));

        // Act
        string response = await resultTask.MatchAsync(
            onSuccess: value => $"Value: {value}",
            onFailure: error => error.Description);

        // Assert
        response.Should().Be("Value: 42");
    }

    [Fact]
    public async Task MatchAsync_Task_Generic_Result_Should_Return_Value_From_Asynchronous_Handler()
    {
        // Arrange
        Task<Result<int>> resultTask =
            Task.FromResult(
                Result<int>.Success(42));

        // Act
        string response = await resultTask.MatchAsync(
            onSuccess: value =>
                Task.FromResult($"Value: {value}"),
            onFailure: error =>
                Task.FromResult(error.Description));

        // Assert
        response.Should().Be("Value: 42");
    }

    #endregion

    #region Null Guards

    [Fact]
    public void Match_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result result = null!;

        // Act
        Action act = () => result.Match(
            () => { },
            _ => { });

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Match_Should_Throw_When_OnSuccess_Is_Null()
    {
        // Arrange
        Result result = Result.Success();
        Action onSuccess = null!;

        // Act
        Action act = () => result.Match(
            onSuccess,
            _ => { });

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("onSuccess");
    }

    [Fact]
    public void Match_Should_Throw_When_OnFailure_Is_Null()
    {
        // Arrange
        Result result = Result.Success();
        Action<Error> onFailure = null!;

        // Act
        Action act = () => result.Match(
            () => { },
            onFailure);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("onFailure");
    }

    [Fact]
    public void Match_Generic_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Action act = () => result.Match(
            _ => { },
            _ => { });

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public async Task MatchAsync_Should_Throw_When_ResultTask_Is_Null()
    {
        // Arrange
        Task<Result> resultTask = null!;

        Action onSuccess = () => { };
        Action<Error> onFailure = _ => { };

        // Act
        Func<Task> act = () =>
            resultTask.MatchAsync(
                onSuccess,
                onFailure);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    [Fact]
    public async Task MatchAsync_Generic_Should_Throw_When_ResultTask_Is_Null()
    {
        // Arrange
        Task<Result<int>> resultTask = null!;

        Action<int> onSuccess = _ => { };
        Action<Error> onFailure = _ => { };

        // Act
        Func<Task> act = () =>
            resultTask.MatchAsync(
                onSuccess,
                onFailure);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    #endregion
}