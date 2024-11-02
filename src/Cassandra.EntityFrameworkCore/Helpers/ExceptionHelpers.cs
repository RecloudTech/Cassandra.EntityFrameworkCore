using System.Runtime.CompilerServices;

namespace Cassandra.EntityFrameworkCore.Helpers;

internal static class ExceptionHelpers
{
    public static void ThrowArgumentExceptionIfNullOrEmpty(
        this string? argument,
        [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (string.IsNullOrEmpty(argument))
        {
            throw argument is null
                ? new ArgumentNullException(paramName)
                : new ArgumentException("The value cannot be an empty string.", paramName);
        }
    }
}