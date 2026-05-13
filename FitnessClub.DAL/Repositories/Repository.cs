using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FitnessClub.DAL.Context;
using FitnessClub.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessClub.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly FitnessContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(FitnessContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T Get(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public IQueryable<T> GetWithIncludes(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }

        public IQueryable<T> FindWithIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.Where(predicate);
        }

        public void Create(T item)
        {
            _dbSet.Add(item);
        }

        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            T item = _dbSet.Find(id);
            if (item != null)
            {
                _dbSet.Remove(item);
            }
        }
    }
}