using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Services.Extensions;
using Web.Services.Features;
using Web.Services.IHttpRepository;

namespace Web.Services.HttpRepository;
internal class ProductCategoryHttpService : IProductCategoryHttpService
{
    private readonly CustomHttpClient _client;
    private readonly JsonSerializerSettings _options;
    private readonly string additionalResourceName = "products/categories";

    public ProductCategoryHttpService(CustomHttpClient client, JsonSerializerSettings options)
    {
        _client = client;
        _options = options;
    }

    public async Task<PagingResponse<ProductCategoryDto>> GetProductCategories(ProductCategoryParam productCategoryParam)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            [$"{nameof(ProductCategoryParam.PageNumber)}"] = productCategoryParam.PageNumber.ToString(),
            [$"{nameof(ProductCategoryParam.SrcByName)}"] = productCategoryParam.SrcByName == null ? "" : productCategoryParam.SrcByName,
            [$"{nameof(ProductCategoryParam.OrderBy)}"] = productCategoryParam.OrderBy!
        };

        var queryHelper = QueryHelpers.AddQueryString(additionalResourceName, queryStringParam!);
        HttpResponseMessage response = await _client.GetResponseAsync(queryHelper);
        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<ProductCategoryDto>()
        {
            Items = JsonConvert.DeserializeObject<List<ProductCategoryDto>>(content)!,
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First())!
        };

        return pagingResponse;
    }

    public async Task<ProductCategoryDto> GetProductCategoryByID(int categoryID)
    {
        var response = await _client.GetResponseAsync($"{additionalResourceName}/{categoryID}");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ProductCategoryDto>(content);
        if (result is not null)
            return result;
        else
            return new();
    }

    public async Task Create(ProductCategoryDto productCategoryDto)
    {
        var response = await _client.PostAsync(additionalResourceName, productCategoryDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content);
    }

    public async Task Update(ProductCategoryDto productCategoryDto)
    {
        var response = await _client.PutAsync(additionalResourceName, productCategoryDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content);
    }

    public async Task Delete(int categoryID)
    {
        var response = await _client.DeleteAsync($"{additionalResourceName}/{categoryID}");
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content);
    }
}
