using System;
using System.Runtime.Serialization;

namespace Amusoft.DotnetNew.Tests.Exceptions;

/// <summary>
/// 
/// </summary>
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
	/// <param name="message"></param>
	/// <param name="output"></param>
	/// <param name="innerException"></param>
	protected CliException(string? message, string? output, Exception? innerException) : base(BuildMessage(message, output), innerException)
	{
		Output = output;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="message"></param>
	/// <param name="output"></param>
	protected CliException(string? message, string? output) : base(BuildMessage(message, output))
	{
		Output = output;
	}

	private static string BuildMessage(string? message, string? output)
	{
		return (message, output) switch
		{
			({ } m, { } o) => $"{m}{Environment.NewLine}{Environment.NewLine}{o}",
			({ } m, null) => $"{m}",
			(null, { } o) => $"{o}",
			(null, null) => $"Message cannot be built because both arguments are null",
		};
	}
}