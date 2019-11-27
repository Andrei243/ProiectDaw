using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DataAccess.Base
{
    public class BaseRepository<T> : Common.IRepository<T> where T : class, IEntity, new()
    {
        protected SocializRContext Context { get; }
        public BaseRepository(SocializRContext context)
        {
            Context = context;
        }
        IQueryable<T> IRepository<T>.Query => Context.Set<T>();

        void IRepository<T>.Add(T el)
        {
            Context.Set<T>().Add(el);
        }

        void IRepository<T>.AddRange(IEnumerable<T> elems)
        {
            Context.Set<T>().AddRange(elems);
        }

        void IRepository<T>.Remove(T el)
        {
            Context.Set<T>().Remove(el);
        }

        void IRepository<T>.RemoveRange(IEnumerable<T> elems)
        {
            Context.Set<T>().RemoveRange(elems);
        }

        void IRepository<T>.Update(T el)
        {
            Context.Entry(el).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
