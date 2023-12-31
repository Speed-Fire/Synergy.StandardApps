﻿using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Synergy.StandardApps.Background.Messages.Alarm;
using Synergy.StandardApps.DAL.Repositories;
using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Domain.Responses;
using Synergy.StandardApps.EntityForms.Alarm;
using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.Service.Exceptions;
using Synergy.StandardApps.Utility.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Service.Alarm
{
    public class AlarmService : IAlarmService
    {
        private readonly IRepository<AlarmRecord> _alarmRepository;
        private readonly IConverter<AlarmRecord, AlarmForm> _converter;

        public AlarmService(IRepository<AlarmRecord> alarmRepository,
            IConverter<AlarmRecord, AlarmForm> converter)
        {
            _alarmRepository = alarmRepository;
            _converter = converter;
        }

        public async Task<IResponse<AlarmForm>> CreateAlarm(AlarmCreationForm form)
        {
            try
            {
                form.Validate();
                if (form.HasErrors)
                    throw new InvalidFormException();

                var alarm = await _alarmRepository
                    .GetAll()
                    .FirstOrDefaultAsync(a => a.Time == form.Time && (a.DayMask & form.DayMask) > 0);

                if (alarm is not null)
                    throw new AlarmTimeIsAlreadyTakenException();

				alarm = new AlarmRecord()
                {
                    Name = form.Name,
                    Time = form.Time,
                    DayMask = form.DayMask,
                    Sound = form.Sound,
                    IsSoundEnabled = form.IsSoundEnabled,
                    IsEnabled = true
                };

                await _alarmRepository
                    .Create(alarm);

                WeakReferenceMessenger.Default
                    .Send(new AddAlarmMessage(alarm));

                return ResponseFactory.OK(_converter.Convert(alarm));
            }
            catch (Exception ex)
            {
                return ResponseFactory.BadResponse<AlarmForm>(ex);
            }
        }

        public async Task<IResponse<AlarmForm>> UpdateAlarm(AlarmCreationForm form, long id)
        {
            try
            {
				form.Validate();
				if (form.HasErrors)
                    throw new InvalidFormException();

				var alarm = await _alarmRepository
					.GetAll()
					.FirstOrDefaultAsync(a => a.Id != id && a.Time == form.Time && (a.DayMask & form.DayMask) > 0);

				if (alarm is not null)
					throw new AlarmTimeIsAlreadyTakenException();

				alarm = await _alarmRepository
                    .GetAll()
                    .FirstOrDefaultAsync(a => a.Id == id)
                    ?? throw new InvalidIdException();

                alarm.Name = form.Name;
                alarm.Time = form.Time;
                alarm.DayMask = form.DayMask;
                alarm.IsEnabled = true;
                alarm.Sound = form.Sound;
                alarm.IsSoundEnabled = form.IsSoundEnabled;

                await _alarmRepository
                    .Update(alarm);

                WeakReferenceMessenger.Default
                    .Send(new AddAlarmMessage(alarm));

                return ResponseFactory.OK(_converter.Convert(alarm));
            }
            catch (Exception ex)
            {
                return ResponseFactory.BadResponse<AlarmForm>(ex);
            }
        }

        public async Task<IResponse<IEnumerable<AlarmForm>>> GetAlarms()
        {
            try
            {
                var alarms = await _alarmRepository
                    .GetAll()
                    .Select(a => _converter.Convert(a))
                    .ToListAsync();

                return ResponseFactory.OK<IEnumerable<AlarmForm>>(alarms);
            }
            catch (Exception ex)
            {
                return ResponseFactory.BadResponse<IEnumerable<AlarmForm>>(ex);
            }
        }

        public async Task<IResponse<bool>> DeleteAlarm(long id)
        {
            try
            {
                var alarm = await _alarmRepository
                    .GetAll()
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (alarm is null)
                    throw new InvalidIdException();

                await _alarmRepository
                    .Delete(alarm);

                WeakReferenceMessenger.Default
                    .Send(new DeleteAlarmMessage(id));

                return ResponseFactory.OK(true);
            }
            catch (Exception ex)
            {
                return ResponseFactory.BadResponse<bool>(ex);
            }
        }

        public async Task<IResponse<bool>> SwitchAlarmEnability(long id, bool isEnabled)
        {
            try
            {
                var alarm = await _alarmRepository
                    .GetAll()
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (alarm is null)
                    throw new InvalidIdException();

                alarm.IsEnabled = isEnabled;

                await _alarmRepository
                    .Update(alarm);

                WeakReferenceMessenger.Default
                    .Send(new EnableAlarmMessage(alarm));

                return ResponseFactory.OK(true);
            }
            catch (Exception ex)
            {
                return ResponseFactory.BadResponse<bool>(ex);
            }
        }
    }
}
