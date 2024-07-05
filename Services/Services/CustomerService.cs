using Application.Exceptions;
using Application.IRepositories;
using Application.Validators;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;
using Serilog;
using Services.Contracts;

namespace Services;

internal sealed class CustomerService : ICustomerService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CustomerService(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(IEnumerable<CustomerDTO> customerDTO, MetaData metaData)> GetByParametersAsync(Guid customerID, CustomerParam customerParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information("Get customers");

        var customers = await _repositoryManager.CustomerRepo.GetByParametersAsync(customerParam, trackChanges, cancellationToken);
        //if (!customers.Any())
        //    throw new NoCustomerFoundException();

        _logger.Information("Customers retrieved");

        var customersToReturn = _mapper.Map<IEnumerable<CustomerDTO>>(customers);

        return (customersToReturn, customers.MetaData);
    }

    public async Task<CustomerDTO> GetByCustomerIDAsync(Guid customerID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information("Get customer with ID : {customerID}", customerID);

        var customer = await _repositoryManager.CustomerRepo.GetByIDAsync(customerID, trackChanges, cancellationToken);
        if (customer is null)
            throw new CustomerIDNotFoundException(customerID);

        _logger.Information("Customer {customerName} retrieved", customer.CustomerName);

        var customersToReturn = _mapper.Map<CustomerDTO>(customer);

        return customersToReturn;
    }

    public async Task<CustomerDTO> CreateAsync(CustomerDTO customerDto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var customerModel = _mapper.Map<CustomerModel>(customerDto);
        var validator = new CustomerValidator();
        validator.ValidateInput(customerModel);

        _logger.Information("Insert new customer {customerName}", customerDto.CustomerName);

        _repositoryManager.CustomerRepo.CreateEntity(customerModel, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        _logger.Information("Customer {customerName} added", customerDto.CustomerName);

        var customerToReturn = _mapper.Map<CustomerDTO>(customerModel);

        return customerToReturn;
    }

    public async Task UpdateAsync(CustomerDTO customerDto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var customerModel = _mapper.Map<CustomerModel>(customerDto);
        var validator = new CustomerValidator();
        validator.ValidateInput(customerModel);

        _logger.Information("Update customer {customerName}", customerDto.CustomerName);

        _repositoryManager.CustomerRepo.UpdateEntity(customerModel, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        _logger.Information("Customer {customerName} updated", customerDto.CustomerName);
    }

    public async Task DeleteAsync(Guid customerID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        //var lstCustomerMD = ServiceExtensions<CustomerMD, CustomerDTO, CustomerQP>.AddEntityListfromQP(QP);
        //var lstCustomerMD = DataProcessingExtension.Create_ListOfModel_FromListOfDTO<CustomerMD, CustomerDTO>(lstCustomerDTOs, _mapper);

        //foreach (var row in lstCustomerDTOs)
        //    _logger.LogInformation("Delete customer : {customerName}", row.CustomerName);

        var customerToDelete = await _repositoryManager.CustomerRepo.GetByIDAsync(customerID, trackChanges, cancellationToken);
        if (customerToDelete is null)
            throw new CustomerIDNotFoundException(customerID);

        _logger.Information("Delete customer {customerName}", customerToDelete.CustomerName);

        _repositoryManager.CustomerRepo.DeleteEntity(customerToDelete, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        _logger.Information("Customer {customerName} deleted", customerToDelete.CustomerName);
    }

    public async Task<(CustomerDTO customerToPatch, CustomerModel customer)> GetCustomerForPatchAsync(Guid customerID, bool empTrackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information("Get customer with ID : {customerID}", customerID);

        var customer = await _repositoryManager.CustomerRepo.GetByIDAsync(customerID, empTrackChanges, cancellationToken);
        if (customer is null)
            throw new CustomerIDNotFoundException(customerID);

        _logger.Information("Customer {customerName} retrieved", customer.CustomerName);

        var customerToPatch = _mapper.Map<CustomerDTO>(customer);

        return (customerToPatch, customer);
    }

    public async Task SaveChangesForPatchAsync(CustomerDTO customerToPatch, CustomerModel customer, CancellationToken cancellationToken = default)
    {
        _mapper.Map(customerToPatch, customer);

        _logger.Information("Update customer {customerName}", customerToPatch.CustomerName);

        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        _logger.Information("Customer {customerName} updated", customerToPatch.CustomerName);
    }
}
