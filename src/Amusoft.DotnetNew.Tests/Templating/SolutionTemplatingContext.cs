using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Rewriters;

namespace Amusoft.DotnetNew.Tests.Templating;

internal class SolutionTemplatingContext
{
	public SolutionTemplatingContext(TemplateSolutionInstaller solutionInstaller, CommandLogger commandLogger)
	{
		CommandLogger = commandLogger;
		SolutionInstaller = solutionInstaller;
		
		CommandLogger.AddRewriter(BackslashRewriter.Instance);
		CommandLogger.AddRewriter(new SolutionDirectoryRewriter(this));
	}

	internal TemplateSolutionInstaller SolutionInstaller { get; set; }
	
	internal CommandLogger CommandLogger { get; set; }
}