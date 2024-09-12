using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Services.Extensions;
using Web.Services.Features;
using Web.Services.IHttpRepository;

namespace Web.Services.HttpRepository;
public class CustomerHttpService : ICustomerHttpService
{
    private readonly CustomHttpClient _client;
    private readonly string additionalResourceName = "customers";

    public CustomerHttpService(CustomHttpClient client)
    {
        _client = client;
    }

    public async Task<PagingResponse<CustomerDTO>> GetCustomers(CustomerParam customerParameter)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            [$"{nameof(CustomerParam.PageNumber)}"] = customerParameter.PageNumber.ToString(),
            [$"{nameof(CustomerParam.SrcByName)}"] = customerParameter.SrcByName ?? "",
            [$"{nameof(CustomerParam.OrderBy)}"] = customerParameter.OrderBy!
        };

        var queryHelper = QueryHelpers.AddQueryString(additionalResourceName, queryStringParam!);
        HttpResponseMessage response = await _client.GetResponseAsync(queryHelper);
        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<CustomerDTO>()
        {
            Items = JsonConvert.DeserializeObject<List<CustomerDTO>>(content)!,
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First())!
        };

        return pagingResponse;
    }

    public async Task<CustomerDTO> GetCustomerByID(Guid customerID)
    {
        var response = await _client.GetResponseAsync($"{additionalResourceName}/{customerID}");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<CustomerDTO>(content);
        if (result is not null)
            return result;
        else
            return new();
    }

    public async Task Create(CustomerDTO customerDTO)
    {
        var response = await _client.PostAsync(additionalResourceName, customerDTO);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content);
    }

    public async Task Update(CustomerDTO customerDTO)
    {
        var response = await _client.PutAsync(additionalResourceName, customerDTO);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content);
    }

    public async Task Delete(Guid customerID)
    {
        var response = await _client.DeleteAsync($"{additionalResourceName}/{customerID}");
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content);
    }
}
