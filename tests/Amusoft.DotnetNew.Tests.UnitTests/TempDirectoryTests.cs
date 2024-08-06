using System.IO;
using Amusoft.DotnetNew.Tests;
using Shouldly;
using Xunit;

public class TempDirectoryTests
{
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