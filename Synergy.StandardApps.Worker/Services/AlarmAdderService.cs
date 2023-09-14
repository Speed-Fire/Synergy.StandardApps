using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Utility.Converters;
using Synergy.StandardApps.Worker;
using Synergy.StandardApps.Worker.Messages;

namespace Synergy.StandardApps.Worker.Services
{
    public class AlarmAdderService : AlarmAdder.AlarmAdderBase
    {
        private readonly ILogger<AlarmAdderService> _logger;
        private readonly IConverter<AlarmRequest, AlarmRecord> _alarmRecordConverter;

        public AlarmAdderService(ILogger<AlarmAdderService> logger,
            IConverter<AlarmRequest, AlarmRecord> alarmRecordConverter)
        {
            _logger = logger;
            _alarmRecordConverter = alarmRecordConverter;
        }

        public override Task<AlarmReply> AddAlarm(AlarmRequest request, ServerCallContext context)
        {
            WeakReferenceMessenger.Default
                .Send(new AddAlarmMessage(_alarmRecordConverter.Convert(request)));

            return Task.FromResult(new AlarmReply());
        }

        public override Task<AlarmReply> DeleteAlarm(AlarmDeleteRequest request, ServerCallContext context)
        {
            WeakReferenceMessenger.Default
                .Send(new DeleteAlarmMessage(request.Id));

            return Task.FromResult(new AlarmReply());
        }
    }
}