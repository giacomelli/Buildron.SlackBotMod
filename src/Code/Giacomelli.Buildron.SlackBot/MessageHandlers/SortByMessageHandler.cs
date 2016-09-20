using System;
using System.Text.RegularExpressions;
using Buildron.Domain.Builds;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Sorting;
using Giacomelli.Buildron.SlackBot;

namespace Giacomelli.Buildron.SlackBot
{
	public class SortByMessageHandler : RegexMessageHandlerBase
	{
		public SortByMessageHandler(IModContext modContext, SlackService slackService)
			: base("sort by (?<value>(date|status|text))", modContext, slackService)
		{
		}

		public override string Description
		{
			get
			{
				return "sort by date|status|text: sorts the builds.";
			}
		}

		protected override IRemoteControlCommand CreateCommand(Match match)
		{
			var value = match.Groups["value"].Value;
			SortBy sortBy = SortBy.RelevantStatus;

			if (!value.Equals("status", StringComparison.OrdinalIgnoreCase))
			{
				sortBy = (SortBy)Enum.Parse(typeof(SortBy), value, true);
			}

			return new SortBuildsRemoteControlCommand(new ShellSortingAlgorithm<IBuild>(), sortBy);
		}

		protected override string CreateMessage(Match match)
		{
			var value = match.Groups["value"].Value;

			return "Sorting builds by '{0}'" .With(value);
		}
	}
}

