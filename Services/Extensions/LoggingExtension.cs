using Serilog;

namespace Services.Extensions;
public static class LoggingExtension
{
    public static void LogInformationWithCustomer(this ILogger logger, string action, string customerName, Guid? customerId = null)
    {
        var customerIdPart = customerId.HasValue ? $" with ID : {customerId}" : string.Empty;
        logger.Information($"{action} customer{customerIdPart} : {customerName}");
    }

    public static void LogInformationAction(this ILogger logger, string action, string entityName)
    {
        logger.Information($"{action} {entityName}");
    }
}
