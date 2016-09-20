using System;
using System.Text.RegularExpressions;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;

namespace Giacomelli.Buildron.SlackBot
{
	public class FilterByMessageHandler : MessageHandlerBase
	{
		private static readonly Regex s_filterByRegex = new Regex("filter by (?<kind>\\S+) (?<value>.+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		public FilterByMessageHandler(IModContext modContext, SlackService slackService)
			: base(modContext, slackService)
		{
		}

		public override bool Process(Message message)
		{
			var match = s_filterByRegex.Match(message.Text);

			if (match.Success)
			{
				var filterCmd = new FilterBuildsRemoteControlCommand();
				var kind = match.Groups["kind"].Value;
				var value = match.Groups["value"].Value;
				filterCmd.FailedEnabled = value.Equals("failed", StringComparison.OrdinalIgnoreCase);
				filterCmd.QueuedEnabled = value.Equals("queued", StringComparison.OrdinalIgnoreCase);
				filterCmd.RunningEnabled = value.Equals("running", StringComparison.OrdinalIgnoreCase);
				filterCmd.SuccessEnabled = value.Equals("success", StringComparison.OrdinalIgnoreCase);
				filterCmd.KeyWord = String.Empty;

				Log.Debug("Remote control receiving command...");

				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					ModContext.RemoteControl.ReceiveCommand(filterCmd);
				});

				Slack.Respond("Ok, take a look on my screen right now! I'm filtering builds by {0} '{1}'. "
				           .With(kind, value));

				return true;
			}

			return false;
		}
	}
}

