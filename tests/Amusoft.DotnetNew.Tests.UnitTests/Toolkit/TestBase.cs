using System;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.UnitTests.Configuration;
using Amusoft.DotnetNew.Tests.UnitTests.Helpers;
using Amusoft.XUnit.NLog.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests.Toolkit;

[Collection("AssemblyInitializer")]
public class TestBase : LoggedTestBase
{
	private readonly AssemblyInitializer _data;

	public TestBase(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper)
	{
		_data = data;
	}
}