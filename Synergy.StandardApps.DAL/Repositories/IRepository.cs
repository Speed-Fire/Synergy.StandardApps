using Microsoft.EntityFrameworkCore.Storage;
using Synergy.StandardApps.DAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.DAL.Repositories
{
    public interface IRepository<T>
    {
        Task Create(T entity, bool save = true);
        Task<T> Update(T entity, bool save = true);
        Task Delete(T entity, bool save = true);
        IQueryable<T> GetAll();

        Task BeginChanges();

        Task<bool> Save();
    }

    public abstract class BaseRepository<T> : IRepository<T>
    {
        private Stack<IDbContextTransaction> transactions;
        protected AppDbContext context;

        protected BaseRepository(AppDbContext _context)
        {
            context = _context;
            transactions = new();
        }

        public abstract Task Create(T entity, bool save = true);
        public abstract Task Delete(T entity, bool save = true);
        public abstract IQueryable<T> GetAll();
        public abstract Task<T> Update(T entity, bool save = true);

        public async Task BeginChanges()
        {
            var transaction = await context.Database.BeginTransactionAsync();

            transactions.Push(transaction);
        }

        public async Task<bool> Save()
        {
            await context.SaveChangesAsync();

            if (transactions.Count == 0) return true;

            var transaction = transactions.Pop();
            try
            {
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction .RollbackAsync();
                return false;
            }
        }
    }
}
