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
		private IModContext m_modContext;
		private ISHLogStrategy m_log;
		private bool m_notifyRunning;
		private bool m_notifyFailed;
		private bool m_notifySuccess;
		private SlackService m_slack;
		private IMessageHandler[] m_messageHandlers;
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

				m_messageHandlers = new IMessageHandler[]
				{
					new FilterByMessageHandler(m_modContext, m_slack),
					new ResetFilterMessageHandler(m_modContext, m_slack),
					new ResetCameraMessageHandler(m_modContext, m_slack)
				};

				m_slack.MessageReceivedCallback += (msg) =>
				{
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

				if (b.Status == e.PreviousStatus)
				{
					return;
				}

				if ((m_notifyRunning && b.Status == BuildStatus.Running)
					|| (m_notifyFailed && b.IsFailed())
					|| (m_notifySuccess && b.IsSuccess()))
				{
					var reason = b.Status == BuildStatus.Running
								  ? ": {0}".With(b.LastChangeDescription)
								  : string.Empty;
					m_slack.SendToDefaultChannel(
						"{0} - {1} {2} by {3} at {4:HH:mm}{5}".With(
						b.Configuration.Project.Name,
						b.Configuration.Name,
						b.Status.ToString().ToLowerInvariant(),
						b.TriggeredBy,
						DateTime.Now,
							reason));
				}
			};
		}

		void RespondToMessage(Message msg)
		{
			var handledCount = 0;

			foreach (var handler in m_messageHandlers)
			{
				if (handler.Process(msg))
				{
					m_log.Debug("{0} has handled the message", handler.GetType().Name);
					handledCount++;
				}
			}

			if (handledCount == 0)
			{
				m_slack.Respond("Sorry, I didn't understand what you said :(");
			}
		}
		#endregion
	}
}