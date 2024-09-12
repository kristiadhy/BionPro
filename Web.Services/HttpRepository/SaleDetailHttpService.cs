using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Services.Extensions;
using Web.Services.Features;
using Web.Services.IHttpRepository;

namespace Web.Services.HttpRepository;
internal class SaleDetailHttpService : ISaleDetailHttpService
{
    private readonly CustomHttpClient _client;
    private readonly JsonSerializerSettings _options;
    private readonly string additionalResourceName = "sale/details";

    public SaleDetailHttpService(CustomHttpClient client, JsonSerializerSettings options)
    {
        _client = client;
        _options = options;
    }


    public async Task<PagingResponse<SaleDetailDto>> GetSaleByID(int saleID, SaleDetailParam saleDetailParam)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            [$"{nameof(SaleDetailParam.PageNumber)}"] = saleDetailParam.PageNumber.ToString(),
            [$"{nameof(SaleDetailParam.SrcProduct)}"] = saleDetailParam.SrcProduct ?? "",
            [$"{nameof(SaleDetailParam.OrderBy)}"] = saleDetailParam.OrderBy!
        };

        var queryHelper = QueryHelpers.AddQueryString($"{additionalResourceName}/{saleID}", queryStringParam!);
        HttpResponseMessage response = await _client.GetResponseAsync(queryHelper);
        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<SaleDetailDto>()
        {
            Items = JsonConvert.DeserializeObject<List<SaleDetailDto>>(content)!,
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First())!
        };

        return pagingResponse;
    }
}
