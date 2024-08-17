using Amusoft.DotnetNew.Tests.Templating;
using Shouldly;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class TemplateSettingsTests
{
	[Fact]
	public void Settable()
	{
		var a = new TemplatingSettings()
		{
			PrintPattern = (token, content) => content,
		};
		
		a.ShouldNotBeNull();
	}
}