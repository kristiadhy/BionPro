using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Application.Mapping;
internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CustomerModel, CustomerDTO>().ReverseMap();
        CreateMap<SupplierModel, SupplierDto>().ReverseMap();
        CreateMap<ProductCategoryModel, ProductCategoryDto>().ReverseMap();
        CreateMap<ProductModel, ProductDto>().ReverseMap();
        CreateMap<ProductModel, ProductDtoForProductQueries>();
        CreateMap<PurchaseDetailModel, PurchaseDetailDto>();
        CreateMap<PurchaseDetailDto, PurchaseDetailModel>()
           .ForMember(dest => dest.Product, opt => opt.Ignore()); // Ignore the Product object to prevent creating a new one
        CreateMap<PurchaseModel, PurchaseDto>()
            .ForMember(dest => dest.PurchaseDetails, opt => opt
            .MapFrom(src => src.PurchaseDetails))
            .ReverseMap();
        CreateMap<UserRegistrationDTO, UserModel>();
    }
}
