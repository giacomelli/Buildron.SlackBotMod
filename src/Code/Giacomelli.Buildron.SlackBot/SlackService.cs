using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Buildron.Domain.Mods;
using Skahal.Logging;
using Skahal.Threading;
using UnityEngine;
using WebSocketSharp;

namespace Giacomelli.Buildron.SlackBot
{
	public class SlackService
	{
		#region Fields
		private IModContext m_modContext;
		private ISHLogStrategy m_log;
		private WebSocket m_ws;
		private string m_slackKey;
		private string m_channel;
		private SlackUserResponse m_bot;
		private string m_messageToBotPrefix;
		private Message m_messageToRespond;
		#endregion

		#region Constructors
		public SlackService(IModContext modContext)
		{
			m_modContext = modContext;
			m_log = m_modContext.Log;

			ReadPreferences();
			CanSend = !String.IsNullOrEmpty(m_slackKey);

			if (!CanSend)
			{
				m_log.Warning("Will not start, because Token is empty.");
				return;
			}

			ListToUsersMessages();
		}

		#endregion

		#region Properties
		public bool CanSend { get; private set; }
		public Action<Message> MessageReceivedCallback { get; set; }
		#endregion

		private void ReadPreferences()
		{
			var prefs = m_modContext.Preferences;
			m_slackKey = prefs.GetValue<string>("Token");
			m_channel = prefs.GetValue<string>("Channel");
		}

		void ListToUsersMessages()
		{
			// https://api.slack.com/faq 
			// How do I connect to a websocket?
			m_log.Debug("Getting websocket info...");
			var url = "https://slack.com/api/rtm.start?token={0}".With(m_slackKey);
			m_log.Debug(url);
			var get = new WWW(url);
			SHCoroutine.WaitFor(() =>
			{
				return get.isDone;
			},
			() =>
			{
				m_log.Debug("Handshake response: {0}", get.text);
				var handshake = SlackHandshakeResponse.Parse(get.text);
				m_bot = handshake.Self;

				if (m_bot == null)
				{
					throw new InvalidOperationException("Handshake response without bot.");
				}

				m_messageToBotPrefix = "<@{0}> ".With(m_bot.Id);

				m_log.Debug("Bot id: {0}", m_bot.Id);
				var wss = handshake.Url;
				wss = wss.Replace("\\", string.Empty);
				m_ws = new WebSocket(wss);

				m_ws.OnOpen += (sender, e) =>
				{
					m_log.Debug("Slack realtime connection opened.");
				};

				m_ws.OnMessage += (sender, e) =>
				{
					m_log.Debug("Slack says: {0}", e.Data);
					var response = JsonUtility.FromJson<SlackResponse>(e.Data);

					if (IsMessageToBot(response))
					{
						var msg = new Message(
							response.text.Replace(m_messageToBotPrefix, string.Empty),
							response.channel);
						m_log.Debug("Message received: {0}", msg.Text);

						m_messageToRespond = msg;

						MessageReceivedCallback(msg);
					}
				};

				m_ws.OnClose += (sender, e) =>
				{
					m_log.Debug("Slack realtime connection closed: {0}", e.Reason);
				};

				m_ws.OnError += (sender, e) =>
				{
					m_log.Warning("Slack realtime error: {0}:{1}", e.Message, e.Exception.StackTrace);
				};

				m_ws.Connect();
			});
		}

		bool IsMessageToBot(SlackResponse response)
		{
			return response != null
				&& response.type.Equals("message")
				&& (
				 	response.text.StartsWith(m_messageToBotPrefix, StringComparison.Ordinal)
	   		     || m_bot.DMs.Any(dm => dm.Id == response.channel))
				 && !m_bot.Name.Equals(response.username, StringComparison.OrdinalIgnoreCase);
		}

		public void Respond(string msg)
		{
			Send(new Message(msg, m_messageToRespond));
		}

		public void SendToDefaultChannel(string msg)
		{
			Send(new Message(msg, m_channel));
		}

		/// <summary>
		/// https://api.slack.com/methods/chat.postMessage
		/// </summary>
		public void Send(Message msg)
		{
			m_log.Debug("Enqueuing message '{0}'", msg.Text);

			UnityMainThreadDispatcher.Instance().Enqueue(() =>
			{
				m_log.Debug("Sending message '{0}'...", msg.Text);
				var url = "https://slack.com/api/chat.postMessage";
				var data = new WWWForm();
				data.AddField("token", m_slackKey);
				data.AddField("channel", msg.Channel ?? m_channel);
				data.AddField("text", msg.Text);
				data.AddField("username", "Buildron");
				data.AddField("icon_url", "https://raw.githubusercontent.com/skahal/Buildron/master/docs/images/Buildron-logo-128x128.png");
				var post = new WWW(url, data);

				SHCoroutine.WaitFor(() =>
				{
					return post.isDone;
				},
				() =>
				{
					m_log.Debug("Message sent.");
				});
			});
		}

		void OnDestroy()
		{
			if (m_ws != null)
			{
				m_ws.Close();
			}
		}
	}
}

