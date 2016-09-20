using System;
namespace Giacomelli.Buildron.SlackBot
{
	public interface IMessageHandler
	{
		string Description { get; }
		bool Process(Message message);
	}
}

