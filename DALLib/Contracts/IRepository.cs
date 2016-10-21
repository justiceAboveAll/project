using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DALLib.Contracts
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(T item);
        void DeleteAll();
    }
}
