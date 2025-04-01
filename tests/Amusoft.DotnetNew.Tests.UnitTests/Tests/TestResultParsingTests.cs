using System.Text;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Shared.TestSdk.Toolkit;
using VerifyXunit;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class TestResultParsingTests
{
	[Fact]
	private async Task VerifyXUnit3Parsing()
	{
		var err = new EmbeddedResourceReader(typeof(CommandResultTests).Assembly);
		var content = err.GetContent("TestResources.DotnetTestOutput.xunit3.txt");
		var result = new TestResult("fake command",content);
		var sb = new StringBuilder();
		result.Print(sb);
		await Verifier.Verify(sb.ToString());
	}
	
	[Fact]
	private async Task VerifyXUnit2Parsing()
	{
		var err = new EmbeddedResourceReader(typeof(CommandResultTests).Assembly);
		var content = err.GetContent("TestResources.DotnetTestOutput.xunit2.txt");
		var result = new TestResult("fake command",content);
		var sb = new StringBuilder();
		result.Print(sb);
		await Verifier.Verify(sb.ToString());
	}
}