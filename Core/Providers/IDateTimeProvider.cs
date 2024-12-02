using Maestro.Core.Result;

namespace Maestro.Core.Providers;

public interface IDateTimeProvider
{
    DateTime GetCurrentDateTime();
    Result<DateTime> TryParse(string time, string? date = null);
}