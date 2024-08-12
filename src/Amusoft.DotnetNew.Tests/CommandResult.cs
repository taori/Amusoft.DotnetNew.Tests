using System;
using System.Text;
using System.Text.Json;

namespace Amusoft.DotnetNew.Tests;

/// <summary>
/// 
/// </summary>
/// <param name="ExitCode"></param>
/// <param name="Output"></param>
/// <param name="Errors"></param>
/// <param name="Success"></param>
/// <param name="Runtime"></param>
internal record class CommandResult(
	int ExitCode,
	string Output,
	string Errors,
	bool Success,
	TimeSpan Runtime
) : ICommandResponse
{
	public void Print(StringBuilder stringBuilder)
	{
		var serialized = JsonSerializer.Serialize((this with
			{
				Runtime = TimeSpan.Zero
			}),
			new JsonSerializerOptions()
			{
				WriteIndented = true,
				Encoder = NewLineIgnoreEncoder.Instance
			}
		);
		
		stringBuilder.Append(TemplatingDefaults.Instance.PrintPattern("Result", serialized));
	}
}