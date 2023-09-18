using Synergy.StandardApps.Utility.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.WorkerIntercator
{
    public static class StandardAppsServiceSetuper
    {
        private static readonly string _serviceName = "Synergy.StandardApps.Service";

        /// <summary>
        /// Setups Synergy.StandardApps.Service.
        /// </summary>
        /// <returns>True if setuped, false if was already setuped.</returns>
        public static bool Setup()
        {
            if(IsServiceAlreadySetuped()) return false;

            SetupService();

            return true;
        }

        private static bool IsServiceAlreadySetuped()
        {
            var res = ProgramExecuter
                .Execute("sc.exe", true, out _, out _, 2000, "query", $"\"{_serviceName}\"");

            if (res == 0)
                return true;

            if (res == -1)
                throw new TimeoutException();

            return false;
        }

        private static bool SetupService()
        {
            var servicePath = Path.Combine(Directory.GetCurrentDirectory(),
                "Services", "Synergy.StandardApps.Worker.exe");

            var url = Properties.Resources.StandardApps_Service_gRPC_address;

            var res = ProgramExecuter
                .Execute("sc.exe",
                         true,
                         out _,
                         out _,
                         2000,
                         "create",
                         $"\"{_serviceName}\"",
                         $"binpath=\"{servicePath} --urls \"{url}\"\"",
                         "start=auto");

            if (res == 0) return true;

            if (res == -1)
                throw new TimeoutException();

            return false;
        }
    }
}
