using System;
using System.Text.RegularExpressions;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;

namespace Giacomelli.Buildron.SlackBot
{
	public class ResetFilterMessageHandler : MessageHandlerBase
	{
		public ResetFilterMessageHandler(IModContext modContext, SlackService slackService)
			: base(modContext, slackService)
		{
		}

		public override bool Process(Message message)
		{
			if (message.Text.Equals("reset filter", StringComparison.OrdinalIgnoreCase))
			{
				var cmd = new FilterBuildsRemoteControlCommand();
				cmd.FailedEnabled = true;
				cmd.QueuedEnabled = true;
				cmd.RunningEnabled = true;
				cmd.SuccessEnabled = true;
				cmd.KeyWord = String.Empty;

				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					ModContext.RemoteControl.ReceiveCommand(cmd);
				});

				Slack.Respond("Filter reseted. Now you see!");

				return true;
			}

			return false;
		}
	}
}

