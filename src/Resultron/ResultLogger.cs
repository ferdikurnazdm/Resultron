using System;
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
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

        var properties = new List<string>
        {
            $"Member={member}",
            $"File={Path.GetFileName(file)}",
            $"Line={line}"
        };

        if (code is not null)
            properties.Add($"Code={code}");

        if (description is not null)
            properties.Add($"Description=\"{description}\"");

        System.Diagnostics.Debug.WriteLine(
            $"{timestamp} [DBG] Resultron {message} {string.Join(" ", properties)}");
    }
}