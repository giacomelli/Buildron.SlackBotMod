using Buildron.Domain.Mods;

namespace Giacomelli.Buildron.SlackBotMod
{
    public class Mod : IMod
    {
		public static IModContext Context { get; private set; }

        public void Initialize(IModContext context)
        {
			Context = context;

			context.Preferences.Register(
				new Preference("Token", "Slack key", PreferenceKind.String, string.Empty),
				new Preference("Channel", "Channel", PreferenceKind.String, "general"),
				new Preference("NotifyRunning", "Notify running", PreferenceKind.Bool, false),
				new Preference("NotifyFailed", "Notify failed", PreferenceKind.Bool, true),
				new Preference("NotifySuccess", "Notify success", PreferenceKind.Bool, true));

			context.CIServerConnected += (sender, e) =>
			{
				context.GameObjects.Create<UnityMainThreadDispatcher>();
				context.GameObjects.Create<BotController>();
			};
        }
    }
}