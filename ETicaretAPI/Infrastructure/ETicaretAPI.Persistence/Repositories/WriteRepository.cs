using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        public ETicaretAPIDbContext _context;
        public WriteRepository(ETicaretAPIDbContext context) { 
            _context= context;
        }
        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entry = await Table.AddAsync(model);
            return entry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> list)
        {
            await Table.AddRangeAsync(list);
            return true;
        }

        public async Task<bool> Remove(string id)
        {
            T entity = await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
            return Remove(entity);
        }

        public bool Remove(T model)
        {
           EntityEntry entry =  Table.Remove(model);
            return entry.State == EntityState.Deleted;
        }

        public bool RemoveRange(List<T> list)
        {
            Table.RemoveRange(list);
            return true;
        }

        public bool Update(T model)
        {
            EntityEntry entry= Table.Update(model);
            return entry.State == EntityState.Modified;
        }
        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    }
}
