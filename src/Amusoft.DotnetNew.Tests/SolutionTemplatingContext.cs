namespace Amusoft.DotnetNew.Tests;

internal class SolutionTemplatingContext
{
	public SolutionTemplatingContext(TemplateSolutionFile solutionFile, CommandLogger commandLogger)
	{
		CommandLogger = commandLogger;
		SolutionFile = solutionFile;
	}

	internal TemplateSolutionFile SolutionFile { get; set; }
	
	internal CommandLogger CommandLogger { get; set; }
}