using System;
using System.IO;
using System.Runtime.CompilerServices;
using Amusoft.DotnetNew.Tests.Internals;
using Amusoft.DotnetNew.Tests.Utility;
using Shouldly;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class PathHelperTests
{
	[Theory]
	[InlineData("/home/runner/work/Amusoft.DotnetNew.Tests/Amusoft.DotnetNew.Tests/tests/Resources/dotnet-library-repo/.template.config/template.json"
		, 2, "/home/runner/work/Amusoft.DotnetNew.Tests/Amusoft.DotnetNew.Tests/tests/Resources/dotnet-library-repo")]
	[InlineData("/home/runner/work/Amusoft.DotnetNew.Tests/Amusoft.DotnetNew.Tests/tests/Resources/dotnet-library-repo/.template.config/template.json"
		, 3, "/home/runner/work/Amusoft.DotnetNew.Tests/Amusoft.DotnetNew.Tests/tests/Resources")]
	public void PathTrimming(string input, int trim, string expected)
	{
		var o = PathHelper.AbsoluteTrimPathEnd(input, trim);
		var r = new CrossPlatformPath(o);
		r.ShouldBe(new CrossPlatformPath(expected));
	}
}