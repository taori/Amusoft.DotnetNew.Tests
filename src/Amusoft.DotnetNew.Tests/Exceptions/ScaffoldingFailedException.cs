using System.Diagnostics.CodeAnalysis;
using Amusoft.DotnetNew.Tests.Scopes;

namespace Amusoft.DotnetNew.Tests.Exceptions;

/// <summary>
/// 
/// </summary>
[ExcludeFromCodeCoverage]
public class ScaffoldingFailedException : CliException
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="rewriteContext"></param>
	/// <param name="message"></param>
	/// <param name="output"></param>
	public ScaffoldingFailedException(IRewriteContext? rewriteContext, string? message, string? output) : base(rewriteContext, message, output)
	{
	}
}