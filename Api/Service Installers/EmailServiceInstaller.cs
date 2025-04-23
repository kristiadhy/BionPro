using Api.ServiceInstallers;
using EmailService;

namespace Api.Service_Installers;

public class EmailServiceInstaller : IServiceInstallers
{
  public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostBuilder host)
  {
    var emailConfig = configuration
     .GetSection("EmailConfiguration")
     .Get<EmailConfiguration>();

    if (emailConfig is not null)
      services.AddSingleton(emailConfig);

    services.AddScoped<IEmailSender, EmailSender>();
  }
}
