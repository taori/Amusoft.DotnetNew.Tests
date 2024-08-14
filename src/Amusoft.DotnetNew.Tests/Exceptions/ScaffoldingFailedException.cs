using System;

namespace Amusoft.DotnetNew.Tests.Exceptions;

/// <summary>
/// 
/// </summary>
public class ScaffoldingFailedException : Exception
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="message"></param>
	/// <param name="innerException"></param>
	public ScaffoldingFailedException(string? message, Exception? innerException) : base(message, innerException)
	{
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="message"></param>
	public ScaffoldingFailedException(string? message) : base(message)
	{
	}
}