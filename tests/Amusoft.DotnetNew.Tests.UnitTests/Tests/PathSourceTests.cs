using System.IO;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Templating;
using Amusoft.DotnetNew.Tests.UnitTests.Configuration;
using Amusoft.DotnetNew.Tests.UnitTests.Toolkit;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class PathSourceTests : TestBase
{
	[Theory(Timeout = 10000)]
	[InlineData("Filename.txt")]
	[InlineData("Filename2.txt")]
	public async Task File(string path)
	{
		var pathSource = new PathSource(Path.Combine(Path.GetTempPath(), path));
		await Verifier.Verify(new
			{
				File = pathSource.File,
				Directory = pathSource.Directory,
			}
		).UseParameters(path);
	}
	
	[Theory(Timeout = 10000)]
	[InlineData("asdf")]
	[InlineData("asdf2")]
	public async Task Directory(string subPath)
	{
		var pathSource = new PathSource(Path.Combine(Path.GetTempPath(), subPath));
		await Verifier.Verify(new
			{
				File = pathSource.File,
				Directory = pathSource.Directory,
			}
		).UseParameters(subPath);
	}

	public PathSourceTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
	{
	}
}