using Application.Exceptions.Abstractions;

namespace Application.Exceptions;

public sealed class PurchaseIDNotFoundException(int purchaseID) : NotFoundException($"Purchase with id: {purchaseID} doesn't exist in the database.");

public sealed class PurchaseCodeAlreadyExistException(string transactionCode) : BadRequestException($"Purchase with transaction code: {transactionCode} already exist in the database.");