using Synergy.StandardApps.DAL.DbContexts;
using Synergy.StandardApps.Domain.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.DAL.Repositories
{
    public class NotesRepository : BaseRepository<Note>
    {
        public NotesRepository(AppDbContext context) : base(context)
        {

        }

        public override async Task Create(Note entity, bool save = true)
        {
            entity.Created = entity.Updated = DateTime.Now;

            await context.Notes.AddAsync(entity);
            if (save)
                await context.SaveChangesAsync();
        }

        public override async Task Delete(Note entity, bool save = true)
        {
            context.Notes.Remove(entity);
            if (save)
                await context.SaveChangesAsync();
        }

        public override IQueryable<Note> GetAll()
        {
            return context.Notes.AsQueryable();
        }

        public override async Task<Note> Update(Note entity, bool save = true)
        {
            entity.Updated = DateTime.Now;

            context.Notes.Update(entity);
            if (save)
                await context.SaveChangesAsync();

            return entity;
        }
    }
}
