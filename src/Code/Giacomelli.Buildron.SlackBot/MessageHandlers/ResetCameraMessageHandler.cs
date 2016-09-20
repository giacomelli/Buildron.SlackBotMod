using System;
using System.Text.RegularExpressions;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;

namespace Giacomelli.Buildron.SlackBot
{
	public class ResetCameraMessageHandler : RegexMessageHandlerBase
	{
		public ResetCameraMessageHandler(IModContext modContext, SlackService slackService)
			: base("^reset camera$", modContext, slackService)
		{
		}

		public override string Description
		{
			get
			{
				return "reset camera: resets the camera position.";
			}
		}

		protected override IRemoteControlCommand CreateCommand(Match match)
		{
			return new ResetCameraRemoteControlCommand();
		}

		protected override string CreateMessage(Match match)
		{
			return "Ok, reseting camera.";
		}
	}
}

