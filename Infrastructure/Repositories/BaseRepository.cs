using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Infrastructure.Context;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T>: IBaseRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = _context.Set<T>();
            return await query.SingleOrDefaultAsync(criteria);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> ListAllAsync(Expression<Func<T, object>> include)
        {
            IQueryable<T> query = _context.Set<T>();
            return await query.Include(include).ToListAsync();
        }

        public async Task<List<T>> ListAllAsync(string[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return await query.ToListAsync();
        }

        public async Task<List<T>> ListAllAsync(Expression<Func<T, bool>> criteria)
        {
            return await _context.Set<T>().Where(criteria).ToListAsync();
        }

        public async Task<List<T>> ListAllAsync(int numberOfRows ,Expression<Func<T, bool>> criteria)
        {
            return await _context.Set<T>().Where(criteria).Take(numberOfRows).ToListAsync();
        }

        public async Task<List<T>> ListAllAsync(int numberOfRows, Expression<Func<T, bool>> criteria, Expression<Func<T, object>> include)
        {
            return await _context.Set<T>().Where(criteria).Take(numberOfRows).Include(include).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<List<T>> ListAllAsync(Expression<Func<T, bool>> criteria
            , Expression<Func<T, bool>> secondCriteria)
        {
            var query = _context.Set<T>().Where(criteria);
            query = query.Where(secondCriteria);

            return await query.ToListAsync();
        }

        public async Task<List<T>> ListTopRecordsAsync(int topRecordsNumber)
        {
            return await _context.Set<T>().Take(topRecordsNumber).ToListAsync();
        }
        public async Task<List<T>> ListTopRecordsAsync(int topRecordsNumber, Expression<Func<T, object>> include)
        {
            return await _context.Set<T>().Take(topRecordsNumber)
                .Include(include)
                .ToListAsync();               
        }
        
        public async Task<List<T>> ListTopRecordsAsync(int topRecordsNumber, Expression<Func<T, bool>> criteria)
        {
            var query = await _context.Set<T>().Where(criteria).Take(topRecordsNumber)
                .ToListAsync();

            return query;
        }

        public async Task<List<T>> ListRecentAddedRecordsAsync<TKey>(int numberOfRecords, Expression<Func<T, TKey>> selector, Expression<Func<T, object>> include)
        {
            return await _context.Set<T>()
                .OrderByDescending(selector)
                .Take(numberOfRecords)
                .Include(include)
                .ToListAsync();
        }

        public bool Delete(T entity)
        {
            var result = _context.Remove(entity);
            if (result == null)
            {
                return false;
            }
            return true;
        }
        public async Task<ICollection<TType>> ListAllAsync<TType>(Expression<Func<T, TType>> select)
        {
            var query = _context.Set<T>().Select(select);

            return await query.ToListAsync();
        }

        public async Task<ICollection<TType>> ListAllAsync<TType>(Expression<Func<T, bool>> criteria, Expression<Func<T, TType>> select)
        {
            var query = _context.Set<T>()
                .Where(criteria).Select(select);

            return await query.ToListAsync();
        }

        public async Task<TType> FindAsync<TType>(Expression<Func<T, bool>> criteria, Expression<Func<T, TType>> select)
        {
            var query = _context.Set<T>()
                .Where(criteria).Select(select);

            return await query.FirstOrDefaultAsync();
        }

        public decimal SumColumn(Expression<Func<T, decimal>> columnToSum)
        {
            var query = _context.Set<T>()
                .Sum(columnToSum);

            return query;
        }

        public decimal SumColumn(Expression<Func<T, bool>> criteria, Expression<Func<T, decimal>> columnToSum)
        {
            var query = _context.Set<T>().Where(criteria)
                .Sum(columnToSum);

            return query;
        }
        
    }
}
