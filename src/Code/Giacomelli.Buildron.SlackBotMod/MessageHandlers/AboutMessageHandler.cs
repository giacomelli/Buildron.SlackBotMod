using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;

namespace Giacomelli.Buildron.SlackBotMod
{
	public class AboutMessageHandler : RegexMessageHandlerBase
	{
		public AboutMessageHandler(IModContext modContext, SlackService slackService)
			: base("^about", modContext, slackService)
		{
		}

		public override string Description
		{
			get
			{
				return "about: informations about this mod.";
			}
		}

		protected override string CreateMessage(Match match)
		{
			return "Buildron.SlackBotMod v{0} (http://github.com/giacomelli/Buildron.SlackBotMod)"
				.With(GetType().Assembly.GetName().Version);
		}
	}
}

