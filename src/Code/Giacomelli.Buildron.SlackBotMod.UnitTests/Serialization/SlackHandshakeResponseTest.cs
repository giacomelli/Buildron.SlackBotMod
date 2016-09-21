using System;
using System.IO;
using NUnit.Framework;
using System.Linq;

namespace Giacomelli.Buildron.SlackBotMod.UnitTests
{
	[TestFixture]
	public class SlackHandshakeResponseTest
	{
		[Test]
		public void Parse_Sample1Json_PropertiesBind()
		{
			var json = File.ReadAllText("Resources/SlackHandshakeResponse-sample1.json");
			var actual = SlackHandshakeResponse.Parse(json);

			// Handshake.
			Assert.IsNotNull(actual);
			Assert.AreEqual("wss://mpmulti-vbok.slack-msgs.com/websocket/_KTKZCIHEIfj03SEihsxPXFNZuXjrbFJeYHAaqnDBgVhS-1wmDEhlb7SGlR4-FEgyfAjGOJ0eOpT4USW7_i3nYHVOj6RyUTxP-0M1qkl7_1eRZV9PiwsYJE-K1_KiZw9UtK3n1TVgfT8F70waQfEOLsqyprJMt1vxCXbSAUXYN4=", actual.Url);

			// Self.
			var self = actual.Self;
			Assert.IsNotNull(self);
			Assert.AreEqual("U1CP7JYUT", self.Id);
			Assert.AreEqual("buildron", self.Name);

			// Channels.
			Assert.IsNotNull(self.Channels);
			var channels = self.Channels.ToArray();
			Assert.AreEqual(3, channels.Length);
			var channel = channels[0];
			Assert.AreEqual("C09LD197T", channel.Id);
			Assert.AreEqual("general", channel.Name);

			channel = channels[1];
			Assert.AreEqual("C09LCS6ES", channel.Id);
			Assert.AreEqual("random", channel.Name);

			channel = channels[2];
			Assert.AreEqual("C0G7T1GCA", channel.Id);
			Assert.AreEqual("toread", channel.Name);

			// DMs.
			Assert.IsNotNull(self.DMs);
			var dms = self.DMs.ToArray();
			Assert.AreEqual(2, dms.Length);
			var dm = dms[0];
			Assert.AreEqual("D1CQKMME0", dm.Id);
			Assert.AreEqual("USLACKBOT", dm.UserId);

			dm = dms[1];
			Assert.AreEqual("D1CR61QSE", dm.Id);
			Assert.AreEqual("U09LD191M", dm.UserId);
		}
	}
}

