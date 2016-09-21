using System;
using System.IO;
using NUnit.Framework;
using System.Linq;
using Rhino.Mocks;
using Buildron.Domain.Mods;
using Skahal.Logging;

namespace Giacomelli.Buildron.SlackBotMod.UnitTests
{
	[TestFixture]
	public class MessageHandlerServiceTest
	{
		[Test]
		public void GetMessageHandlers_Args_Handlers()
		{
			var ctx = MockRepository.Mock<IModContext>();
			ctx.Expect(c => c.Log).Return(MockRepository.Mock<ISHLogStrategy>());
			ctx.Expect(c => c.Preferences).Return(MockRepository.Mock<IPreferencesProxy>());
			var actual = MessageHandlerService.GetMessageHandlers(ctx, new SlackService(ctx));
			Assert.IsNotNull(actual);
			Assert.AreNotEqual(0, actual.Length);
		}
	}
}

