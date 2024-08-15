using System.IO;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Templating;
using VerifyXunit;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class PathSourceTests
{
	[Theory]
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
	
	[Theory]
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
}