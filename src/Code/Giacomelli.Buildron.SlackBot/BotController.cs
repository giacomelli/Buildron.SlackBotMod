using System;
using System.Collections;
using System.Text.RegularExpressions;
using Buildron.Domain.Builds;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;
using Skahal.Logging;
using Skahal.Threading;
using UnityEngine;
using WebSocketSharp;

namespace Giacomelli.Buildron.SlackBot
{
	public class BotController : MonoBehaviour
	{
		#region Fields
		private static readonly Regex s_filterByRegex = new Regex("filter by (?<kind>\\S+) (?<value>.+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private IModContext m_modContext;
		private ISHLogStrategy m_log;
		private string m_slackKey;
		private string m_channel;
		private bool m_notifyRunning;
		private bool m_notifyFailed;
		private bool m_notifySuccess;
		#endregion

		#region Methods
		private void Start()
		{
			m_modContext = Mod.Context;
			m_log = m_modContext.Log;

			ReadPreferences();

			if (String.IsNullOrEmpty(m_slackKey))
			{
				m_log.Warning("Will not start, because Token is empty.");
				return;
			}

			ListenToBuildStatusChanged();
			ListToUsersMessages();
		}

		private void ReadPreferences()
		{
			var prefs = m_modContext.Preferences;
			m_slackKey = prefs.GetValue<string>("Token");
			m_channel = prefs.GetValue<string>("Channel");
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
					Send("{0} - {1}: {2}", b.Configuration.Project.Name, b.Configuration.Name, b.Status.ToString().ToLowerInvariant());
				}
			};
		}

		void ListToUsersMessages()
		{
			// https://api.slack.com/faq 
			// How do I connect to a websocket?
			m_log.Debug("Getting websocket info...");
			var url = "https://slack.com/api/rtm.start?token={0}".With(m_slackKey);
			var get = new WWW(url);
			SHCoroutine.WaitFor(() =>
			{
				return get.isDone;
			},
			() =>
			{
				var urlRegex = new Regex("\"url\":\"(.+)\"");
				var wss = urlRegex.Match(get.text).Groups[1].Value;
				wss = wss.Replace("\\", string.Empty);
				var ws = new WebSocket(wss);

				ws.OnOpen += (sender, e) =>
				{
					m_log.Debug("Slack realtime connection opened.");
				};

				ws.OnMessage += (sender, e) =>
				{
					m_log.Debug("Slack says: {0}", e.Data);
					RespondToMessage(e.Data);
				};

				ws.OnClose += (sender, e) =>
				{
					m_log.Debug("Slack realtime connection closed: {0}", e.Reason);
				};

				ws.OnError += (sender, e) =>
				{
					m_log.Error("Slack realtime error: {0}.", e.Message);
				};

				ws.Connect();
			});
		}

		void RespondToMessage(string data)
		{
			var match = s_filterByRegex.Match(data);

			if (match.Success)
			{
				var filterCmd = new FilterBuildsRemoteControlCommand();
				var kind = match.Groups["kind"].Value;
				var value = match.Groups["value"].Value;
				filterCmd.FailedEnabled = value.Equals("failed", StringComparison.OrdinalIgnoreCase);
				filterCmd.QueuedEnabled = value.Equals("queued", StringComparison.OrdinalIgnoreCase);
				filterCmd.RunningEnabled = value.Equals("running", StringComparison.OrdinalIgnoreCase);
				filterCmd.SuccessEnabled = value.Equals("success", StringComparison.OrdinalIgnoreCase);

				m_modContext.RemoteControl.ReceiveCommand(filterCmd);
				//Send("filtering by {0} {1}", kind, value);
			}
			else 
			{
				//Send("Sorry, I didn't understand what you said :(");
			}
		}

		/// <summary>
		/// https://api.slack.com/methods/chat.postMessage
		/// </summary>
		private void Send(string message, params object[] args)
		{
			var formattedMsg = message.With(args);
			m_log.Debug("Sending message '{0}'...", formattedMsg);
			var url = "https://slack.com/api/chat.postMessage";
			var data = new WWWForm();
			data.AddField("token", m_slackKey);
			data.AddField("channel", m_channel);
			data.AddField("text", formattedMsg);
			data.AddField("username", "Buildron");
			data.AddField("icon_url", "https://raw.githubusercontent.com/skahal/Buildron/master/docs/images/Buildron-logo-128x128.png");
			var post = new WWW(url, data);

			StartCoroutine(WaitForPost(post));
		}

		private IEnumerator WaitForPost(WWW post)
		{
			yield return post;

			m_log.Debug("Message sent.");
		}
		#endregion
	}
}