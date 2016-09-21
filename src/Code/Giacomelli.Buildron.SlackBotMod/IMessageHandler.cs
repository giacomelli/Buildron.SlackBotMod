using System;
namespace Giacomelli.Buildron.SlackBotMod
{
	public interface IMessageHandler
	{
		string Description { get; }
		bool Process(Message message);
	}
}

