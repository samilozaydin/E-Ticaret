using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        public readonly ETicaretAPIDbContext _context;
        public ReadRepository(ETicaretAPIDbContext context)
        {
            _context= context;
        }
        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool tracker = true)
        {
            var query = Table.AsQueryable();
            if(!tracker)
               query= query.AsNoTracking();
            return query;
        }

        public async Task<T> GetByIdAsync(string id, bool tracker = true)
        {
            
            var query = Table.AsQueryable();
            if(!tracker)
                query= query.AsNoTracking();

            return await query.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));  
        }
        public async Task<T> GetSingelAsync(Expression<Func<T, bool>> method, bool tracker = true)
        {
            var query = Table.AsQueryable();
            if (!tracker)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(method);

        }
        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracker = true)
        {
            var query = Table.Where(method);
            if (!tracker) 
                query= query.AsNoTracking();
            return query;
        }
    }
}
