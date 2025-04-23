using Radzen;

namespace WebAssembly.Services;

public class CustomNotificationService
{
  private readonly NotificationService _notificationService;

  public CustomNotificationService(NotificationService notificationService)
  {
    _notificationService = notificationService;
  }

  public void SaveNotification(string detailText)
  {
    _notificationService.Notify(
       new NotificationMessage
       {
         Severity = NotificationSeverity.Info,
         Duration = 3000,
         Summary = "Saved Successfully",
         Detail = detailText
       });
  }

  public void DeleteNotification(string detailText)
  {
    _notificationService.Notify(
       new NotificationMessage
       {
         Severity = NotificationSeverity.Success,
         Duration = 3000,
         Summary = "Deleted Successfully",
         Detail = detailText
       });
  }
}
