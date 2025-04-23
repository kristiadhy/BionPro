using MimeKit;

namespace EmailService;
public class MessageModel
{
  public List<MailboxAddress> To { get; set; }
  public string Subject { get; set; }
  public string Content { get; set; }

  public MessageModel(IEnumerable<string> to, string subject, string content)
  {
    //To = new List<MailboxAddress>();
    //To.AddRange(to.Select(x => new MailboxAddress("email", x)));
    To = [.. to.Select(x => new MailboxAddress(x, x))];
    Subject = subject;
    Content = content;
  }
}