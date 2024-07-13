using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Services.Extensions;
using Web.Services.Features;
using Web.Services.IHttpRepository;

namespace Web.Services.HttpRepository;
internal class ProductCategoryService : IProductCategoryService
{
    private readonly CustomHttpClient _client;
    private readonly JsonSerializerSettings _options;
    private readonly string additionalResourceName = "products/categories";

    public ProductCategoryService(CustomHttpClient client, JsonSerializerSettings options)
    {
        _client = client;
        _options = options;
    }

    public async Task<PagingResponse<ProductCategoryDto>> GetProductCategories(ProductCategoryParam productCategoryParam)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageNumber"] = productCategoryParam.PageNumber.ToString(),
            ["searchTerm"] = productCategoryParam.SrcByName == null ? "" : productCategoryParam.SrcByName,
            ["orderBy"] = productCategoryParam.OrderBy!
        };

        var queryHelper = QueryHelpers.AddQueryString(additionalResourceName, queryStringParam!);
        HttpResponseMessage response = await _client.GetResponseAsync(queryHelper);
        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<ProductCategoryDto>()
        {
            Items = JsonConvert.DeserializeObject<List<ProductCategoryDto>>(content, _options)!,
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First(), _options)!
        };

        return pagingResponse;
    }

    public async Task<ProductCategoryDto> GetProductCategoryByID(int categoryID)
    {
        var content = await _client.GetResponseAndContentAsync($"{additionalResourceName}/{categoryID}");
        var result = JsonConvert.DeserializeObject<ProductCategoryDto>(content, _options);
        if (!string.IsNullOrEmpty(content) && result is not null)
            return result;
        else
            return new();
    }

    public async Task<HttpResponseMessage> Create(ProductCategoryDto productCategoryDto)
    {
        var response = await _client.PostAsync(additionalResourceName, productCategoryDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }

    public async Task<HttpResponseMessage> Update(ProductCategoryDto productCategoryDto)
    {
        var response = await _client.PutAsync(additionalResourceName, productCategoryDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }

    public async Task<HttpResponseMessage> Delete(int categoryID)
    {
        var response = await _client.DeleteAsync($"{additionalResourceName}/{categoryID}");
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }
}
