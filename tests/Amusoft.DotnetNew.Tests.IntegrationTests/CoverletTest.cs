using Amusoft.DotnetNew.Tests.Templating;
using Shouldly;

namespace Amusoft.DotnetNew.Tests.IntegrationTests;

public class CoverletTest
{
    [Fact]
    public void DirectoryExists()
    {
        using (var dir = new TempDirectory())
        {
            Directory.Exists(dir.Path).ShouldBeTrue();
        }
    }
}