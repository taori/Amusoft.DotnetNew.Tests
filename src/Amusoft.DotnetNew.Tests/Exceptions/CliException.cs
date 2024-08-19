using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Amusoft.DotnetNew.Tests.Scopes;

namespace Amusoft.DotnetNew.Tests.Exceptions;

/// <summary>
/// 
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class CliException : Exception
{
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return $"{Message}{Environment.NewLine}{Output}";
	}

	/// <summary>
	/// Build output
	/// </summary>
	public string? Output { get; }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="rewriteContext"></param>
	/// <param name="message"></param>
	/// <param name="output"></param>
	protected CliException(IRewriteContext? rewriteContext, string? message, string? output) : base(BuildMessage(rewriteContext, message, output))
	{
		Output = output;
	}

	private static string BuildMessage(IRewriteContext? rewriteContext, string? message, string? output)
	{
		return (message, output) switch
		{
			({ } m, { } o) => Rewritten($"{m}{Environment.NewLine}{Environment.NewLine}{o}"),
			({ } m, null) => Rewritten($"{m}"),
			(null, { } o) => Rewritten($"{o}"),
			(null, null) => $"Message cannot be built because both arguments are null",
		};

		string Rewritten(string i)
		{
			return rewriteContext is not null
				? rewriteContext.Rewrite(i)
				: i;
		}
	}
}