using Amusoft.DotnetNew.Tests.Utility;
using Shouldly;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class PathTranslatorTests
{
	[Theory]
	[InlineData(@"D:\a\b",@"D:\a\c",@"../c")]
	[InlineData(@"D:\a\b\c.txt",@"D:\a\c",@"../../c")]
	[InlineData(@"D:\a\b",@"D:\a\d",@"../d")]
	[InlineData(@"D:\a\b\c.txt",@"D:\a\d",@"../../d")]
	[InlineData(@"D:\a\b",@"D:\a\c\d.txt",@"../c/d.txt")]
	[InlineData(@"D:\a\b\c.txt",@"D:\a\c\d.txt",@"../../c/d.txt")]
	public void RelativePath(string refDirectory, string absolutePath, string expected)
	{
		var translator = new PathTranslator(refDirectory);
		translator.GetRelativePath(absolutePath).VirtualPath
			.ShouldBe(new CrossPlatformPath(expected).VirtualPath);
	}
}