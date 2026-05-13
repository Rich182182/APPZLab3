using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FitnessClub.DAL.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        IQueryable<T> GetWithIncludes(params Expression<Func<T, object>>[] includes);
        IQueryable<T> FindWithIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> Find(Func<T, bool> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        
    }
}