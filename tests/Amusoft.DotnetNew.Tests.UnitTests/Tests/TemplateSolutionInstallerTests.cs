using System.IO;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Templating;
using Amusoft.DotnetNew.Tests.UnitTests.Toolkit;
using Amusoft.DotnetNew.Tests.Utility;
using Shouldly;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests
{
    public class TemplateSolutionInstallerTests : TestBase
    {
        public TemplateSolutionInstallerTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
        {
        }

        [Fact]
        public void SearchingWorks()
        {
            var file = new TemplateSolutionInstaller(typeof(TemplateSolutionInstallerTests).Assembly.Location, 6, "Amusoft.DotnetNew.Tests.sln");
            File.Exists(file.Solution.File.OriginalPath).ShouldBeTrue();
            Directory.Exists(file.Solution.Directory.OriginalPath).ShouldBeTrue();
        }

        [Fact]
        public void GetAbsolutePathWorks()
        {
            var file = new TemplateSolutionInstaller(typeof(TemplateSolutionInstallerTests).Assembly.Location, 6, "Amusoft.DotnetNew.Tests.sln");
            var absolutePath = file.Solution.PathTranslator.GetAbsolutePath("Amusoft.DotnetNew.Tests.sln");
            absolutePath.ShouldBe(file.Solution.File);
        }

        [Fact]
        public async Task NonSlnThrows()
        {
            await Verifier.Throws(() => new TemplateSolutionInstaller("bla.txt"));
        }

        [Fact]
        public async Task NoFoundHandling()
        {
            await Verifier.Throws(() => new TemplateSolutionInstaller(Path.GetTempPath(), 0, "bla44685465.sln"));
        }

        [Fact]
        public async Task DirectoryDoesNotExist()
        {
            await Verifier.Throws(() => new TemplateSolutionInstaller(Path.GetTempPath() + "2", 0, "bla.sln"));
        }

        [Fact]
        public async Task LocalRelativePathDeclined()
        {
            var file = new TemplateSolutionInstaller(typeof(TemplateSolutionInstallerTests).Assembly.Location, 6, "Amusoft.DotnetNew.Tests.sln");
            await Verifier.Throws(() => file.Solution.PathTranslator.GetAbsolutePath("./Amusoft.DotnetNew.Tests.sln"));
        }
    }
}