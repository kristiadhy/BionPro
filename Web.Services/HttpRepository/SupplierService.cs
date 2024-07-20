using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Services.Extensions;
using Web.Services.Features;
using Web.Services.IHttpRepository;

namespace Web.Services.HttpRepository;
public class SupplierService : ISupplierService
{
    private readonly CustomHttpClient _client;
    private readonly JsonSerializerSettings _options;
    private readonly string additionalResourceName = "suppliers";

    public SupplierService(CustomHttpClient client, JsonSerializerSettings options)
    {
        _client = client;
        _options = options;
    }

    public async Task<PagingResponse<SupplierDto>> GetSuppliers(SupplierParam supplierParameter)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            [$"{nameof(SupplierParam.PageNumber)}"] = supplierParameter.PageNumber.ToString(),
            [$"{nameof(SupplierParam.SrcByName)}"] = supplierParameter.SrcByName == null ? "" : supplierParameter.SrcByName,
            [$"{nameof(SupplierParam.OrderBy)}"] = supplierParameter.OrderBy!
        };

        var queryHelper = QueryHelpers.AddQueryString(additionalResourceName, queryStringParam!);
        HttpResponseMessage response = await _client.GetResponseAsync(queryHelper);
        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<SupplierDto>()
        {
            Items = JsonConvert.DeserializeObject<List<SupplierDto>>(content, _options)!,
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First(), _options)!
        };

        return pagingResponse;
    }

    public async Task<SupplierDto> GetSupplierByID(Guid supplierID)
    {
        var content = await _client.GetResponseAndContentAsync($"{additionalResourceName}/{supplierID}");
        var result = JsonConvert.DeserializeObject<SupplierDto>(content, _options);
        if (!string.IsNullOrEmpty(content) && result is not null)
            return result;
        else
            return new();
    }

    public async Task<HttpResponseMessage> Create(SupplierDto supplierDto)
    {
        var response = await _client.PostAsync(additionalResourceName, supplierDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }

    public async Task<HttpResponseMessage> Update(SupplierDto supplierDto)
    {
        var response = await _client.PutAsync(additionalResourceName, supplierDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }

    public async Task<HttpResponseMessage> Delete(Guid supplierID)
    {
        var response = await _client.DeleteAsync($"{additionalResourceName}/{supplierID}");
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseWithContent(response, content, _options);
        return response;
    }
}
