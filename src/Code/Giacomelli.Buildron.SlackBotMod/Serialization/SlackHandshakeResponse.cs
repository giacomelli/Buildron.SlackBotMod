using System;
using System.Collections.Generic;

namespace Giacomelli.Buildron.SlackBotMod
{
	[Serializable]
	public class SlackHandshakeResponse
	{
		public string Url { get; private set; }

		public SlackUserResponse Self { get; private set; }

		public static SlackHandshakeResponse Parse(string json)
		{
			var data = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;

			return new SlackHandshakeResponse
			{
				Url = data["url"] as string,
				Self = SlackUserResponse.Parse(data)
			};
		}
	}	
}