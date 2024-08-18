using Amusoft.DotnetNew.Tests.Templating;

namespace Shared.TestSdk.Helpers;

public static class TemplateSolutionInstallerHelper
{
	public static TemplateSolution CreateLocalSolution() => 
		new(typeof(TemplateSolutionInstallerHelper).Assembly.Location, 6, "Amusoft.DotnetNew.Tests.sln");
}