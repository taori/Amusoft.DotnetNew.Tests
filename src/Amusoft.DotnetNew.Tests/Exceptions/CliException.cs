using System;
using System.Diagnostics.CodeAnalysis;

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
		return Message;
	}

	/// <summary>
	/// Build output
	/// </summary>
	public string? Output { get; }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="message"></param>
	/// <param name="output"></param>
	protected CliException(string? message, string? output) : base(message)
	{
		Output = output;
	}
}