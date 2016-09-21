using System;
namespace Giacomelli.Buildron.SlackBotMod
{
	[Serializable]
	public class SlackResponse
	{
		public string type;
		public string subtype;
		public string text;
		public string username;
		public string channel;
	}
}

