using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions;
public sealed class ProductNotFoundException(Guid productID) : NotFoundException($"Product with id: {productID} doesn't exist in the database.") { }
