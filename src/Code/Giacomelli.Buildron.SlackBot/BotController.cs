using System;
using System.Collections;
using Buildron.Domain.Mods;
using Skahal.Logging;
using Skahal.Threading;
using UnityEngine;

namespace Giacomelli.Buildron.SlackBot
{
	public class BotController : MonoBehaviour
	{
		#region Fields
		private IModContext m_modContext;
		private string m_slackKey;
		private ISHLogStrategy m_log;
		#endregion

		#region Methods
		private void Start()
		{
			m_modContext = Mod.Context;
			m_log = m_modContext.Log;
		
			m_slackKey = m_modContext.Preferences.GetValue<string>("Token");

			if (String.IsNullOrEmpty(m_slackKey))
			{
				m_log.Warning("Will not start, because Token is empty.");
				return;
			}

			m_modContext.BuildStatusChanged += (sender, e) =>
			{
				var b = e.Build;
				Send("{0} - {1}: {2}", b.Configuration.Project.Name, b.Configuration.Name, b.Status.ToString().ToLowerInvariant());
			};
		}

		/// <summary>
		/// Send the specified message and args.
		/// https://api.slack.com/methods/chat.postMessage
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="args">Arguments.</param>
		private void Send(string message, params object[] args)
		{
			var formattedMsg = message.With(args);
			m_log.Debug("Sending message '{0}'...", formattedMsg);
			var url = "https://slack.com/api/chat.postMessage";
			var data = new WWWForm();
			data.AddField("token", m_slackKey);
			data.AddField("channel", "general");
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