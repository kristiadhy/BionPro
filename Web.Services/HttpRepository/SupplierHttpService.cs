using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Services.Extensions;
using Web.Services.Features;
using Web.Services.IHttpRepository;

namespace Web.Services.HttpRepository;
public class SupplierHttpService : ISupplierHttpService
{
    private readonly CustomHttpClient _client;
    private readonly string additionalResourceName = "suppliers";

    public SupplierHttpService(CustomHttpClient client)
    {
        _client = client;
    }

    public async Task<PagingResponse<SupplierDto>> GetSuppliers(SupplierParam supplierParameter)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            [$"{nameof(SupplierParam.PageNumber)}"] = supplierParameter.PageNumber.ToString(),
            [$"{nameof(SupplierParam.SrcByName)}"] = supplierParameter.SrcByName ?? "",
            [$"{nameof(SupplierParam.OrderBy)}"] = supplierParameter.OrderBy!
        };

        var queryHelper = QueryHelpers.AddQueryString(additionalResourceName, queryStringParam!);
        HttpResponseMessage response = await _client.GetResponseAsync(queryHelper);
        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<SupplierDto>()
        {
            Items = JsonConvert.DeserializeObject<List<SupplierDto>>(content)!,
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First())!
        };

        return pagingResponse;
    }

    public async Task<SupplierDto> GetSupplierByID(Guid supplierID)
    {
        var response = await _client.GetResponseAsync($"{additionalResourceName}/{supplierID}");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<SupplierDto>(content);
        if (result is not null)
            return result;
        else
            return new();
    }

    public async Task Create(SupplierDto supplierDto)
    {
        var response = await _client.PostAsync(additionalResourceName, supplierDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content);
    }

    public async Task Update(SupplierDto supplierDto)
    {
        var response = await _client.PutAsync(additionalResourceName, supplierDto);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content);
    }

    public async Task Delete(Guid supplierID)
    {
        var response = await _client.DeleteAsync($"{additionalResourceName}/{supplierID}");
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content);
    }
}
