using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;
using Skahal.Threading;
using UnityEngine;

namespace Giacomelli.Buildron.SlackBotMod
{
	public class ScreenshotMessageHandler : RegexMessageHandlerBase
	{
		public ScreenshotMessageHandler(IModContext modContext, SlackService slackService)
			: base("^take a screenshot|take a selfie", modContext, slackService)
		{
		}

		public override string Description
		{
			get
			{
				return "take a screenshot: takes a screenshot from Buildron screen.";
			}
		}

		protected override string CreateMessage(Match match)
		{
			UnityMainThreadDispatcher.Instance().Enqueue(() =>
			{
				var filepath = Path.Combine(Application.dataPath, "Buildron-screenshot.png");
				Log.Debug("Saving screenshot to {0}...", filepath);
				Application.CaptureScreenshot(filepath, 2);

				SHCoroutine.Start(1, () =>
				{
					Slack.UploadFile(filepath);
				});

			});

			return "Uploading screenshot...";
		}
	}
}

