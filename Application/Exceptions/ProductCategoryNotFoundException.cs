using Application.Exceptions.Abstractions;

namespace Application.Exceptions;
public sealed class ProductCategoryNotFoundException(int categoryID) : NotFoundException($"Product category with id: {categoryID} doesn't exist in the database.");
