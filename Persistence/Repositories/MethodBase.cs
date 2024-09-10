using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Linq.Expressions;

namespace Persistence.Repositories;

//IMPORTANT : DbSet<T>
//A DbSet<T> represents a collection for a given entity type and is the primary way of querying and saving instances of your entity classes.
//No Need for DbSet Properties: While it's common to define DbSet<T> properties in your DbContext class, you can still access entity sets dynamically using Set<T>(). This can be useful in scenarios where the entity type is determined at runtime.

public abstract class MethodBase<T> : IMethodBase<T> where T : class
{
    protected AppDBContext appDBContext;

    public MethodBase(AppDBContext repositoryContext) => appDBContext = repositoryContext;

    public IQueryable<T> FindAll(bool trackChanges) => !trackChanges ? appDBContext.Set<T>().AsNoTracking() : appDBContext.Set<T>();
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) => !trackChanges ? appDBContext.Set<T>().Where(expression).AsNoTracking() : appDBContext.Set<T>().Where(expression);
    public IQueryable<T> FindWithIncludes(bool trackChanges, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = appDBContext.Set<T>();

        if (!trackChanges)
        {
            query = query.AsNoTracking();
        }

        foreach (var includeExpression in includes)
        {
            query = query.Include(includeExpression);
        }

        return query;
    }
    public void Create(T entity) => appDBContext.Set<T>().Add(entity);
    public void Update(T entity) => appDBContext.Set<T>().Update(entity);
    public void Delete(T entity) => appDBContext.Set<T>().Remove(entity);
    public void DeleteRange(IEnumerable<T> entities) => appDBContext.Set<T>().RemoveRange(entities);
    public void Attach(T entity) => appDBContext.Set<T>().Attach(entity);

    public void IgnorePropertiesForUpdate(T entity, string dateCreated, string CreatedBy)
    {
        appDBContext.Entry(entity).Property(dateCreated).IsModified = false;
        appDBContext.Entry(entity).Property(CreatedBy).IsModified = false;
    }
}