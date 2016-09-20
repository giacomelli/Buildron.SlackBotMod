using System;
using System.Text.RegularExpressions;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;

namespace Giacomelli.Buildron.SlackBot
{
	public class ResetCameraMessageHandler : MessageHandlerBase
	{
		public ResetCameraMessageHandler(IModContext modContext, SlackService slackService)
			: base(modContext, slackService)
		{
		}

		public override bool Process(Message message)
		{
			if (message.Text.Equals("reset camera", StringComparison.OrdinalIgnoreCase))
			{
				var cmd = new ResetCameraRemoteControlCommand();
			
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					ModContext.RemoteControl.ReceiveCommand(cmd);
				});

				Slack.Respond("Ok, reseting camera.");

				return true;
			}

			return false;
		}
	}
}

