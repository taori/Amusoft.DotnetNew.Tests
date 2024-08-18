using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Scopes;
using Shared.TestSdk;
using Shared.TestSdk.Helpers;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class InstallationTests : TestBase
{
	[Fact(Timeout = 10000)]
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
	
	[Fact(Timeout = 10000)]
	public async Task DisposeOnlyHappensOnceOnUninstall()
	{
		using (var loggingScope = new LoggingScope())
		{
			var solutionFile = TemplateSolutionInstallerHelper.CreateLocalSolution();
			var installation = await solutionFile.InstallTemplateAsync("../tests/Resources/dotnet-library-repo", CancellationToken.None);
			await installation.UninstallAsync(CancellationToken.None);
			installation.Dispose();

			await Verifier.Verify(loggingScope.ToFullString(PrintKind.All));
		}
	}
	
	[Fact(Timeout = 10000)]
	public async Task DiscoverTemplateProjects()
	{
		var solutionFile = TemplateSolutionInstallerHelper.CreateLocalSolution();
		var paths = solutionFile.DiscoverTemplates("../tests/Resources")
			.Select(d => d.VirtualPath);

		await Verifier.Verify(paths);
	}
	
	[Fact(Timeout = 10000)]
	public async Task InstallFromDiscovery()
	{
		using (var loggingScope = new LoggingScope())
		{
			var solutionFile = TemplateSolutionInstallerHelper.CreateLocalSolution();
			var installations = await solutionFile.InstallTemplatesFromDirectoryAsync("../tests/Resources", CancellationToken.None);

			installations.Dispose();

			var log = loggingScope.ToFullString(PrintKind.All);
			await Verifier.Verify(log);
			
		}
	}

	public InstallationTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
	{
	}
}