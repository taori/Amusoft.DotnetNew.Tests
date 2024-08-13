using Amusoft.DotnetNew.Tests.Utility;

namespace Amusoft.DotnetNew.Tests.Templating;

/// <summary>
/// Context of templating operations
/// </summary>
public class ProjectTemplatingContext
{
	internal ProjectTemplatingContext(SolutionTemplatingContext solutionContext, CrossPlatformPath projectTemplatePath)
	{
		SolutionContext = solutionContext;
		ProjectTemplatePath = projectTemplatePath;
	}

	/// <summary>
	/// Solution file associated with the operations
	/// </summary>
	internal SolutionTemplatingContext SolutionContext { get; set; }

	/// <summary>
	/// Path of the templating solution
	/// </summary>
	internal CrossPlatformPath ProjectTemplatePath { get; set; }
}