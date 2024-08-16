using System.IO;
using Amusoft.DotnetNew.Tests.Templating;
using Amusoft.DotnetNew.Tests.UnitTests.Configuration;
using Amusoft.DotnetNew.Tests.UnitTests.Toolkit;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class TempDirectoryTests : TestBase
{

	[Fact(Timeout = 10000)]
	public void DirectoryExists()
	{
		using (var dir = new TempDirectory())
		{
			Directory.Exists(dir.Path).ShouldBeTrue();
		}
	}

	[Fact(Timeout = 10000)]
	public void DeletedAfterScope()
	{
		var path = string.Empty;
		using (var dir = new TempDirectory())
		{
			path = dir.Path;
		}
		
		Directory.Exists(path).ShouldBeFalse();
	}

	public TempDirectoryTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
	{
	}
}