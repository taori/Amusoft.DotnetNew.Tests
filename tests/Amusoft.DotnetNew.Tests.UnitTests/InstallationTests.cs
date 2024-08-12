using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.UnitTests.Helpers;
using VerifyXunit;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests;

public class InstallationTests
{
	[Fact]
	public async Task TestInstallAndUninstall()
	{
		var solutionFile = SolutionFileHelper.GetLocalSolution();
		var installation = await solutionFile.InstallTemplateAsync("../tests/Resources/dotnet-library-repo", CancellationToken.None);
		await installation.UninstallAsync(CancellationToken.None);

		var sb = new StringBuilder();
		solutionFile.Print(sb, PrintKind.All);

		await Verifier.Verify(sb.ToString());
	}
}