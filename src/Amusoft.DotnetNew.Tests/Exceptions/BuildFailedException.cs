namespace Amusoft.DotnetNew.Tests.Exceptions;

/// <summary>
/// 
/// </summary>
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