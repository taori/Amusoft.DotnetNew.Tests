using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Templating;
using Amusoft.DotnetNew.Tests.Utility;

namespace Amusoft.DotnetNew.Tests.Diagnostics;

internal class TestResult(
	string command,
	string content
) : ICommandResult
{
	// .A total of 1 test files matched the specified pattern.Passed!  - Failed:     0, Passed:     1, Skipped:     0, Total:     1, Duration: < 1 ms - Project1.IntegrationTests.dll (net6.0)Passed!  - Failed:     0, Passed:     1, Skipped:     0, Total:     1, Duration: < 1 ms - Project1.UnitTests.dll (net6.0)",
	private static readonly Regex Regex = new(@"(?<=\r?\n)(?!\r?\n)([\w]+|!)!.+<(?<duration>[^-]+)(?<project>[^(]+).+", RegexOptions.Compiled);
	
	private IEnumerable<string> GetDlls()
	{
		var matchCollection = Regex.Matches(content);
		return matchCollection
			.Select(match => (content: match.Value.Replace(match.Groups["duration"].Value, " SCRUBBED "), sort: match.Groups["project"].Value))
			.OrderBy(d => d.sort)
			.Select(d => d.content.Trim());
	}

	public void Print(StringBuilder stringBuilder)
	{
		var serializeContent = new
		{
			Command = command,
			Lines = GetDlls(),
		};
		var serialized = JsonSerializer.Serialize(serializeContent,new JsonSerializerOptions()
			{
				WriteIndented = true,
				Encoder = CustomJsonEncoder.Instance
			}
		);
		
		stringBuilder.Append(TemplatingDefaults.Instance.PrintPattern("TestResult", serialized));
	}
}