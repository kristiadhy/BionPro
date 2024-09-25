using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.State;

public class SaleDetailState(IServiceManager serviceManager)
{
    private readonly IServiceManager _serviceManager = serviceManager;
    public List<SaleDetailDto> SaleDetailList { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public SaleDetailDto SaleDetail { get; set; } = new();

    //public async Task LoadSaleDetails()
    //{
    //    //var pagingResponse = await ServiceManager.SaleDetailService.GetSaleDetails(SaleDetailParameter);
    //    //SaleDetailList = pagingResponse.Items;
    //    //MetaData = pagingResponse.MetaData;
    //}
}
