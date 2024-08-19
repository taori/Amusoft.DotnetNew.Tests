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
	/// <param name="rewriteContext"></param>
	public BuildFailedException(string? message, string? output, IRewriteContext? rewriteContext) : base(rewriteContext, message, output)
	{
	}
}