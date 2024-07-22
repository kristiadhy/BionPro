namespace Application.IRepositories;
public interface IRepositoryBase<T>
{
    void CreateEntity(T entity);
    void UpdateEntity(T entity);
    void DeleteEntity(T entity);
}
