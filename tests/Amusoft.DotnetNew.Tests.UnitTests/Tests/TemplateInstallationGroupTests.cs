using Amusoft.DotnetNew.Tests.Templating;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class TemplateInstallationGroupTests
{
	[Fact]
	public void MultiDisposeNoThrow()
	{
		using (var g = new TemplateInstallationGroup())
			g.Dispose();
	}
}