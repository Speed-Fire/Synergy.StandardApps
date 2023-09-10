using Synergy.StandardApps.DAL.DbContexts;
using Synergy.StandardApps.Domain.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.DAL.Repositories
{
    public class CalendarRepository : IRepository<CalendarEvent>
    {
        private readonly AppDbContext context;

        public CalendarRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Create(CalendarEvent entity)
        {
            entity.Created = DateTime.Now;

            await context.CalendarEvents.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(CalendarEvent entity)
        {
            context.CalendarEvents.Remove(entity);
            await context.SaveChangesAsync();
        }

        public IQueryable<CalendarEvent> GetAll()
        {
            return context.CalendarEvents.AsQueryable();
        }

        public async Task<CalendarEvent> Update(CalendarEvent entity)
        {
            entity.Updated = DateTime.Now;

            context.CalendarEvents.Update(entity);
            await context.SaveChangesAsync();

            return entity;
        }
    }
}
