using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Misc
{
    public interface IEntrypointInfo
    {
		String CommandLine { get; }

		IReadOnlyList<String> CommandLineArgs { get; }

		// Default interface implementation, requires C# 8.0 or later:
		Boolean HasFlag(String flagName)
		{
			return this.CommandLineArgs.Any(a => ("-" + flagName) == a || ("/" + flagName) == a);
		}
	}

	public class SystemEnvironmentEntrypointInfo : IEntrypointInfo
	{
		public String CommandLine => System.Environment.CommandLine;

		public IReadOnlyList<String> CommandLineArgs => System.Environment.GetCommandLineArgs();
	}
}
