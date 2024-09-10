using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Services.Extensions;
using Web.Services.Features;
using Web.Services.IHttpRepository;

namespace Web.Services.HttpRepository;
internal class SaleHttpService : ISaleHttpService
{
    private readonly CustomHttpClient _client;
    private readonly JsonSerializerSettings _options;
    private readonly string additionalResourceName = "sales";

    public SaleHttpService(CustomHttpClient client, JsonSerializerSettings options)
    {
        _client = client;
        _options = options;
    }

    public async Task<PagingResponse<SaleDtoForSummary>> GetSalesForSummary(SaleParam saleParam)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            [$"{nameof(SaleParam.PageNumber)}"] = saleParam.PageNumber.ToString(),
            [$"{nameof(SaleParam.SrcCustomerID)}"] = saleParam.SrcCustomerID.ToString() ?? "",
            [$"{nameof(SaleParam.SrcCustomerName)}"] = saleParam.SrcCustomerName ?? "",
            [$"{nameof(SaleParam.SrcDateFrom)}"] = saleParam.SrcDateFrom.ToString() ?? "",
            [$"{nameof(SaleParam.SrcDateTo)}"] = saleParam.SrcDateTo.ToString() ?? "",
            [$"{nameof(SaleParam.OrderBy)}"] = saleParam.OrderBy ?? ""
        };

        var queryHelper = QueryHelpers.AddQueryString(additionalResourceName, queryStringParam!);
        HttpResponseMessage response = await _client.GetResponseAsync(queryHelper);
        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<SaleDtoForSummary>()
        {
            Items = JsonConvert.DeserializeObject<List<SaleDtoForSummary>>(content, _options)!,
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First(), _options)!
        };

        return pagingResponse;
    }

    public async Task<SaleDto> GetSaleByID(int saleID)
    {
        var content = await _client.GetResponseAndContentAsync($"{additionalResourceName}/{saleID}");
        var result = JsonConvert.DeserializeObject<SaleDto>(content, _options);
        if (!string.IsNullOrEmpty(content) && result is not null)
            return result;
        else
            return new();
    }

    public async Task<HttpResponseMessage> Create(SaleDto saleDto)
    {
        var response = await _client.PostAsync(additionalResourceName, saleDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }

    public async Task<HttpResponseMessage> Update(SaleDto saleDto)
    {
        var response = await _client.PutAsync(additionalResourceName, saleDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }

    public async Task<HttpResponseMessage> Delete(int saleID)
    {
        var response = await _client.DeleteAsync($"{additionalResourceName}/{saleID}");
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }
}
