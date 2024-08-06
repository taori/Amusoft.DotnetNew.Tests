using System.IO;
using Amusoft.DotnetNew.Tests.UnitTests.Toolkit;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests;

public class CrossPlatformPathTests : TestBase
{
	public CrossPlatformPathTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
	{
	}

	[Fact]
	public void OriginalPathEquals()
	{
		var a = new CrossPlatformPath(Path.GetTempPath());
		var b = new CrossPlatformPath(Path.GetTempPath());
		a.OriginalPath.Equals(b.OriginalPath).ShouldBeTrue();
	}

	[Fact]
	public void CrossPlatformPathEquals()
	{
		var a = new CrossPlatformPath(Path.GetTempPath());
		var b = new CrossPlatformPath(Path.GetTempPath());
		a.Equals(b).ShouldBeTrue();
		a.GetHashCode().Equals(b.GetHashCode()).ShouldBeTrue();
	}
}