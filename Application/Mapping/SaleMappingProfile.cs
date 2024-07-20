using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Application.Mapping;
internal class SaleMappingProfile : Profile
{
    public SaleMappingProfile()
    {
        CreateMap<SaleDetailModel, SaleDetailDto>();
        CreateMap<SaleDetailDto, SaleDetailModel>()
           .ForMember(dest => dest.Product, opt => opt.Ignore()); // Ignore the Product object to prevent creating a new one
        CreateMap<SaleModel, SaleDto>()
            .ForMember(dest => dest.SaleDetails, opt => opt
            .MapFrom(src => src.SaleDetails))
            .ForMember(dest => dest.CustomerID, opt => opt
            .MapFrom(src => src.Customer!.CustomerID))
             .ForMember(dest => dest.CustomerName, opt => opt
            .MapFrom(src => src.Customer!.CustomerName));
        CreateMap<SaleDto, SaleModel>()
            .ForMember(dest => dest.Customer, opt => opt.Ignore());
    }
}
