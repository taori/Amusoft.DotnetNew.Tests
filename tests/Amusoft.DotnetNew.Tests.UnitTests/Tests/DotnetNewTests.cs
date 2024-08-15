using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.DotnetNew.Tests.UnitTests.Helpers;
using Shouldly;
using VerifyXunit;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class DotnetNewTests
{
	[Theory]
	[InlineData("Project1", "gitUser", "authorname")]
	[InlineData("Project2", "gitUser", "authorname")]
	private async Task ScaffoldRepo(string projectName, string gitUser, string author)
	{
		using (var loggingScope = new LoggingScope())
		{
			var solution = TemplateSolutionInstallerHelper.GetLocalSolution();
			var installations = await solution.InstallTemplatesFromDirectoryAsync("../tests/Resources", CancellationToken.None);

			var args = $"""
			           -n "{projectName}"
			           --GitProjectName "{projectName}",
			           --NugetPackageId "{projectName}", 
			           --ProductName "{projectName}", 
			           --GitUser "{gitUser}", 
			           --Author "{author}"
			           """;
			var scaffold = await CLI.DotnetNew.NewAsync("dotnet-library-repo", args.Replace(Environment.NewLine, " "), CancellationToken.None);
			var list = scaffold.GetDirectoryContents().ToArray();
			await scaffold.RestoreAsync($"src/{projectName}.sln", null, CancellationToken.None);
			await scaffold.BuildAsync($"src/{projectName}.sln", null, CancellationToken.None);

			var sb = new StringBuilder();
			loggingScope.Logger.Print(sb, PrintKind.All);
			var log = sb.ToString();
			await Verifier.Verify(log).UseParameters(projectName, gitUser, author);
			
			installations.Count.ShouldBe(1);
		}
	}
}