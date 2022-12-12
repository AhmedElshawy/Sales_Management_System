using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria);
        Task<TType> FindAsync<TType>(Expression<Func<T, bool>> criteria, Expression<Func<T, TType>> select);
        Task<List<T>> ListAllAsync();
        Task<List<T>> ListAllAsync(Expression<Func<T, object>> include);
        Task<List<T>> ListAllAsync(string[] includes);
        Task<List<T>> ListAllAsync(Expression<Func<T, bool>> criteria);
        Task<List<T>> ListAllAsync(Expression<Func<T, bool>> criteria, Expression<Func<T, bool>> secondCriteria);
        Task<ICollection<TType>> ListAllAsync<TType>(Expression<Func<T, TType>> select);
        Task<ICollection<TType>> ListAllAsync<TType>(Expression<Func<T, bool>> criteria, Expression<Func<T, TType>> select);
        Task<List<T>> ListTopRecordsAsync(int topRecordsNumber);
        Task<List<T>> ListTopRecordsAsync(int topRecordsNumber, Expression<Func<T, bool>> criteria);
        Task AddAsync(T entity);
        void Update(T entity);
        bool Delete(T entity);
    }
}
