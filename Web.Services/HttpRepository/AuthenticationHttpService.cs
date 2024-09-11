using Blazored.LocalStorage;
using Domain.DTO;
using Domain.Parameters;
using Extension.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Services.Extensions;
using Web.Services.IHttpRepository;

namespace Web.Services.HttpRepository;

public class AuthenticationHttpService : IAuthenticationHttpService
{
    private readonly CustomHttpClient _client;
    private readonly JsonSerializerSettings _options;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly string additionalResourceName = "authentication";

    public AuthenticationHttpService(CustomHttpClient client, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage, JsonSerializerSettings options)
    {
        _client = client;
        _options = options;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }

    public async Task<string> GetCurrentTokenFromLocalStorage()
    {
        var accessToken = await _localStorage.GetItemAsync<string>("authToken");
        if (!string.IsNullOrEmpty(accessToken))
            return accessToken;
        else
            return string.Empty;
    }

    public async Task<ApiResponseDto<List<string>>?> RegisterUser(UserRegistrationDTO userForRegistration)
    {
        var response = await _client.PostAsync($"{additionalResourceName}/registration", userForRegistration);
        var content = await response.Content.ReadAsStringAsync();

        // We don't catch the exception error here because we want to display it differently since we are not in the main application yet. In the main application, we handle the exception error using ErrorContent. In this form, we handle the exception error manually and display it in an alert.
        var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<List<string>>>(content, _options);
        return apiResponse;
    }

    public async Task Login(UserAuthenticationDTO userForAuthentication)
    {
        var response = await _client.PostAsync($"{additionalResourceName}/login", userForAuthentication);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);

        var apiResponse = JsonConvert.DeserializeObject<ApiResponseDto<TokenDTO>>(content, _options);
        if (apiResponse?.IsSuccess == true)
        {
            await _localStorage.SetItemAsync("authToken", apiResponse!.Data!.AccessToken);
            await _localStorage.SetItemAsync("refreshToken", apiResponse!.Data!.RefreshToken);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(apiResponse!.Data!.AccessToken);
        }
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("refreshToken");
        ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
        _client.RemoveAuthorizationHeader();
    }

    public async Task<string> RefreshToken()
    {
        var accessToken = await _localStorage.GetItemAsync<string>("authToken");
        var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            return string.Empty;

        var tokenDto = new TokenDTO(accessToken, refreshToken);

        var response = await _client.PostAsync($"{additionalResourceName}/refresh", tokenDto);
        var content = await response.Content.ReadAsStringAsync();

        _client.CheckErrorResponseWithContent(response, content, _options);
        if (content is null)
            throw new ApplicationException("Something wrong with the refresh token API response");

        var result = JsonConvert.DeserializeObject<TokenDTO>(content, _options);

        await _localStorage.SetItemAsync("authToken", result!.AccessToken);
        await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

        return result.AccessToken;
    }

    public async Task ConfirmEmail(string email, string token)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            [$"{nameof(email)}"] = email,
            [$"{nameof(token)}"] = token
        };
        string uri = $"{additionalResourceName}/emailconfirmation";
        var queryHelper = QueryHelpers.AddQueryString(uri, queryStringParam!);
        HttpResponseMessage response = await _client.GetResponseAsync(queryHelper);
    }
}

