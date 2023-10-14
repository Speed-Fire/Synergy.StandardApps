using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Synergy.StandardApps.DAL.Repositories;
using Synergy.StandardApps.Domain.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Background.Workers
{
    internal class BackgroundNoteCleanerService : BackgroundBaseService
    {
        #region Readonly

        private readonly IServiceProvider _serviceProvider;

        private readonly int _sleepDelay;

        #endregion

        private int _maxNoteAge;
        private DateTime _lastDay;

        public BackgroundNoteCleanerService(IServiceProvider serviceProvider,
            ILogger<BackgroundCalendarService> logger) :
            base(logger, nameof(BackgroundNoteCleanerService))
        {
            _serviceProvider = serviceProvider;

            _lastDay = DateTime.Now.AddDays(-1);

            _sleepDelay = 1000 * 60 * 60;
            _maxNoteAge = 30;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            LogInformation("started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var date = DateTime.Now;

                if (_lastDay.AddDays(1).Date.Equals(date.Date))
                {
                    LoadSettings();

                    await ClearOldNotes(date);

                    _lastDay = date;
                }

                await Task.Delay(_sleepDelay, stoppingToken);
            }

            LogInformation("finished.");
        }

        private void LoadSettings()
        {
            _maxNoteAge = Settings.Properties.NoteMaxAge;
        }

        private async Task ClearOldNotes(DateTime dt)
        {
            var notesRepository = _serviceProvider
                .GetRequiredService<IRepository<Note>>();

            var notesToDel = await notesRepository
                .GetAll()
                .Where(n => n.Updated.Date.AddDays(_maxNoteAge) < dt.Date)
                .ToListAsync();

            if(notesToDel.Count == 0)
                return;

            await notesRepository.BeginChanges();

            foreach ( var note in notesToDel)
            {
                await notesRepository.Delete(note, false);
            }

            var res = await notesRepository.Save();

            if (res)
                LogInformation($"notes older than {_maxNoteAge} days have been deleted.");
            else
                LogInformation("something went wrong during old note deletion.");
        }
    }
}
