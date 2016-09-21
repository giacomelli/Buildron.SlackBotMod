using System;
using System.Collections.Generic;
using System.Linq;

namespace Giacomelli.Buildron.SlackBotMod
{
	[Serializable]
	public class SlackUserResponse
	{
		public string Id { get; private set; }
		public string Name { get; private set; }
		public IEnumerable<SlackChannelResponse> Channels { get; private set; }
		public IEnumerable<SlackDirectMessageResponse> DMs { get; private set; }

		internal static SlackUserResponse Parse(Dictionary<string, object> data)
		{
			var selfData = data["self"] as Dictionary<string, object>;

			var r = new SlackUserResponse
			{
				 Id = selfData["id"] as string,
				 Name = selfData["name"] as string
			};

			r.Channels = SlackChannelResponse.ParseMany(data);
			r.DMs = SlackDirectMessageResponse.ParseMany(data);

			return r;
		}
	}
}

