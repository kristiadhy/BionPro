using Microsoft.AspNetCore.Components;
using Web.Services.IHttpRepository;

namespace WebAssembly;

public partial class App : IDisposable
{
    [Inject]
    IServiceManager ServiceManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        ServiceManager.InterceptorService.RegisterEvent();
    }

    public void Dispose()
    {
        ServiceManager.InterceptorService.DisposeEvent();
    }
}
