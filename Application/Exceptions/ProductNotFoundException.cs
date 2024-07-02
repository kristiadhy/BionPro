namespace Application.Exceptions;
public sealed class ProductNotFoundException(Guid productID) : NotFoundException($"Product with id: {productID} doesn't exist in the database.") { }
