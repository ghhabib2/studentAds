using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Classified.Data.Infrastructure
{
public abstract class RepositoryBase<T> where T : class
{
    private ClassifiedContext _dataContext;
    private readonly IDbSet<T> _dbset;
    protected RepositoryBase(IDatabaseFactory databaseFactory)
    {
        DatabaseFactory = databaseFactory;
        _dbset = DataContext.Set<T>();
    }

    protected IDatabaseFactory DatabaseFactory
    {
        get; private set;
    }

    protected ClassifiedContext DataContext
    {
        get { return _dataContext ?? (_dataContext = DatabaseFactory.Get()); }
    }
   
    public virtual void Add(T entity)
    {
        _dbset.Add(entity);
        _dataContext.SaveChanges();
    }
    public virtual void Update(T entity)
    {
        _dbset.Attach(entity);
        _dataContext.Entry(entity).State = EntityState.Modified;
        _dataContext.SaveChanges();
    }
    public virtual void Delete(T entity)
    {
        _dbset.Remove(entity);
        _dataContext.SaveChanges();  
    }
    public virtual void Delete(Expression<Func<T, bool>> where)
    {
        var objects = _dbset.Where<T>(where).AsEnumerable();
        foreach (T obj in objects)
            _dbset.Remove(obj);
        _dataContext.SaveChanges();
    } 
    public virtual T GetById(long id)
    {
        return _dbset.Find(id);

    }
    public virtual T GetById(Expression<Func<T, bool>> predicate)
    {
        return _dbset.FirstOrDefault(predicate);
    }

    public virtual T GetById(string id)
    {
        return _dbset.Find(id);
    }
    public virtual T GetById(int id)
    {
        return _dbset.Find(id);
    }
   
    public virtual IEnumerable<T> GetAll()
    {
        return _dbset.ToList();
    }
    public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
    {
        return _dbset.Where(where).ToList();
    }
    public T Get(Expression<Func<T, bool>> where)
    {
        return _dbset.Where(where).FirstOrDefault<T>();
    }
    public virtual IQueryable<T> Filter(Expression<Func<T, bool>> predicate)
    {
        return _dbset.Where(predicate).AsQueryable<T>();
    }

}
}
