﻿namespace Amusoft.DotnetNew.Tests.Exceptions;

/// <summary>
/// 
/// </summary>
public class ScaffoldingFailedException : CliException
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="message"></param>
	/// <param name="output"></param>
	public ScaffoldingFailedException(string? message, string? output) : base(message, output)
	{
	}
}