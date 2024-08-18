using System.IO;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Utility;
using Shared.TestSdk;
using Shouldly;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class CrossPlatformPathTests : TestBase
{

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

	[Fact]
	public async Task ToStringOutput()
	{
		var a = new CrossPlatformPath(Path.GetTempPath()).ToString();
		await Verifier.Verify(a);
	}

	public CrossPlatformPathTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
	{
	}
}