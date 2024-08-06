using System.IO;
using Amusoft.DotnetNew.Tests.UnitTests.Toolkit;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests;

public class TempDirectoryTests : TestBase
{
	public TempDirectoryTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
	{
	}

	[Fact]
	public void DirectoryExists()
	{
		using (var dir = new TempDirectory())
		{
			Directory.Exists(dir.Path).ShouldBeTrue();
		}
	}

	[Fact]
	public void DeletedAfterScope()
	{
		var path = string.Empty;
		using (var dir = new TempDirectory())
		{
			path = dir.Path;
		}
		
		Directory.Exists(path).ShouldBeFalse();
	}
}