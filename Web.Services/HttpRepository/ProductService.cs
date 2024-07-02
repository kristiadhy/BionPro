using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Services.Extensions;
using Web.Services.Features;
using Web.Services.IHttpRepository;

namespace Web.Services.HttpRepository;
internal class ProductService : IProductService
{
    private readonly CustomHttpClient _client;
    private readonly JsonSerializerSettings _options;
    private readonly string additionalResourceName = "products";

    public ProductService(CustomHttpClient client, JsonSerializerSettings options)
    {
        _client = client;
        _options = options;
    }

    public async Task<PagingResponse<ProductDto>> GetProductCategories(ProductParam productParam)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageNumber"] = productParam.PageNumber.ToString(),
            ["searchTerm"] = productParam.srcByName == null ? "" : productParam.srcByName,
            ["orderBy"] = productParam.OrderBy!
        };

        var queryHelper = QueryHelpers.AddQueryString(additionalResourceName, queryStringParam!);
        HttpResponseMessage response = await _client.GetResponseAsync(queryHelper);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseForGetMethod(response);

        var pagingResponse = new PagingResponse<ProductDto>()
        {
            Items = JsonConvert.DeserializeObject<List<ProductDto>>(content, _options)!,
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First(), _options)!
        };

        return pagingResponse;
    }

    public async Task<ProductDto> GetProductByID(Guid productID)
    {
        var content = await _client.GetResponseAndContentAsync($"{additionalResourceName}/{productID}");
        var result = JsonConvert.DeserializeObject<ProductDto>(content, _options);
        if (!string.IsNullOrEmpty(content))
            return result!;
        else
            return new();
    }

    public async Task<HttpResponseMessage> Create(ProductDto productDto)
    {
        var response = await _client.PostAsync(additionalResourceName, productDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseForPostMethod(response, content, _options);
        return response;
    }

    public async Task<HttpResponseMessage> Update(ProductDto productDto)
    {
        var response = await _client.PutAsync(additionalResourceName, productDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseForPostMethod(response, content, _options);
        return response;
    }

    public async Task<HttpResponseMessage> Delete(Guid productID)
    {
        var response = await _client.DeleteAsync($"{additionalResourceName}/{productID}");
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseForPostMethod(response, content, _options);
        return response;
    }
}
