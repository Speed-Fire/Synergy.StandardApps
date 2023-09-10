using Synergy.StandardApps.DAL.DbContexts;
using Synergy.StandardApps.Domain.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.DAL.Repositories
{
    public class AlarmRepository : IRepository<AlarmRecord>
    {
        private readonly AppDbContext context;

        public AlarmRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Create(AlarmRecord entity)
        {
            entity.Created = DateTime.Now;

            await context.Alarms.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(AlarmRecord entity)
        {
            context.Alarms.Remove(entity);
            await context.SaveChangesAsync();
        }

        public IQueryable<AlarmRecord> GetAll()
        {
            return context.Alarms.AsQueryable();
        }

        public async Task<AlarmRecord> Update(AlarmRecord entity)
        {
            entity.Updated = DateTime.Now;

            context.Alarms.Update(entity);
            await context.SaveChangesAsync();

            return entity;
        }
    }
}
