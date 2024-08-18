using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using VerifyXunit;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class CommandResultTests
{
	[Fact]
	private async Task RenderCheck()
	{
		var item = new CommandResult(0, "some output", "some error", true, TimeSpan.Zero);
		await Verifier.Verify(item);
	}
}