using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Services.Extensions;
using Web.Services.Features;
using Web.Services.IHttpRepository;

namespace Web.Services.HttpRepository;
internal class PurchaseDetailService : IPurchaseDetailService
{
    private readonly CustomHttpClient _client;
    private readonly JsonSerializerSettings _options;
    private readonly string additionalResourceName = "purchase/details";

    public PurchaseDetailService(CustomHttpClient client, JsonSerializerSettings options)
    {
        _client = client;
        _options = options;
    }


    public async Task<PagingResponse<PurchaseDetailDto>> GetPurchaseByID(int purchaseID, PurchaseDetailParam purchaseDetailParam)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            [$"{nameof(PurchaseDetailParam.PageNumber)}"] = purchaseDetailParam.PageNumber.ToString(),
            [$"{nameof(PurchaseDetailParam.SrcProduct)}"] = purchaseDetailParam.SrcProduct ?? "",
            [$"{nameof(PurchaseDetailParam.OrderBy)}"] = purchaseDetailParam.OrderBy!
        };

        var queryHelper = QueryHelpers.AddQueryString($"{additionalResourceName}/{purchaseID}", queryStringParam!);
        HttpResponseMessage response = await _client.GetResponseAsync(queryHelper);
        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<PurchaseDetailDto>()
        {
            Items = JsonConvert.DeserializeObject<List<PurchaseDetailDto>>(content, _options)!,
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First(), _options)!
        };

        return pagingResponse;
    }
}
