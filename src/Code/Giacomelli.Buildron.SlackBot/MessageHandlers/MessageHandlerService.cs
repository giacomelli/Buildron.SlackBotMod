using System;
using System.Linq;
using System.Reflection;
using Buildron.Domain.Mods;

namespace Giacomelli.Buildron.SlackBot
{
	public static class MessageHandlerService
	{
		private static IMessageHandler[] s_handlers;

		public static IMessageHandler[] GetMessageHandlers(IModContext modContext, SlackService slackService)
		{
			if (s_handlers == null)
			{
				var interfaceType = typeof(IMessageHandler);
				var handlersTypes = typeof(MessageHandlerService).Assembly.GetTypes()
								.Where(t =>
									   !t.IsAbstract &&
									   !t.IsInterface &&
									   interfaceType.IsAssignableFrom(t)).ToArray();

				s_handlers = new IMessageHandler[handlersTypes.Length];

				for (int i = 0; i < s_handlers.Length; i++)
				{
					s_handlers[i] = Activator.CreateInstance(
						handlersTypes[i],
						new object[] { modContext, slackService }) as IMessageHandler;
				}
			}

			return s_handlers;
		}
	}
}

