using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace KallpaBox.Site.Data
{
    /// <summary>
    /// "There's some repetition here - couldn't we have some the sync methods call the ?"
    /// https://blogs.msdn.microsoft.com/pfxteam/2012/04/13/should-i-expose-synchronous-wrappers-for-hronous-methods/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly GymContext _dbContext;

        public EfRepository(GymContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public virtual  T GetById(int? id)
        {
            return  _dbContext.Set<T>().Find(id);
        }
        
        public  IReadOnlyList<T> ListAll()
        {
            return  _dbContext.Set<T>().ToList();
        }

        public  IReadOnlyList<T> List(ISpecification<T> spec)
        {
            return  ApplySpecification(spec).ToList();
        }
        
        public  int Count(ISpecification<T> spec)
        {
            return  ApplySpecification(spec).Count();
        }

        public  void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
             _dbContext.SaveChanges();
        }
        
        public  void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
             _dbContext.SaveChanges();
        }
        
        public  void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
             _dbContext.SaveChanges();
        }
        
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }
    }
}
