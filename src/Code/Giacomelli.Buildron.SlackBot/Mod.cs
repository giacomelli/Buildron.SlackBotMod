using Buildron.Domain.Mods;

namespace Giacomelli.Buildron.SlackBot
{
    public class Mod : IMod
    {
		public static IModContext Context { get; private set; }

        public void Initialize(IModContext context)
        {
			Context = context;

			//xoxb-80789644979-uQMlJbghIvWZkXVQJP9eQrWg
			context.Preferences.Register(new Preference("Token", "Slack key", PreferenceKind.String, string.Empty));

			context.CIServerConnected += (sender, e) =>
			{
				context.GameObjects.Create<BotController>();
			};
        }
    }
}