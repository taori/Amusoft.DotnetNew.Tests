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

public class InstallationTests
{
	[Fact]
	public async Task TestInstallAndUninstall()
	{
		using (var loggingScope = new LoggingScope())
		{
			var solutionFile = TemplateSolutionInstallerHelper.GetLocalSolution();
			var installation = await solutionFile.InstallTemplateAsync("../tests/Resources/dotnet-library-repo", CancellationToken.None);
			await installation.UninstallAsync(CancellationToken.None);

			var sb = new StringBuilder();
			solutionFile.Print(sb, PrintKind.All);

			await Verifier.Verify(sb.ToString());
		}
	}
	
	[Fact]
	public async Task DiscoverTemplateProjects()
	{
		var solutionFile = TemplateSolutionInstallerHelper.GetLocalSolution();
		var paths = solutionFile.DiscoverTemplates("../tests/Resources")
			.Select(d => d.VirtualPath);

		await Verifier.Verify(paths);
	}
	
	[Fact]
	public async Task InstallFromDiscovery()
	{
		using (var loggingScope = new LoggingScope())
		{
			var solutionFile = TemplateSolutionInstallerHelper.GetLocalSolution();
			var installations = await solutionFile.InstallTemplatesFromDirectoryAsync("../tests/Resources", CancellationToken.None);

			foreach (var installation in installations)
			{
				await installation.UninstallAsync(CancellationToken.None);
			}

			installations.Count.ShouldBe(1);

			var sb = new StringBuilder();
			loggingScope.Logger.Print(sb, PrintKind.All);
			await Verifier.Verify(sb.ToString());
		}
	}
}