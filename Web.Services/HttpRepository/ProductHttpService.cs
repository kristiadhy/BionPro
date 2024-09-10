using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Services.Extensions;
using Web.Services.Features;
using Web.Services.IHttpRepository;

namespace Web.Services.HttpRepository;
internal class ProductHttpService : IProductHttpService
{
    private readonly CustomHttpClient _client;
    private readonly JsonSerializerSettings _options;
    private readonly string additionalResourceName = "products";

    public ProductHttpService(CustomHttpClient client, JsonSerializerSettings options)
    {
        _client = client;
        _options = options;
    }

    public async Task<PagingResponse<ProductDtoForProductQueries>> GetProducts(ProductParam productParam)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            [$"{nameof(ProductParam.PageNumber)}"] = productParam.PageNumber.ToString(),
            [$"{nameof(ProductParam.SrcByProductCategory)}"] = productParam.SrcByProductCategory.ToString() ?? "",
            [$"{nameof(ProductParam.SrcByProductName)}"] = productParam.SrcByProductName ?? "",
            [$"{nameof(ProductParam.OrderBy)}"] = productParam.OrderBy ?? ""
        };

        var queryHelper = QueryHelpers.AddQueryString(additionalResourceName, queryStringParam!);
        HttpResponseMessage response = await _client.GetResponseAsync(queryHelper);
        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<ProductDtoForProductQueries>()
        {
            Items = JsonConvert.DeserializeObject<List<ProductDtoForProductQueries>>(content, _options)!,
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First(), _options)!
        };

        return pagingResponse;
    }

    public async Task<ProductDto> GetProductByID(Guid productID)
    {
        var content = await _client.GetResponseAndContentAsync($"{additionalResourceName}/{productID}");
        var result = JsonConvert.DeserializeObject<ProductDto>(content, _options);
        if (!string.IsNullOrEmpty(content) && result is not null)
            return result;
        else
            return new();
    }

    public async Task<HttpResponseMessage> Create(ProductDto productDto)
    {
        var response = await _client.PostAsync(additionalResourceName, productDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }

    public async Task<HttpResponseMessage> Update(ProductDto productDto)
    {
        var response = await _client.PutAsync(additionalResourceName, productDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }

    public async Task<HttpResponseMessage> Delete(Guid productID)
    {
        var response = await _client.DeleteAsync($"{additionalResourceName}/{productID}");
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }

    public async Task<string> UploadProductImage(MultipartFormDataContent multiContent)
    {
        var response = await _client.PostMultiContentAsync($"{additionalResourceName}/upload", multiContent);
        var postContent = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, postContent, _options);
        return postContent.Trim('"');
    }

    public async Task<byte[]?> GetProductImage(string fileName)
    {
        var responseMessage = await _client.GetResponseAsync($"{additionalResourceName}/upload/{fileName}");
        bool responseStatus = _client.CheckErrorResponseForGetMethod(responseMessage);
        if (responseStatus)
        {
            var fileBytes = await responseMessage.Content.ReadAsByteArrayAsync();
            return fileBytes;
        }
        else
            return null;
    }

    public async Task<HttpResponseMessage> DeleteProductImage(string imageUrl)
    {
        string fileName = Path.GetFileName(imageUrl);
        var response = await _client.DeleteAsync($"{additionalResourceName}/upload/{fileName}");
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }
}
