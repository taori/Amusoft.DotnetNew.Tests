using Amusoft.DotnetNew.Tests.Templating;
using Amusoft.DotnetNew.Tests.UnitTests.Tests;

namespace Amusoft.DotnetNew.Tests.UnitTests.Helpers;

public static class TemplateSolutionInstallerHelper
{
	public static TemplateSolutionInstaller GetLocalSolution() => 
		new(typeof(InstallationTests).Assembly.Location, 6, "Amusoft.DotnetNew.Tests.sln");
}