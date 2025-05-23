using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Scopes;
using Shared.TestSdk.Helpers;

namespace Amusoft.DotnetNew.Tests.IntegrationTests.Tests;

public class TemplateSolutionTests
{
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
			.Select(d => d.VirtualPath)
			.ToArray();

		await Verifier.Verify(paths);
	}
	
	[Fact(Timeout = 15000)]
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
}