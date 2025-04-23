using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Application.Mapping;
internal class GeneralMappingProfile : Profile
{
  public GeneralMappingProfile()
  {
    CreateMap<CustomerModel, CustomerDTO>().ReverseMap();
    CreateMap<SupplierModel, SupplierDto>().ReverseMap();
    CreateMap<ProductCategoryModel, ProductCategoryDto>().ReverseMap();
    CreateMap<ProductModel, ProductDto>().ReverseMap();
    CreateMap<ProductModel, ProductDtoForProductQueries>();
    CreateMap<UserRegistrationDTO, UserModel>();
  }
}
