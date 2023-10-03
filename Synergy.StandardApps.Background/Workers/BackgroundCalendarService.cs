using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Hosting;
using Synergy.StandardApps.Background.Messages.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Background.Workers
{
    public class BackgroundCalendarService
        :
        BackgroundService,
        IRecipient<AddCalendarEventMessage>,
        IRecipient<DeleteCalendarEventMessage>
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        #region Messages

        void IRecipient<AddCalendarEventMessage>.Receive(AddCalendarEventMessage message)
        {
            throw new NotImplementedException();
        }

        void IRecipient<DeleteCalendarEventMessage>.Receive(DeleteCalendarEventMessage message)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
