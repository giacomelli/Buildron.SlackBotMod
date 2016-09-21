using System;
using System.Text.RegularExpressions;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;

namespace Giacomelli.Buildron.SlackBotMod
{
	public class ResetFilterMessageHandler : RegexMessageHandlerBase
	{
		public ResetFilterMessageHandler(IModContext modContext, SlackService slackService)
			: base("^reset filter$", modContext, slackService)
		{
		}

		public override string Description
		{
			get
			{
				return "reset filter: resets the builds filter.";
			}
		}

		protected override IRemoteControlCommand CreateCommand(Match match)
		{
			var cmd = new FilterBuildsRemoteControlCommand();
			cmd.FailedEnabled = true;
			cmd.QueuedEnabled = true;
			cmd.RunningEnabled = true;
			cmd.SuccessEnabled = true;
			cmd.KeyWord = String.Empty;

			return cmd;
		}
	
		protected override string CreateMessage(Match match)
		{
			return "Filter reseted. Now you see!";
		}
	}
}

