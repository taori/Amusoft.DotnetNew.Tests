namespace Amusoft.DotnetNew.Tests.UnitTests.Helpers;

public static class SolutionFileHelper
{
	public static TemplateSolutionFile GetLocalSolution() => 
		new(typeof(InstallationTests).Assembly.Location, 6, "Amusoft.DotnetNew.Tests.sln");
}