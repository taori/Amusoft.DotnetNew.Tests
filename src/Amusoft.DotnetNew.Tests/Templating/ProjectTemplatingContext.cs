using Amusoft.DotnetNew.Tests.Utility;

namespace Amusoft.DotnetNew.Tests.Templating;

/// <summary>
/// Context of templating operations
/// </summary>
public class ProjectTemplatingContext
{
	internal ProjectTemplatingContext(CrossPlatformPath projectTemplatePath)
	{
		ProjectTemplatePath = projectTemplatePath;
	}

	/// <summary>
	/// Path of the templating solution
	/// </summary>
	internal CrossPlatformPath ProjectTemplatePath { get; set; }
}