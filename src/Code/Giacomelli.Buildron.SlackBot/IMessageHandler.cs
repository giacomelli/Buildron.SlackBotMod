using System;
namespace Giacomelli.Buildron.SlackBot
{
	public interface IMessageHandler
	{
		bool Process(SlackMessage message);
	}
}

