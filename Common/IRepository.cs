using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Common
{
    public interface IRepository<T> : IRepository where T : IEntity
    {
        void Add(T el);
        void AddRange(IEnumerable<T> elems);
        IQueryable<T> Query { get; }
        void Remove(T el);
        void RemoveRange(IEnumerable<T> elems);
        void Update(T el);

    }

    public interface IRepository
    {

    }
}
