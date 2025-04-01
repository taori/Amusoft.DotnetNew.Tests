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
	// old:.A total of 1 test files matched the specified pattern.Passed!  - Failed:     0, Passed:     1, Skipped:     0, Total:     1, Duration: < 1 ms - Project1.IntegrationTests.dll (net6.0)Passed!  - Failed:     0, Passed:     1, Skipped:     0, Total:     1, Duration: < 1 ms - Project1.UnitTests.dll (net6.0)",
	// new: Test summary: total: 1; failed: 0; succeeded: 1; skipped: 0; duration: 1,2s
	// new: Amusoft.Toolkit.Mvvm.Wpf.IntegrationTests test net8.0 succeeded (2,7s)
	private static readonly Regex Xunit2Regex = new(@"Passed!\s*-\s*Failed:\s*(?<failed>\d+),\s*Passed:\s*(?<passed>\d+),\s*Skipped:\s*(?<skipped>\d+),\s*Total:\s*(?<total>\d+),\s*Duration:\s*<?\s*(?<duration>[\d,.]+ [ms]+)\s*-\s*(?<project>[\w.]+)\s*\((?<version>[^)]+)\)", RegexOptions.Compiled);
	private static readonly Regex Xunit3Regex = new(@"Test summary: total: [\d]+; failed: [\d]+; succeeded: [\d]+; skipped: [\d]+; duration: (?<duration>[^sm]+)[sm]|(?<=\n\s).+(?= test ) test .+\((?<duration>[^)]+)\)", RegexOptions.Compiled);
	
	private IEnumerable<string> GetTestResultLines()
	{
		if (GetXunit2Lines() is { Length: > 0 } xunit2)
			return xunit2;
		if (GetXunit3Lines() is { Length: > 0 } xunit3)
			return xunit3;
		return GetXunit2Lines();
	}

	private string[] GetXunit2Lines()
	{
		var matchCollection = Xunit2Regex.Matches(content);
		return matchCollection
			.Select(match => (content: match.Value.Replace(match.Groups["duration"].Value, "SCRUBBED"), sort: match.Groups["project"].Value))
			.OrderBy(d => d.sort)
			.Select(d => d.content.Trim())
			.ToArray();
	}

	private string[] GetXunit3Lines()
	{
		var matchCollection = Xunit3Regex.Matches(content);
		return matchCollection
			.Select(match => (content: match.Value.Replace(match.Groups["duration"].Value, " SCRUBBED "), sort: match.Groups["project"].Value))
			.OrderBy(d => d.sort)
			.Select(d => d.content.Trim())
			.ToArray();
	}

	public void Print(StringBuilder stringBuilder)
	{
		var serializeContent = new
		{
			Command = command,
			Lines = GetTestResultLines(),
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