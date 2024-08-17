using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.DotnetNew.Tests.UnitTests.Configuration;
using Amusoft.DotnetNew.Tests.UnitTests.Helpers;
using Amusoft.DotnetNew.Tests.UnitTests.Toolkit;
using Shouldly;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class DotnetNewTests : TestBase
{
	// 15m
	[InlineData("Project1", "gitUser", "authorname")]
	[InlineData("Project2", "gitUser", "authorname")]
	// [Trait("Category","SkipInCI")]
	[Theory(Timeout = 900_000)]
	private async Task ScaffoldRepo(string projectName, string gitUser, string author)
	{
		using (var loggingScope = new LoggingScope())
		{
			var solution = TemplateSolutionInstallerHelper.CreateLocalSolution();
			await using (var installations = await solution.InstallTemplatesFromDirectoryAsync("../tests/Resources", CancellationToken.None))
			{
				var args = $"""
				            -n "{projectName}"
				            --GitProjectName "{projectName}",
				            --NugetPackageId "{projectName}", 
				            --ProductName "{projectName}", 
				            --GitUser "{gitUser}", 
				            --Author "{author}"
				            """;
				var scaffold = await CLI.DotnetNew.NewAsync("dotnet-library-repo", args.Replace(Environment.NewLine, " "), CancellationToken.None);
				var list = scaffold.GetRelativeDirectoryPaths().ToArray();
				await scaffold.RestoreAsync($"src/{projectName}.sln", null, CancellationToken.None);
				await scaffold.BuildAsync($"src/{projectName}.sln", null, CancellationToken.None);

				var a = loggingScope.ToFullString(PrintKind.All);

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

	public DotnetNewTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
	{
	}
}