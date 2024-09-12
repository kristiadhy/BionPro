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
    private readonly string additionalResourceName = "sales";

    public SaleHttpService(CustomHttpClient client)
    {
        _client = client;
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
            Items = JsonConvert.DeserializeObject<List<SaleDtoForSummary>>(content)!,
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First())!
        };

        return pagingResponse;
    }

    public async Task<SaleDto> GetSaleByID(int saleID)
    {
        var response = await _client.GetResponseAsync($"{additionalResourceName}/{saleID}");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<SaleDto>(content);
        if (result is not null)
            return result;
        else
            return new();
    }

    public async Task Create(SaleDto saleDto)
    {
        var response = await _client.PostAsync(additionalResourceName, saleDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content);
    }

    public async Task Update(SaleDto saleDto)
    {
        var response = await _client.PutAsync(additionalResourceName, saleDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content);
    }

    public async Task Delete(int saleID)
    {
        var response = await _client.DeleteAsync($"{additionalResourceName}/{saleID}");
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content);
    }
}
