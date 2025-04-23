using Microsoft.AspNetCore.Components;
using Web.Services.HttpRepository;

namespace WebAssembly;

public partial class App : IDisposable
{
  [Inject]
  HttpInterceptorService InterceptorService { get; set; } = default!;

  protected override void OnInitialized()
  {
    InterceptorService.RegisterEvent();
  }

  public void Dispose()
  {
    InterceptorService.DisposeEvent();
  }
}
