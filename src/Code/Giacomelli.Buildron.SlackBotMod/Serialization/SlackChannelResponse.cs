using System;
using System.Collections.Generic;

namespace Giacomelli.Buildron.SlackBotMod
{
	public class SlackChannelResponse
	{
		public string Id { get; private set; }

		public string Name { get; private set; }

		internal static IEnumerable<SlackChannelResponse> ParseMany(Dictionary<string, object> data)
		{
			var r = new List<SlackChannelResponse>();
			var channels = data["channels"] as List<object>;

			foreach (Dictionary<string, object> c in channels)
			{
				r.Add(Parse(c));
			}

			return r;
		}

		internal static SlackChannelResponse Parse(Dictionary<string, object> data)
		{
			return new SlackChannelResponse
			{
				Id = data["id"] as string,
				Name = data["name"]  as string
			};
		}
	}
}