using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Templating;
using Shared.TestSdk;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class TempDirectoryTests : TestBase
{

	[Fact]
	public void DirectoryExists()
	{
		using (var dir = new TempDirectory())
		{
			Directory.Exists(dir.Path.Directory.OriginalPath).ShouldBeTrue();
		}
	}

	[Fact]
	public void DeletedAfterScope()
	{
		var path = string.Empty;
		using (var dir = new TempDirectory())
		{
			path = dir.Path.Directory.OriginalPath;
		}
		
		Directory.Exists(path).ShouldBeFalse();
	}
	
	[Fact]
	public void MultiDisposeNoThrow()
	{
		using (var dir = new TempDirectory())
		{
			dir.Dispose();
		}
	}
	
	[Fact]
	public async Task GetFileContentWorks()
	{
		using (var dir = new TempDirectory())
		{
			await File.WriteAllTextAsync(Path.Combine(dir.Path.Directory.OriginalPath, "test.txt"), "test");
			(await dir.GetFileContentAsync("test.txt", CancellationToken.None)).ShouldBe("test");
		}
	}
	
	[Fact]
	public void GetRelativePathsWorks()
	{
		using (var dir = new TempDirectory())
		{
			File.WriteAllText(Path.Combine(dir.Path.Directory.OriginalPath, "test.txt"), "test");
			dir.GetRelativePaths().ShouldBe(["test.txt"]);
		}
	}

	public TempDirectoryTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
	{
	}
}