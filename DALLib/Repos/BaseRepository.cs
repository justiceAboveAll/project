using System.Collections.Generic;
using System.Linq;

using DALLib.EF;
using System.Data.Entity;
using DALLib.Contracts;
using System;

namespace DALLib.Repos
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected MsSqlMonitorEntities context;
        protected DbSet<T> table;

        public BaseRepository(MsSqlMonitorEntities context)
        {
            this.context = context;
            table = context.Set<T>();
        }

        public IEnumerable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {

            IEnumerable<T> query = table.Where(predicate).AsEnumerable();
            return query;
        }

        public virtual void Create(T item)
        {
            table.Add(item);
        }

        public virtual void Delete(T item)
        {
            table.Remove(item);
        }

        public virtual void DeleteAll()
        {
            foreach (var entity in table)
            {
                table.Remove(entity);
            }
        }

        public virtual T Get(int id)
        {
            return table.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return table;
        }

        public virtual void Update(T item)
        {
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
