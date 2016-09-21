using System;
using System.Text.RegularExpressions;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;

namespace Giacomelli.Buildron.SlackBotMod
{
	public abstract class RegexMessageHandlerBase : MessageHandlerBase
	{
		private Regex m_regex;

		protected RegexMessageHandlerBase(string pattern, IModContext modContext, SlackService slackService)
			: base(modContext, slackService)
		{
			m_regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}

		public override bool Process(Message message)
		{
			var match = m_regex.Match(message.Text);

			if (match.Success)
			{
				var cmd = CreateCommand(match);

				if (cmd != null)
				{
					Log.Debug("Remote control receiving command...");
					UnityMainThreadDispatcher.Instance().Enqueue(() =>
					{
						ModContext.RemoteControl.ReceiveCommand(cmd);
					});
				}

				var msg = CreateMessage(match);

				if (!String.IsNullOrEmpty(msg))
				{
					Slack.Respond(msg);
				}

				return true;
			}

			return false;
		}

		protected virtual IRemoteControlCommand CreateCommand(Match match)
		{
			return null;
		}

		protected virtual string CreateMessage(Match match)
		{
			return null;
		}
	}
}