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
using Xunit.Sdk;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class InstallationTests : TestBase
{
	[Fact]
	public async Task TestInstallAndUninstall()
	{
		using (var loggingScope = new LoggingScope())
		{
			var solutionFile = TemplateSolutionInstallerHelper.CreateLocalSolution();
			var installation = await solutionFile.InstallTemplateAsync("../tests/Resources/dotnet-library-repo", CancellationToken.None);
			await installation.UninstallAsync(CancellationToken.None);

			await Verifier.Verify(loggingScope.ToFullString(PrintKind.All));
		}
	}
	
	[Fact]
	public async Task DiscoverTemplateProjects()
	{
		var solutionFile = TemplateSolutionInstallerHelper.CreateLocalSolution();
		var paths = solutionFile.DiscoverTemplates("../tests/Resources")
			.Select(d => d.VirtualPath);

		await Verifier.Verify(paths);
	}
	
	[Fact]
	public async Task InstallFromDiscovery()
	{
		using (var loggingScope = new LoggingScope())
		{
			var solutionFile = TemplateSolutionInstallerHelper.CreateLocalSolution();
			var installations = await solutionFile.InstallTemplatesFromDirectoryAsync("../tests/Resources", CancellationToken.None);
			foreach (var installation in installations.Installations)
			{
				await installation.UninstallAsync(CancellationToken.None);
			}

			var log = loggingScope.ToFullString(PrintKind.All);
			await Verifier.Verify(log);
			
		}
	}

	public InstallationTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
	{
	}
}