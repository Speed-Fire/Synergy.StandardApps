using Grpc.Net.Client;
using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Utility.Converters;
using Synergy.StandardApps.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Grpc.Core.Metadata;

namespace Synergy.StandardApps.WorkerInteractor.Interactors
{
    public class AlarmServiceInteractor : IServiceInteractor<AlarmRecord>
    {
        private readonly IConverter<AlarmRecord, AlarmRequest> _converter;

        public AlarmServiceInteractor(IConverter<AlarmRecord, AlarmRequest> converter)
        {
            _converter = converter;
        }

        public async Task Add(AlarmRecord entity)
        {
            var url = Properties.Resources.StandardApps_Service_gRPC_address;

            using var channel = GrpcChannel.ForAddress(url);

            var client = new AlarmAdder.AlarmAdderClient(channel);
            
            var reply = await client.AddAlarmAsync(_converter.Convert(entity));
        }

        public async Task Delete(long id)
        {
            var url = Properties.Resources.StandardApps_Service_gRPC_address;

            using var channel = GrpcChannel.ForAddress(url);

            var client = new AlarmAdder.AlarmAdderClient(channel);

            var reply = await client.DeleteAlarmAsync(new() { Id = id });
        }

        public Task Update(AlarmRecord entity)
        {
            return Add(entity);
        }

        public async Task Enable(AlarmRecord entity)
        {
            var url = Properties.Resources.StandardApps_Service_gRPC_address;

            using var channel = GrpcChannel.ForAddress(url);

            var client = new AlarmAdder.AlarmAdderClient(channel);

            var _alarm = _converter.Convert(entity);

            var reply = await client.EnableAlarmAsync(new()
            {
                Alarm = _alarm,
                IsEnabled = entity.IsEnabled
            });
        }
    }
}
