using System;
using System.Threading;
using Amusoft.DotnetNew.Tests.UnitTests.Helpers;
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
		TemplateSolutionInstallerHelper.GetLocalSolution().UninstallTemplatesFromDirectory("../tests/Resources", CancellationToken.None);
	}
}