using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Resultron;

internal static class ResultLogger
{
    [Conditional("DEBUG")]
    public static void Debug(
            string message,
            string? code = null,
            string? description = null,
            [CallerMemberName] string? member = null,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int line = 0)
    {
        WriteLog(
            message: message,
            level: "DEBUG",
            errorCode: code,
            errorDescription: description,
            member: member,
            file: file,
            line: line);
    }

    [Conditional("DEBUG")]
    public static void Success(
        string message,
        [CallerMemberName] string? member = null,
        [CallerFilePath] string? file = null,
        [CallerLineNumber] int line = 0)
    {
        WriteLog(
            message: message,
            level: "SUCCESS",
            errorCode: null,
            errorDescription: null,
            member: member,
            file: file,
            line: line);
    }

    [Conditional("DEBUG")]
    public static void Failure(
    string message,
    string code,
    string description,
    [CallerMemberName] string? member = null,
    [CallerFilePath] string? file = null,
    [CallerLineNumber] int line = 0)
    {
        WriteLog(
            message: message,
            level: "FAILURE",
            errorCode: code,
            errorDescription: description,
            member: member,
            file: file,
            line: line);
    }

    [Conditional("DEBUG")]
    public static void Information(
        string message,
        [CallerMemberName] string? member = null,
        [CallerFilePath] string? file = null,
        [CallerLineNumber] int line = 0)
    {
        WriteLog(
            message: message,
            level: "INFORMATION",
            errorCode: null,
            errorDescription: null,
            member: member,
            file: file,
            line: line);
    }







    private static void WriteLog(
        string message,
        string level,
        string? errorCode,
        string? errorDescription,
        string? member,
        string? file,
        int line)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

        var properties = new List<string>
        {
            $"Member={member}",
            $"File={Path.GetFileName(file)}",
            $"Line={line}"
        };

        if (errorCode is not null)
            properties.Add($"ErrorCode={errorCode}");

        if (errorDescription is not null)
            properties.Add($"ErrorDescription=\"{errorDescription}\"");

        var logMessage = $"{timestamp} [{level}] Resultron {message} {string.Join(" ", properties)}";

        System.Diagnostics.Debug.WriteLine(logMessage);
        Console.WriteLine(logMessage);
    }
}