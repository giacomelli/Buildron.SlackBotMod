using System;
using System.Text.RegularExpressions;
using Buildron.Domain.Builds;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;
using Skahal.Logging;
using UnityEngine;

namespace Giacomelli.Buildron.SlackBot
{
	public class BotController : MonoBehaviour
	{
		#region Fields
		private static readonly Regex s_filterByRegex = new Regex("filter by (?<kind>\\S+) (?<value>.+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private IModContext m_modContext;
		private ISHLogStrategy m_log;
		private bool m_notifyRunning;
		private bool m_notifyFailed;
		private bool m_notifySuccess;
		private SlackService m_slack;
		#endregion

		#region Methods
		private void Start()
		{
			m_slack = new SlackService(Mod.Context);
			m_modContext = Mod.Context;
			m_log = m_modContext.Log;
		
			if (m_slack.CanSend)
			{
				ReadPreferences();
				ListenToBuildStatusChanged();
				m_slack.MessageReceivedCallback += (msg) => {
					RespondToMessage(msg);
				};
			}
		}

		private void ReadPreferences()
		{
			var prefs = m_modContext.Preferences;
			m_notifyRunning = prefs.GetValue<bool>("NotifyRunning");
			m_notifyFailed = prefs.GetValue<bool>("NotifyFailed");
			m_notifySuccess = prefs.GetValue<bool>("NotifySuccess");
		}

		void ListenToBuildStatusChanged()
		{
			m_modContext.BuildStatusChanged += (sender, e) =>
			{
				var b = e.Build;

				if ((m_notifyRunning && b.Status == BuildStatus.Running)
					|| (m_notifyFailed && b.IsFailed())
					|| (m_notifySuccess && b.IsSuccess()))
				{
					m_slack.Send("{0} - {1}: {2}", b.Configuration.Project.Name, b.Configuration.Name, b.Status.ToString().ToLowerInvariant());
				}
			};
		}

		void RespondToMessage(SlackMessage msg)
		{
			var match = s_filterByRegex.Match(msg.Text);

			if (match.Success)
			{
				var filterCmd = new FilterBuildsRemoteControlCommand();
				var kind = match.Groups["kind"].Value;
				var value = match.Groups["value"].Value;
				filterCmd.FailedEnabled = value.Equals("failed", StringComparison.OrdinalIgnoreCase);
				filterCmd.QueuedEnabled = value.Equals("queued", StringComparison.OrdinalIgnoreCase);
				filterCmd.RunningEnabled = value.Equals("running", StringComparison.OrdinalIgnoreCase);
				filterCmd.SuccessEnabled = value.Equals("success", StringComparison.OrdinalIgnoreCase);
				filterCmd.KeyWord = String.Empty;

				m_log.Debug("Remote control receiving command...");

				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					m_modContext.RemoteControl.ReceiveCommand(filterCmd);
				});

				m_slack.Send("Ok, take a look on my screen right now! I'm filtering builds by {0} '{1}'. ", kind, value);
			}
			else
			{
				m_slack.Send("Sorry, I didn't understand what you said :(");
			}
		}
		#endregion
	}
}