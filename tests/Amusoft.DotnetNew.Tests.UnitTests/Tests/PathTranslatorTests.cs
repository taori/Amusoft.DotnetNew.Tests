﻿using Amusoft.DotnetNew.Tests.Utility;
using Shared.TestSdk;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class PathTranslatorTests : TestBase
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
		
		//C:\Users\A\AppData\Local\Temp\bf129b466576486fb0bfbb43d5f28b49
	}

	[Theory]
	[InlineData(@"C:\Users\A\AppData\Local\Temp\bf129b466576486fb0bfbb43d5f28b49",@"src/All.sln",@"C:/Users/A/AppData/Local/Temp/bf129b466576486fb0bfbb43d5f28b49/src/All.sln")]
	[InlineData(@"C:\Users\A\AppData\Local\Temp\bf129b466576486fb0bfbb43d5f28b49",@"../src/All.sln",@"C:/Users/A/AppData/Local/Temp/src/All.sln")]
	[InlineData(@"C:\Users\A\AppData\Local\Temp\bf129b466576486fb0bfbb43d5f28b49",@"../src/All.slnx",@"C:/Users/A/AppData/Local/Temp/src/All.slnx")]
	public void AbsolutePath(string refpath, string relPath, string expected)
	{
		var translator = new PathTranslator(refpath);
		translator.GetAbsolutePath(relPath).VirtualPath
			.ShouldBe(new CrossPlatformPath(expected).VirtualPath);
	}

	public PathTranslatorTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
	{
	}
}