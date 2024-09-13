using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class SaleDetailState
{
    private readonly IServiceManager ServiceManager;
    public List<SaleDetailDto> SaleDetailList { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    //public SaleDetailParam SaleDetailParameter { get; set; } = new();
    public SaleDetailDto SaleDetail { get; set; } = new();

    public SaleDetailState(IServiceManager serviceManager)
    {
        ServiceManager = serviceManager;
    }

    //public async Task LoadSaleDetails()
    //{
    //    //var pagingResponse = await ServiceManager.SaleDetailService.GetSaleDetails(SaleDetailParameter);
    //    //SaleDetailList = pagingResponse.Items;
    //    //MetaData = pagingResponse.MetaData;
    //}
}
