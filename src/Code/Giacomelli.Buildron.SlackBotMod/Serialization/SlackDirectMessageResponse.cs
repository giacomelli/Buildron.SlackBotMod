using System;
using System.Collections.Generic;

namespace Giacomelli.Buildron.SlackBotMod
{
	public class SlackDirectMessageResponse
	{
		public string Id { get; private set; }

		public string UserId { get; private set; }

		internal static IEnumerable<SlackDirectMessageResponse> ParseMany(Dictionary<string, object> data)
		{
			var r = new List<SlackDirectMessageResponse>();
			var dms = data["ims"] as List<object>;

			foreach (Dictionary<string, object> c in dms)
			{
				r.Add(Parse(c));
			}

			return r;
		}

		internal static SlackDirectMessageResponse Parse(Dictionary<string, object> data)
		{
			return new SlackDirectMessageResponse
			{
				Id = data["id"] as string,
				UserId = data["user"]  as string
			};
		}
	}
}