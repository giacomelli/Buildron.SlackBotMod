using System;
using System.Text;
using System.Text.RegularExpressions;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;

namespace Giacomelli.Buildron.SlackBot
{
	public class HelpMessageHandler : RegexMessageHandlerBase
	{
		public HelpMessageHandler(IModContext modContext, SlackService slackService)
			: base("^help", modContext, slackService)
		{
		}

		public override string Description
		{
			get
			{
				return "help: this help message.";
			}
		}

		protected override string CreateMessage(Match match)
		{
			var handlers = MessageHandlerService.GetMessageHandlers(ModContext, Slack);
			var msg = new StringBuilder();
			msg.AppendLine("Available messages:");

			foreach (var h in handlers)
			{
				msg.AppendLine("\t * {0}".With(h.Description));
			}

			return msg.ToString();
		}
	}
}

