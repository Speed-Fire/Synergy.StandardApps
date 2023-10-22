using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace Synergy.StandardApps.Misc.Notifications
{
	internal class SoundTestNotifier : INotifier<string>
	{
		private static readonly int notificationId = 188035131;

		public void Notify(string entity)
		{
			var builder = new ToastContentBuilder();

			builder
				.SetToastScenario(ToastScenario.Reminder)
				.AddArgument("action", "soundTest")
				.AddArgument("soundTestId", notificationId)
				.AddText("Alarm sound test")
				.AddButton(new ToastButton()
				{
					ActivationOptions = new()
					{
						AfterActivationBehavior = ToastAfterActivationBehavior.Default
					}
				}
					.SetContent("Stop")
					.AddArgument("action", "stop"));

			var audio = new ToastAudio()
			{
				Loop = true,
				Silent = false,
				Src = new Uri(entity)
			};

			builder.AddAudio(audio);

			builder
				.Show(toast =>
				{
					toast.ExpirationTime = DateTime.Now.AddMinutes(1);
					toast.ExpiresOnReboot = true;
					toast.Priority = ToastNotificationPriority.High;
				});
		}
	}
}
