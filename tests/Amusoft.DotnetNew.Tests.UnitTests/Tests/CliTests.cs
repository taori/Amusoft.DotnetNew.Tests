using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Extensions;
using VerifyXunit;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class CliTests
{
	[Fact]
	public async Task VerbosityExtensions()
	{
		var values = new Dictionary<Verbosity, string>();
		foreach (var verbosity in Enum.GetValues<Verbosity>())
		{
			values.Add(verbosity, verbosity.ToVerbosityText());
		}

		await Verifier.Verify(values);
	}
}