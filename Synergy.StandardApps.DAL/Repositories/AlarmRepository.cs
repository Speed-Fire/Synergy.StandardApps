using Synergy.StandardApps.DAL.DbContexts;
using Synergy.StandardApps.Domain.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.DAL.Repositories
{
    public class AlarmRepository : BaseRepository<AlarmRecord>
    {
        public AlarmRepository(AppDbContext context) : base(context)
        {

        }

        public override async Task Create(AlarmRecord entity, bool save = true)
        {
            entity.Created = entity.Updated = DateTime.Now;

            await context.Alarms.AddAsync(entity);
            if (save)
                await context.SaveChangesAsync();
        }

        public override async Task Delete(AlarmRecord entity, bool save = true)
        {
            context.Alarms.Remove(entity);
            if (save)
                await context.SaveChangesAsync();
        }

        public override IQueryable<AlarmRecord> GetAll()
        {
            return context.Alarms.AsQueryable();
        }

        public override async Task<AlarmRecord> Update(AlarmRecord entity, bool save = true)
        {
            entity.Updated = DateTime.Now;

            context.Alarms.Update(entity);
            if (save)
                await context.SaveChangesAsync();

            return entity;
        }
    }
}
