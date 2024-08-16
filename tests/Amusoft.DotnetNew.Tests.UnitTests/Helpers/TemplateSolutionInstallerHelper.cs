using Amusoft.DotnetNew.Tests.Templating;
using Amusoft.DotnetNew.Tests.UnitTests.Tests;

namespace Amusoft.DotnetNew.Tests.UnitTests.Helpers;

public static class TemplateSolutionInstallerHelper
{
	public static TemplateSolution CreateLocalSolution() => 
		new(typeof(InstallationTests).Assembly.Location, 6, "Amusoft.DotnetNew.Tests.sln");
}