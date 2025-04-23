using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Application.Mapping;
internal class PurchaseMappingProfile : Profile
{
  public PurchaseMappingProfile()
  {
    CreateMap<PurchaseDetailModel, PurchaseDetailDto>();
    CreateMap<PurchaseDetailDto, PurchaseDetailModel>()
       .ForMember(dest => dest.Product, opt => opt.Ignore()); // Ignore the Product object to prevent creating a new one
    CreateMap<PurchaseModel, PurchaseDto>()
        .ForMember(dest => dest.PurchaseDetails, opt => opt
        .MapFrom(src => src.PurchaseDetails))

        .ForMember(dest => dest.SupplierID, opt => opt
        .MapFrom(src => src.Supplier!.SupplierID))

         .ForMember(dest => dest.SupplierName, opt => opt
        .MapFrom(src => src.Supplier!.SupplierName));
    CreateMap<PurchaseDto, PurchaseModel>()
        .ForMember(dest => dest.Supplier, opt => opt.Ignore());

    //CreateMap<PurchaseModel, PurchaseDtoForSummary>()
    //     .ForMember(dest => dest.GrandTotal, opt => opt.MapFrom(src => CalculateGrandTotal(src)))

    //.MapFrom(src => src.PurchaseDetails!.Select(pd => new PurchaseDetailDto
    // {
    //     Quantity = pd.Quantity,
    //     Price = pd.Price,
    //     DiscountPercentage = pd.DiscountPercentage,
    //     DiscountAmount = pd.DiscountAmount,
    //     ProductID = pd.Product!.ProductID,
    //     ProductName = pd.Product.Name
    // })))
  }
}
