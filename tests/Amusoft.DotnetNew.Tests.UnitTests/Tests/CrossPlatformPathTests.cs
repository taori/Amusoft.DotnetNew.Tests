using System.IO;
using Amusoft.DotnetNew.Tests.UnitTests.Configuration;
using Amusoft.DotnetNew.Tests.UnitTests.Toolkit;
using Amusoft.DotnetNew.Tests.Utility;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class CrossPlatformPathTests : TestBase
{

	[Fact(Timeout = 10000)]
	public void OriginalPathEquals()
	{
		var a = new CrossPlatformPath(Path.GetTempPath());
		var b = new CrossPlatformPath(Path.GetTempPath());
		a.OriginalPath.Equals(b.OriginalPath).ShouldBeTrue();
	}

	[Fact(Timeout = 10000)]
	public void CrossPlatformPathEquals()
	{
		var a = new CrossPlatformPath(Path.GetTempPath());
		var b = new CrossPlatformPath(Path.GetTempPath());
		a.Equals(b).ShouldBeTrue();
		a.GetHashCode().Equals(b.GetHashCode()).ShouldBeTrue();
	}

	public CrossPlatformPathTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
	{
	}
}