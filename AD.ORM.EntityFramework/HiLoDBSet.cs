using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AD.DomainModel;

namespace AD.ORM.EntityFramework
{
    public class HiLoSet<T> : IDbSet<T> where T : Entity<long>
    {     
        private readonly IDbSet<T> _dbSet;     
        private readonly IHiLoGenerator<long> _generator;       
        
        public HiLoSet(IDbSet<T> dbSet, IHiLoGenerator<long> generator)
        {
            _dbSet = dbSet;         
            _generator = generator;
        }
        
        public T Add(T entity)
        {
            var add = _dbSet.Add(entity);         
            if (entity.Id == default(long))
            {
                entity.Id = _generator.GetIdentifier();
            }           
            return add;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return _dbSet.GetEnumerator();
        }       
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dbSet).GetEnumerator();
        }

        public Expression Expression
        {
            get { return _dbSet.Expression; }
        }

        public Type ElementType
        {
            get { return _dbSet.ElementType; }
        }
        
        public IQueryProvider Provider
        {
            get { return _dbSet.Provider; }
        }
        
        public T Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }
        
        public T Remove(T entity)
        {
            return _dbSet.Remove(entity);
        }

        public T Attach(T entity)
        {
            return _dbSet.Attach(entity);
        }
        
        public T Create()
        {
            return _dbSet.Create();
        }
        
        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return _dbSet.Create<TDerivedEntity>();
        }
        
        public ObservableCollection<T> Local
        {
            get { return _dbSet.Local; }
        }
    }
}