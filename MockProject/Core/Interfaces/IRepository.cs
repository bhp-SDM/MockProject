using System.Collections.Generic;

namespace MockProject.Core.Interfaces
{
    public interface IRepository<K, T>
    {
        int Count { get; }
        void Add(T item);
        void Remove(T item);
        T GetByID(K id);
        List<T> GetAll();
    }
}
