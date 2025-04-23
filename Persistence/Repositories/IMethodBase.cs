using System.Linq.Expressions;

namespace Persistence.Repositories;

public interface IMethodBase<T1>
{
  IQueryable<T1> FindAll(bool trackChanges);
  IQueryable<T1> FindByCondition(Expression<Func<T1, bool>> expression, bool trackChanges);
  IQueryable<T1> FindWithIncludes(bool trackChanges, params Expression<Func<T1, object>>[] includes);
  void Create(T1 entity);
  void Update(T1 entity);
  void Delete(T1 entity);
}