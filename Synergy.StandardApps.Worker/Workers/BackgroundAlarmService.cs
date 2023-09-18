using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Synergy.StandardApps.DAL.Repositories;
using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Worker.Messages;
using Synergy.StandardApps.Worker.Utility.Notifications;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Synergy.StandardApps.Worker.Workers
{
    public class BackgroundAlarmService :
        BackgroundService,
        IRecipient<AddAlarmMessage>,
        IRecipient<DeleteAlarmMessage>,
        IRecipient<EnableAlarmMessage>
    {
        #region Readonly

        private readonly ILogger<BackgroundAlarmService> _logger;
        private readonly INotifier<AlarmRecord> _notifier;

        private readonly ConcurrentQueue<AlarmRecord> _alarmsToAdd;
        private readonly ConcurrentQueue<long> _alarmsToDelete;
        private readonly ConcurrentQueue<AlarmRecord> _alarmsToChangeEnability;

        private readonly LinkedList<AlarmRecord> _alarms;

        #endregion

        private DateTime _lastDay;

        public BackgroundAlarmService(ILogger<BackgroundAlarmService> logger,
            INotifier<AlarmRecord> notifier)
        {
            _logger = logger;
            _notifier = notifier;

            _alarmsToAdd = new();
            _alarmsToDelete = new();
            _alarms = new();
            _alarmsToChangeEnability = new();

            _lastDay = DateTime.Now.Date.AddDays(-1);

            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested)
            {
                var date = DateTime.Now.Date;

                if (_lastDay.AddDays(1).Equals(date))
                {
                    await LoadAlarmsFromDb(date);

                    _lastDay = date;

                    _logger.LogInformation(
                        $"[{nameof(BackgroundAlarmService)}]: new day alarms loaded.");
                }

                HandleAlarmsEnability();
                HandleAddedAlarms(date);
                HandleDeletedAlarms();

                if(_alarms.First is null)
                {
                    await Task.Delay(1000);
                    continue;
                }

                if(_alarms.First.Value.IsTimeToAlarm(date))
                {
                    var alarm = _alarms.First.Value;
                    _alarms.Remove(alarm);

                    _notifier.Notify(alarm);

                    _logger.LogInformation(
                        $"[{nameof(BackgroundAlarmService)}]: alarm notified.");
                }
            }
        }

        private async Task LoadAlarmsFromDb(DateTime today)
        {
            using var _scope = Program.App.Services.CreateAsyncScope();

            var _alarmRecordRepository =
                _scope.ServiceProvider.GetRequiredService<IRepository<AlarmRecord>>();

            _alarms.Clear();

            var alarms = await _alarmRecordRepository
                .GetAll()
                .Where(a => a.IsEnabled)
                .ToListAsync();

            // Select only todays alarms
            var todaysAlarms = new List<AlarmRecord>();
            foreach(var alarm in alarms)
            {
                if (alarm.IsAlarmedDay(today))
                {
                    todaysAlarms.Add(alarm);
                }
            }

            // sort todays alarms by Time
            todaysAlarms.Sort((a1, a2) => { return a1.Time.CompareTo(a2); });

            // fill _alarms
            foreach (var alarm in todaysAlarms
                .Where(a => a.Time >= TimeOnly.FromDateTime(today)))
            {
                _alarms.AddLast(alarm);
            }
        }

        #region Adding/updating alarm

        private void HandleAddedAlarms(DateTime today)
        {
            while (!_alarmsToAdd.IsEmpty)
            {
                if (_alarmsToAdd.TryDequeue(out var alarm))
                {
                    var existing = _alarms.FirstOrDefault(a => a.Id == alarm.Id);

                    if(existing != null) // update
                    {
                        UpdateAlarm(alarm, existing, today);
                    }
                    else                // add
                    {
                        AddAlarm(alarm, today);
                    }
                }
            }
        }

        private void AddAlarm(AlarmRecord alarm, DateTime today)
        {
            if (!alarm.IsAlarmedDay(today))
                return;

            var time = TimeOnly.FromDateTime(today);

            if (alarm.Time < time &&
                alarm.Time.Add(TimeSpan.FromSeconds(2)) < time)
                return;

            var nextAlarm = _alarms
                .FirstOrDefault(a =>
                {
                    if(a.Time.Hour > alarm.Time.Hour)
                        return true;

                    if (a.Time.Hour < alarm.Time.Hour)
                        return false;

                    if(a.Time.Minute >= alarm.Time.Minute)
                        return true;

                    return false;
                });

            if(nextAlarm is null)
            {
                _alarms.AddLast(alarm);
            }
            else
            {
                _alarms.AddBefore(_alarms.Find(nextAlarm), alarm);
            }

            _logger.LogInformation(
                $"[{nameof(BackgroundAlarmService)}]: alarm added to queue.");
        }

        private void UpdateAlarm(AlarmRecord alarm, AlarmRecord existing, DateTime today)
        {
            void localRemoveAlarm(AlarmRecord toRem)
            {
                _alarms.Remove(toRem);

                _logger.LogInformation(
                            $"[{nameof(BackgroundAlarmService)}]: alarm deleted from queue.");
            }

            if (!alarm.IsAlarmedDay(today))
            {
                localRemoveAlarm(existing);
                return;
            }

            var time = TimeOnly.FromDateTime(today);

            if (alarm.Time < time &&
                alarm.Time.Add(TimeSpan.FromSeconds(2)) < time)
            {
                localRemoveAlarm(existing);
                return;
            }

            _alarms.Remove(existing);

            var nextAlarm = _alarms
                .FirstOrDefault(a =>
                {
                    if (a.Time.Hour > alarm.Time.Hour)
                        return true;

                    if (a.Time.Hour < alarm.Time.Hour)
                        return false;

                    if (a.Time.Minute >= alarm.Time.Minute)
                        return true;

                    return false;
                });

            if (nextAlarm is null)
            {
                _alarms.AddLast(alarm);
            }
            else
            {
                _alarms.AddBefore(_alarms.Find(nextAlarm), alarm);
            }

            _logger.LogInformation(
                $"[{nameof(BackgroundAlarmService)}]: alarm updated.");
        }

        #endregion

        private void HandleDeletedAlarms()
        {
            while (!_alarmsToDelete.IsEmpty)
            {
                if (_alarmsToDelete.TryDequeue(out var id))
                {
                    var existed = _alarms.FirstOrDefault(a => a.Id == id);

                    if (existed != null) // update
                    {
                        _alarms.Remove(existed);

                        _logger.LogInformation(
                            $"[{nameof(BackgroundAlarmService)}]: alarm deleted from queue.");
                    }
                }
            }
        }

        private void HandleAlarmsEnability()
        {
            while (!_alarmsToChangeEnability.IsEmpty)
            {
                if(_alarmsToChangeEnability.TryDequeue(out var alarm))
                {
                    if (alarm.IsEnabled)
                        _alarmsToAdd.Enqueue(alarm);
                    else
                        _alarmsToDelete.Enqueue(alarm.Id);
                }
            }
        }

        #region Messages

        public void Receive(AddAlarmMessage message)
        {
            _alarmsToAdd.Enqueue(message.Value);
        }

        public void Receive(DeleteAlarmMessage message)
        {
            _alarmsToDelete.Enqueue(message.Value);
        }

        public void Receive(EnableAlarmMessage message)
        {
            _alarmsToChangeEnability.Enqueue(message.Value);
        }

        #endregion
    }
}
