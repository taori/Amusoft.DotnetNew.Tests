using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Scopes;
using Shared.TestSdk;
using Shared.TestSdk.Helpers;
using Shouldly;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.IntegrationTests.Tests;

[Trait("Category","SkipInCI")]
public class ScaffoldingTests : TestBase
{
	[InlineData("Project1", "gitUser", "authorname")]
	[InlineData("Project2", "gitUser", "authorname")]
	// https://github.com/taori/Amusoft.DotnetNew.Tests/issues/1
	[Theory(Timeout = 60_000)]
	private async Task BuildRepositoryTemplate(string projectName, string gitUser, string author)
	{
		using (var loggingScope = new LoggingScope())
		{
			var solution = TemplateSolutionInstallerHelper.CreateLocalSolution();
			using (var installations = await solution.InstallTemplatesFromDirectoryAsync("../tests/Resources", CancellationToken.None))
			{
				var args = $"""
				            -n "{projectName}"
				            --GitProjectName "{projectName}"
				            --NugetPackageId "{projectName}"
				            --ProductName "{projectName}"
				            --GitUser "{gitUser}"
				            --Author "{author}"
				            """;
				using (var scaffold = await CLI.Dotnet.NewAsync("dotnet-library-repo", args.Replace(Environment.NewLine, " "), CancellationToken.None))
				{
					var list = scaffold.GetRelativeDirectoryPaths().ToArray();
					await scaffold.RestoreAsync($"src/{projectName}.sln", null, CancellationToken.None);
					await scaffold.BuildAsync($"src/{projectName}.sln", null, CancellationToken.None);

					await Verifier.Verify(new
							{
								log = loggingScope.ToFullString(PrintKind.All),
								files = list,
							}
						)
						.UseParameters(projectName, gitUser, author);

					installations.Installations.Count.ShouldBe(1);
				}
			}
		}
	}
	
	[Fact(Timeout = 10_000)]
	private async Task GetFileContentAsync()
	{
		var projectName = "Project1";
		var gitUser = "GitUser";
		var author = "TestAuthor";
		using (var loggingScope = new LoggingScope())
		{
			var solution = TemplateSolutionInstallerHelper.CreateLocalSolution();
			using (var installations = await solution.InstallTemplatesFromDirectoryAsync("../tests/Resources", CancellationToken.None))
			{
				var args = $"""
				            -n "{projectName}"
				            --GitProjectName "{projectName}"
				            --NugetPackageId "{projectName}"
				            --ProductName "{projectName}" 
				            --GitUser "{gitUser}" 
				            --Author "{author}"
				            """;
				using (var scaffold = await Dotnet.NewAsync("dotnet-library-repo", args.Replace(Environment.NewLine, " "), CancellationToken.None))
				{
					var readmeContent = await scaffold.GetFileContentAsync("README.md");
					await Verifier.Verify(readmeContent);
				}
			}
		}
	}

	public ScaffoldingTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
	{
	}
}