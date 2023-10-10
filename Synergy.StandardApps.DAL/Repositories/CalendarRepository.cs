using Synergy.StandardApps.DAL.DbContexts;
using Synergy.StandardApps.Domain.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.DAL.Repositories
{
    public class CalendarRepository : BaseRepository<CalendarEvent>
    {
        public CalendarRepository(AppDbContext context) : base(context)
        {

        }

        public override async Task Create(CalendarEvent entity, bool save = true)
        {
            entity.Created = entity.Updated = DateTime.Now;

            await context.CalendarEvents.AddAsync(entity);
            if (save)
                await context.SaveChangesAsync();
        }

        public override async Task Delete(CalendarEvent entity, bool save = true)
        {
            context.CalendarEvents.Remove(entity);
            if (save)
                await context.SaveChangesAsync();
        }

        public override IQueryable<CalendarEvent> GetAll()
        {
            return context.CalendarEvents.AsQueryable();
        }

        public override async Task<CalendarEvent> Update(CalendarEvent entity, bool save = true)
        {
            entity.Updated = DateTime.Now;

            context.CalendarEvents.Update(entity);
            if (save)
                await context.SaveChangesAsync();

            return entity;
        }
    }
}
