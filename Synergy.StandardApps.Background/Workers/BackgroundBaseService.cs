using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Background.Workers
{
    public abstract class BackgroundBaseService : BackgroundService
    {
        private readonly string SERVICE_NAME;
        private readonly ILogger<BackgroundBaseService> _logger;

        protected BackgroundBaseService(ILogger<BackgroundBaseService> logger, 
            string sERVICE_NAME)
        {
            SERVICE_NAME = sERVICE_NAME;
            this._logger = logger;
        }

        protected void LogInformation(string info)
        {
            _logger
                .LogInformation("[{service_name}]: {info}", SERVICE_NAME, info);
        }
    }
}
