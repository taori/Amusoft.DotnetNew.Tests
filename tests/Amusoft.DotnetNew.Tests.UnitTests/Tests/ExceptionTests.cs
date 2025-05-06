using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Exceptions;
using Shared.TestSdk;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class ExceptionTests : TestBase
{
	[Fact]
	public async Task TestScaffoldExceptionMessage()
	{
		var ex = new ScaffoldingFailedException("message", "output");
		await Verifier.Verify(new
		{
			message = ex.Message,
			output = ex.Output
		});
	}

	public ExceptionTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
	{
	}
}