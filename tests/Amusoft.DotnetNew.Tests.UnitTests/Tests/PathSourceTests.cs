using System.IO;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Templating;
using Shared.TestSdk;
using Shouldly;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class PathSourceTests : TestBase
{
	[Theory(Timeout = 10000)]
	[InlineData("Filename.txt")]
	[InlineData("Filename2.txt")]
	public async Task FileDoesNotExist(string path)
	{
		var pathSource = new PathSource(Path.Combine(Path.GetTempPath(), path));
		await Verifier.Verify(new
			{
				File = pathSource.File,
				Directory = pathSource.Directory,
			}
		).UseParameters(path);
	}
	
	[Fact]
	public void FileExists()
	{
		var pathSource = new PathSource(Path.GetTempFileName());
		pathSource.Directory.VirtualPath.ShouldNotBe(pathSource.File.VirtualPath);
	}
	
	[Theory(Timeout = 10000)]
	[InlineData("asdf")]
	[InlineData("asdf2")]
	public async Task DirectoryDoesNotExistPhysical(string subPath)
	{
		var pathSource = new PathSource(Path.Combine(Path.GetTempPath(), subPath));
		await Verifier.Verify(new
			{
				File = pathSource.File,
				Directory = pathSource.Directory,
			}
		).UseParameters(subPath);
	}
	
	[Fact(Timeout = 10000)]
	public async Task DirectoryDoesNotExist()
	{
		var pathSource = new PathSource("tmp2341");
		await Verifier.Verify(new
			{
				File = pathSource.File.VirtualPath,
				Directory = pathSource.Directory.VirtualPath,
			}
		);
	}
	
	[Fact(Timeout = 10000)]
	public async Task DirectoryDoesExist()
	{
		var pathSource = new PathSource(Path.GetTempPath());
		await Verifier.Verify(new
			{
				File = pathSource.File.VirtualPath,
				Directory = pathSource.Directory.VirtualPath,
			}
		);
	}

	public PathSourceTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
	{
	}
}