using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Classified.Data.Infrastructure
{
public interface IRepository<T> where T : class
{
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    void Delete(Expression<Func<T, bool>> where);
    T GetById(long id);
    T GetById(string id);
    T GetById(Expression<Func<T, bool>> predicate);
    T Get(Expression<Func<T, bool>> where);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
    IQueryable<T> Filter(Expression<Func<T, bool>> predicate);

}
}
