using System.Net.Http.Headers;
using Toolbelt.Blazor;

namespace Web.Services.HttpRepository;

public class HttpInterceptorService
{
  private readonly HttpClientInterceptor _interceptor;
  private readonly RefreshTokenHttpService _refreshTokenService;
  private readonly SemaphoreSlim _refreshTokenLock = new(1, 1);

  public HttpInterceptorService(HttpClientInterceptor interceptor, RefreshTokenHttpService refreshTokenService)
  {
    _interceptor = interceptor ?? throw new ArgumentNullException(nameof(interceptor));
    _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
  }

  public void RegisterEvent() => _interceptor.BeforeSendAsync += InsertNewRefreshTokenOnRequestHeader;

  public async Task InsertNewRefreshTokenOnRequestHeader(object sender, HttpClientInterceptorEventArgs e)
  {
    //IMPORTANT : How refresh token work.
    //1. We must include the JWT token bearer every time we make a call to the web API service; The token should be active, it means we can't use an expired token.
    //2. TryRefreshToken() is checking whether the token is expired or not (We know it because we save the expiry time in the JWT token)
    //3. When the token is nearly expired, then extend the expiration time (In this method, we extend token expiry time when the expiration time remaining is < 5 minutes).

    var absPath = e.Request.RequestUri?.AbsolutePath;

    if (absPath is null || absPath.Contains("authentication")) return;

    await _refreshTokenLock.WaitAsync(); // Wait to enter the semaphore
    try
    {
      var accessToken = await _refreshTokenService.TryRefreshToken();
      if (!string.IsNullOrEmpty(accessToken))
      {
        e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
      }
    }
    finally
    {
      _refreshTokenLock.Release(); // Release the semaphore
    }
  }

  public void DisposeEvent() => _interceptor.BeforeSendAsync -= InsertNewRefreshTokenOnRequestHeader;
}
