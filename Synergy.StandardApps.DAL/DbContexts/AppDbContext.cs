using Microsoft.EntityFrameworkCore;
using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Domain.Calendar;
using Synergy.StandardApps.Domain.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.DAL.DbContexts
{
    public class AppDbContext : DbContext
    {
        internal DbSet<AlarmRecord> Alarms => Set<AlarmRecord>();
        internal DbSet<CalendarEvent> CalendarEvents => Set<CalendarEvent>();
        internal DbSet<Note> Notes => Set<Note>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : 
            base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<AlarmRecord>()
                .HasIndex(a => a.Time)
                .IsUnique();

            modelBuilder
                .Entity<Note>()
                .HasIndex(n => n.Name)
                .IsUnique();
        }
    }
}
