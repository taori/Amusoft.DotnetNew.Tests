using System;
using System.Text;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using VerifyXunit;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class CommandResultTests
{
	[Fact]
	private async Task VerifyCommandResult()
	{
		var item = new CommandResult(0, "some output", "some error", true, TimeSpan.Zero);
		var sb = new StringBuilder();
		item.Print(sb);
		await Verifier.Verify(sb.ToString());
	}

	[Fact]
	private async Task VerifyDotnetCommand()
	{
		var dotnetCommand = new DotnetCommand("wasd");
		var sb = new StringBuilder();
		dotnetCommand.Print(sb);
		await Verifier.Verify(sb.ToString());
	}
}