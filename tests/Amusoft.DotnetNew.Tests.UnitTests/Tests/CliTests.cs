﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Extensions;
using Amusoft.DotnetNew.Tests.UnitTests.Configuration;
using Amusoft.DotnetNew.Tests.UnitTests.Toolkit;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class CliTests : TestBase
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

	public CliTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
	{
	}
}