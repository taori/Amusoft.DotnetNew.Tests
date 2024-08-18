using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Templating;
using Amusoft.DotnetNew.Tests.UnitTests.Configuration;
using Amusoft.DotnetNew.Tests.UnitTests.Helpers;
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

        [Fact]
        public void SearchingWorks()
        {
            var file = TemplateSolutionInstallerHelper.CreateLocalSolution();
            File.Exists(file.Solution.File.OriginalPath).ShouldBeTrue();
            Directory.Exists(file.Solution.Directory.OriginalPath).ShouldBeTrue();
        }

        [Fact]
        public void GetAbsolutePathWorks()
        {
            var file = TemplateSolutionInstallerHelper.CreateLocalSolution();
            var absolutePath = file.Solution.PathTranslator.GetAbsolutePath("Amusoft.DotnetNew.Tests.sln");
            absolutePath.ShouldBe(file.Solution.File);
        }

        [Fact]
        public async Task InstallFailsIfDirectoryDoesNotExist()
        {
            var file = TemplateSolutionInstallerHelper.CreateLocalSolution();
            await Verifier.ThrowsTask(() => file.InstallTemplateAsync("../nonexistingfolder", CancellationToken.None));
        }

        [Fact(Timeout = 10000)]
        public async Task NonSlnThrows()
        {
            await Verifier.Throws(() => new TemplateSolution("bla.txt"));
        }

        [Fact(Timeout = 10000)]
        public async Task NoFoundHandling()
        {
            await Verifier.Throws(() => new TemplateSolution(Path.GetTempPath(), 0, "bla44685465.sln"));
        }

        [Fact(Timeout = 10000)]
        public async Task FileNotFound()
        {
            await Verifier.Throws(() => new TemplateSolution(Path.Combine(Path.GetTempPath(), "somefile.sln")));
        }

        [Fact(Timeout = 10000)]
        public async Task DirectoryDoesNotExist()
        {
            await Verifier.Throws(() => new TemplateSolution(Path.GetTempPath() + "2", 0, "bla.sln"));
        }

        [Fact(Timeout = 10000)]
        public async Task LocalRelativePathDeclined()
        {
            var file = TemplateSolutionInstallerHelper.CreateLocalSolution();
            await Verifier.Throws(() => file.Solution.PathTranslator.GetAbsolutePath("./Amusoft.DotnetNew.Tests.sln"));
        }

        public TemplateSolutionInstallerTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
        {
        }
    }
}