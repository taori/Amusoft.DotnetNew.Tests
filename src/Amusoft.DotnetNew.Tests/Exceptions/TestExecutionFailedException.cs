using System.Diagnostics.CodeAnalysis;

namespace Amusoft.DotnetNew.Tests.Exceptions;

/// <summary>
/// 
/// </summary>
[ExcludeFromCodeCoverage]
public class TestExecutionFailedException : CliException
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="message"></param>
	/// <param name="output"></param>
	public TestExecutionFailedException(string? message, string? output) : base(message, output)
	{
	}
}