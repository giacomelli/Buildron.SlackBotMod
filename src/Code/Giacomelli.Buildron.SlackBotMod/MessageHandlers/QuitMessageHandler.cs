using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;
using Skahal.Threading;
using UnityEngine;

namespace Giacomelli.Buildron.SlackBotMod
{
	public class QuitMessageHandler : RegexMessageHandlerBase
	{
		public QuitMessageHandler(IModContext modContext, SlackService slackService)
			: base("^quit", modContext, slackService)
		{
		}

		public override string Description
		{
			get
			{
				return "quit: quits Buildron.";
			}
		}

		protected override string CreateMessage(Match match)
		{
			return "I'll be back!";
		}

		public override bool Process(Message message)
		{
			var result = base.Process(message);

			if (result)
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					Application.Quit();
				});
			}

			return result;
		}
	}
}

