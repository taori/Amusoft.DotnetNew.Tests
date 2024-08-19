using System.Diagnostics.CodeAnalysis;
using Amusoft.DotnetNew.Tests.Scopes;

namespace Amusoft.DotnetNew.Tests.Exceptions;

/// <summary>
/// 
/// </summary>
[ExcludeFromCodeCoverage]
public class BuildFailedException : CliException
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="message"></param>
	/// <param name="output"></param>
	public BuildFailedException(string? message, string? output) : base(message, output)
	{
	}
}