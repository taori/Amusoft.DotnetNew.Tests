using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Templating;
using Amusoft.DotnetNew.Tests.Utility;

namespace Amusoft.DotnetNew.Tests.Diagnostics;

internal class BuildResult(
	string command,
	string content
) : ICommandResult
{
	private static readonly Regex Regex = new(@"-(?:>) (?<dll>.+?\.dll)", RegexOptions.Compiled);
	
	private IEnumerable<string> GetDlls()
	{
		var matchCollection = Regex.Matches(content);
		return matchCollection
			.Select(d => d.Groups["dll"].Value)
			.OrderBy(d => d)
			.Select(d => new CrossPlatformPath(d).VirtualPath);
	}

	public void Print(StringBuilder stringBuilder)
	{
		var serializeContent = new
		{
			Command = command,
			Files = GetDlls(),
		};
		var serialized = JsonSerializer.Serialize(serializeContent,new JsonSerializerOptions()
			{
				WriteIndented = true,
				Encoder = CustomJsonEncoder.Instance
			}
		);
		
		stringBuilder.Append(TemplatingDefaults.Instance.PrintPattern("BuildResult", serialized));
	}
}