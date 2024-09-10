using Application.Exceptions.Abstractions;

namespace Application.Exceptions;

public sealed class SaleIDNotFoundException(int saleID) : NotFoundException($"Sale with id: {saleID} doesn't exist in the database.");

public sealed class SaleCodeAlreadyExistException(string transactionCode) : BadRequestException($"Sale with transaction code: {transactionCode} already exist in the database.");