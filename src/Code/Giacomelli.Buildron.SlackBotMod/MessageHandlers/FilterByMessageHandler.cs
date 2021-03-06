using System;
using System.Text.RegularExpressions;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;

namespace Giacomelli.Buildron.SlackBotMod
{
	public class FilterByMessageHandler : RegexMessageHandlerBase
	{
		public FilterByMessageHandler(IModContext modContext, SlackService slackService)
			: base("filter by (?<kind>(status|text)) (?<value>.+)", modContext, slackService)
		{
		}

		public override string Description
		{
			get
			{
				return @"filter by
				status failed|queued|running|success: filters the builds by specified status.
				text <text>: filters the builds by specified text.";
			}
		}

		protected override IRemoteControlCommand CreateCommand(Match match)
		{
			var cmd = new FilterBuildsRemoteControlCommand();
			var kind = match.Groups["kind"].Value;
			var value = match.Groups["value"].Value;

			if (kind.Equals("status", StringComparison.Ordinal))
			{
				cmd.FailedEnabled = value.Equals("failed", StringComparison.OrdinalIgnoreCase);
				cmd.QueuedEnabled = value.Equals("queued", StringComparison.OrdinalIgnoreCase);
				cmd.RunningEnabled = value.Equals("running", StringComparison.OrdinalIgnoreCase);
				cmd.SuccessEnabled = value.Equals("success", StringComparison.OrdinalIgnoreCase);
				cmd.KeyWord = String.Empty;
			}
			else {
				cmd.KeyWord = value;
			}

			return cmd;
		}

		protected override string CreateMessage(Match match)
		{
			var kind = match.Groups["kind"].Value;
			var value = match.Groups["value"].Value;

			return "Ok, take a look on my screen right now! I'm filtering builds by {0} '{1}'. "
				    .With(kind, value);
		}
	}
}

