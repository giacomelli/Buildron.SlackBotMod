using System;
using System.Collections.Generic;

namespace Giacomelli.Buildron.SlackBot
{
	[Serializable]
	public class SlackHandshakeResponse
	{
		public string url;

		public SlackUserResponse self;

		public static SlackHandshakeResponse Parse(string json)
		{
			var data = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;

			return new SlackHandshakeResponse
			{
				url = data["url"] as string,
				self = SlackUserResponse.Parse(data["self"] as Dictionary<string, object>)
			};
		}
	}	
}