using System;
using Buildron.Domain.Mods;
using Skahal.Logging;

namespace Giacomelli.Buildron.SlackBot
{
	public abstract class MessageHandlerBase : IMessageHandler
	{
		protected MessageHandlerBase(IModContext modContext, SlackService slackService)
		{
			ModContext = modContext;
			Log = modContext.Log;
			Slack = slackService;
		}

		protected IModContext ModContext { get; private set; }

		protected ISHLogStrategy Log { get; private set; }

		protected SlackService Slack { get; private set; }

		public abstract string Description { get; }

		public abstract bool Process(Message message);
	}
}

