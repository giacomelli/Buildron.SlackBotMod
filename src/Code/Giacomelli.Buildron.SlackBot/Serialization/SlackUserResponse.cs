using System;
using System.Collections.Generic;

namespace Giacomelli.Buildron.SlackBot
{
	[Serializable]
	public class SlackUserResponse
	{
		public string id;
		public string name;

		internal static SlackUserResponse Parse(Dictionary<string, object> data)
		{	
			return new SlackUserResponse
			{
				 id = data["id"] as string,
				 name = data["name"] as string
			};
		}
	}
}

