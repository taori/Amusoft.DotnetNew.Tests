using System;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Configuration;

[CollectionDefinition("AssemblyInitializer")]
public class AssemblyInitializer: IDisposable, ICollectionFixture<AssemblyInitializer>
{
	public AssemblyInitializer()
	{
	}

	public void Dispose()
	{
	}
}