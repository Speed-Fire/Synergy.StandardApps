using Synergy.StandardApps.DAL.DbContexts;
using Synergy.StandardApps.Domain.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.DAL.Repositories
{
    public class NotesRepository : IRepository<Note>
    {
        private readonly AppDbContext context;

        public NotesRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Create(Note entity)
        {
            entity.Created = DateTime.Now;

            await context.Notes.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Note entity)
        {
            context.Notes.Remove(entity);
            await context.SaveChangesAsync();
        }

        public IQueryable<Note> GetAll()
        {
            return context.Notes.AsQueryable();
        }

        public async Task<Note> Update(Note entity)
        {
            entity.Updated = DateTime.Now;

            context.Notes.Update(entity);
            await context.SaveChangesAsync();

            return entity;
        }
    }
}
