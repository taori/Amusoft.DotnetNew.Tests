using System;
using System.Threading.Tasks;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Configuration;

[CollectionDefinition("AssemblyInitializer")]
public class AssemblyInitializer: IAsyncLifetime, ICollectionFixture<AssemblyInitializer>
{
	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public Task DisposeAsync()
	{
		return Task.CompletedTask;
	}
}