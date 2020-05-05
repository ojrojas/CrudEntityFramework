using System.Collections.Generic;
using KallpaBox.Core.Entities;

namespace KallpaBox.Core.Interfaces
{
    public interface IRepository<T> where T :BaseEntity
    {
        T GetById(int? id);
        IReadOnlyList<T> ListAll();
        IReadOnlyList<T> List(ISpecification<T> spec);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        int Count(ISpecification<T> spec);
    }
}