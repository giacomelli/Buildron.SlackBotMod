using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;
using UnityEngine;

namespace Giacomelli.Buildron.SlackBot
{
	public class MoveCameraMessageHandler : RegexMessageHandlerBase
	{
		public MoveCameraMessageHandler(IModContext modContext, SlackService slackService)
			: base("^move camera (?<x>-*\\d+),\\s*(?<y>-*\\d+),\\s*(?<z>-*\\d+)$", modContext, slackService)
		{
		}

		public override string Description
		{
			get
			{
				return "move camera <x,y,x>: move the camera the amount of pixels define in the x,y,z coordinates.";
			}
		}

		protected override IRemoteControlCommand CreateCommand(Match match)
		{
			var x = float.Parse(match.Groups["x"].Value, CultureInfo.InvariantCulture);
			var y = float.Parse(match.Groups["y"].Value, CultureInfo.InvariantCulture);
			var z = float.Parse(match.Groups["z"].Value, CultureInfo.InvariantCulture);
			return new MoveCameraRemoteControlCommand(new Vector3(x, y, z));
		}

		protected override string CreateMessage(Match match)
		{
			return "Ok, moving camera.";
		}
	}
}

