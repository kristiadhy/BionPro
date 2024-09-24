using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Services.Extensions;
using Web.Services.Features;
using Web.Services.IHttpRepository;

namespace Web.Services.HttpRepository;
internal class PurchaseHttpService : IPurchaseHttpService
{
    private readonly CustomHttpClient _client;
    private readonly string additionalResourceName = "purchases";
    private readonly JsonSerializerSettings _options;

    public PurchaseHttpService(CustomHttpClient client, JsonSerializerSettings options)
    {
        _client = client;
        _options = options;
    }

    public async Task<DataResponse<PurchaseDtoForSummary>> GetPurchasesForSummary(PurchaseParam purchaseParam)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            [$"{nameof(PurchaseParam.PageNumber)}"] = purchaseParam.PageNumber.ToString(),
            [$"{nameof(PurchaseParam.SrcSupplierID)}"] = purchaseParam.SrcSupplierID.ToString() ?? "",
            [$"{nameof(PurchaseParam.SrcSupplierName)}"] = purchaseParam.SrcSupplierName ?? "",
            [$"{nameof(PurchaseParam.SrcDateFrom)}"] = purchaseParam.SrcDateFrom.ToString() ?? "",
            [$"{nameof(PurchaseParam.SrcDateTo)}"] = purchaseParam.SrcDateTo.ToString() ?? "",
            [$"{nameof(PurchaseParam.OrderBy)}"] = purchaseParam.OrderBy ?? ""
        };

        var queryHelper = QueryHelpers.AddQueryString(additionalResourceName, queryStringParam!);
        HttpResponseMessage response = await _client.GetResponseAsync(queryHelper);
        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new DataResponse<PurchaseDtoForSummary>()
        {
            Items = JsonConvert.DeserializeObject<List<PurchaseDtoForSummary>>(content, _options)!,
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First())!
        };

        return pagingResponse;
    }

    public async Task<PurchaseDto> GetPurchaseByID(int purchaseID)
    {
        var response = await _client.GetResponseAsync($"{additionalResourceName}/{purchaseID}");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PurchaseDto>(content);
        if (result is not null)
            return result;
        else
            return new();
    }

    public async Task Create(PurchaseDto purchaseDto)
    {
        var response = await _client.PostAsync(additionalResourceName, purchaseDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content);
    }

    public async Task Update(PurchaseDto purchaseDto)
    {
        var response = await _client.PutAsync(additionalResourceName, purchaseDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content);
    }

    public async Task Delete(int purchaseID)
    {
        var response = await _client.DeleteAsync($"{additionalResourceName}/{purchaseID}");
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content);
    }
}
