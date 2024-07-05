namespace Services.Contracts;
public interface IServiceBase<T>
{
    Task<T> CreateAsync(T dto, bool trackChanges, CancellationToken cancellationToken = default);
    Task UpdateAsync(T dto, bool trackChanges, CancellationToken cancellationToken = default);
}
