namespace Giacomelli.Buildron.SlackBotMod
{
	public class Message
	{
		public Message(string text, string channel)
		{
			Text = text;
			Channel = channel;
		}

		public Message(string text, Message responseTo)
			:this(text, responseTo.Channel)
		{
			ResponseTo = responseTo;
		}

		public string Text { get; private set; }
		public string Channel { get; private set; }

		public Message ResponseTo { get; private set; }
	}
}