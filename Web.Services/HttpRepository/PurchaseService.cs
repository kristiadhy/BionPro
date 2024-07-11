using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Services.Extensions;
using Web.Services.Features;
using Web.Services.IHttpRepository;

namespace Web.Services.HttpRepository;
internal class PurchaseService : IPurchaseService
{
    private readonly CustomHttpClient _client;
    private readonly JsonSerializerSettings _options;
    private readonly string additionalResourceName = "purchases";

    public PurchaseService(CustomHttpClient client, JsonSerializerSettings options)
    {
        _client = client;
        _options = options;
    }

    public async Task<PagingResponse<PurchaseDto>> GetPurchases(PurchaseParam purchaseParam)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageNumber"] = purchaseParam.PageNumber.ToString(),
            ["searchTerm"] = purchaseParam.SrcSupplier ?? "",
            ["orderBy"] = purchaseParam.OrderBy!
        };

        var queryHelper = QueryHelpers.AddQueryString(additionalResourceName, queryStringParam!);
        HttpResponseMessage response = await _client.GetResponseAsync(queryHelper);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseForGetMethod(response);

        var pagingResponse = new PagingResponse<PurchaseDto>()
        {
            Items = JsonConvert.DeserializeObject<List<PurchaseDto>>(content, _options)!,
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First(), _options)!
        };

        return pagingResponse;
    }

    public async Task<PurchaseDto> GetPurchaseByID(int purchaseID)
    {
        var content = await _client.GetResponseAndContentAsync($"{additionalResourceName}/{purchaseID}");
        var result = JsonConvert.DeserializeObject<PurchaseDto>(content, _options);
        if (!string.IsNullOrEmpty(content) && result is not null)
            return result;
        else
            return new();
    }

    public async Task<HttpResponseMessage> Create(PurchaseDto purchaseDto)
    {
        var response = await _client.PostAsync(additionalResourceName, purchaseDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }

    public async Task<HttpResponseMessage> Update(PurchaseDto purchaseDto)
    {
        var response = await _client.PutAsync(additionalResourceName, purchaseDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }

    public async Task<HttpResponseMessage> Delete(int purchaseID)
    {
        var response = await _client.DeleteAsync($"{additionalResourceName}/{purchaseID}");
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }
}
