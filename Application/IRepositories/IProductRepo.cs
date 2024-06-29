using Domain.Entities;
using Domain.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories;
internal interface IProductRepo : IRepositoryBase<ProductModel>
{
    Task<PagedList<ProductModel>> GetAllAsync(ProductParam productParam, bool trackChanges);
    Task<PagedList<ProductModel>> GetByParametersAsync(ProductParam productParam, bool trackChanges);
    Task<ProductModel?> GetByIDAsync(Guid productID, bool trackChanges);
}
