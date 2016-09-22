using System;
using Buildron.Domain.Builds;
using Buildron.Domain.Mods;
using Skahal.Logging;
using UnityEngine;

namespace Giacomelli.Buildron.SlackBotMod
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

                m_messageHandlers = MessageHandlerService.GetMessageHandlers(m_modContext, m_slack);

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
                try
                {
                    var b = e.Build;

                    if (b.Status == e.PreviousStatus || e.PreviousStatus > BuildStatus.Running)
                    {
                        return;
                    }

                    if ((m_notifyRunning && b.Status == BuildStatus.Running)
                        || (m_notifyFailed && b.IsFailed())
                        || (m_notifySuccess && b.IsSuccess()))
                    {
                        m_slack.SendToDefaultChannel(
							"{6} {0} - {1} by {2} {3} at {4:HH:mm}{5}".With(
                            b.Configuration.Project.Name,
                            b.Configuration.Name,
                            b.TriggeredBy,
                            b.Status.ToString().ToLowerInvariant(),
                            DateTime.Now,
								String.IsNullOrEmpty(b.LastChangeDescription) ? string.Empty : ": {0}".With(b.LastChangeDescription),
                            GetEmoji(b)));
                    }
                }
                catch (Exception ex)
                {
                    m_log.Error("Error while trying to send build status change message: {0}. {1}", ex.Message, ex.StackTrace);
                }
            };
        }

        private string GetEmoji(IBuild build)
        {
            if (build.IsRunning())
            {
                return ":grey_question:";
            }
            else if (build.IsSuccess())
            {
                return ":smiley:";
            }
            else
            {
                return ":rage:";
            }
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
                m_slack.Respond("Sorry, I didn't understand what you said :(. Try \"help\".");
            }
        }
        #endregion
    }
}