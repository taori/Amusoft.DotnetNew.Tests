using System;
using System.IO;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.UnitTests.Toolkit;
using Shouldly;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests
{
    public class TemplateSolutionFileTests : TestBase
    {
        public TemplateSolutionFileTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
        {
        }

        [Fact]
        public void SearchingWorks()
        {
            var file = new TemplateSolutionFile(typeof(TemplateSolutionFileTests).Assembly.Location, 6, "Amusoft.DotnetNew.Tests.sln");
            File.Exists(file.SolutionPath).ShouldBeTrue();
            Directory.Exists(file.SolutionDirectory).ShouldBeTrue();
        }

        [Fact]
        public void GetAbsolutePathWorks()
        {
            var file = new TemplateSolutionFile(typeof(TemplateSolutionFileTests).Assembly.Location, 6, "Amusoft.DotnetNew.Tests.sln");
            var absolutePath = file.GetAbsolutePath("Amusoft.DotnetNew.Tests.sln");
            var xslnPath = new CrossPlatformPath(file.SolutionPath);
            xslnPath.VirtualPath.ShouldBe(absolutePath.VirtualPath);
        }

        [Fact]
        public async Task NonSlnThrows()
        {
            await Verifier.Throws(() => new TemplateSolutionFile("bla.txt"));
        }

        [Fact]
        public async Task NoFoundHandling()
        {
            await Verifier.Throws(() => new TemplateSolutionFile(Path.GetTempPath(), 0, "bla44685465.sln"));
        }

        [Fact]
        public async Task DirectoryDoesNotExist()
        {
            await Verifier.Throws(() => new TemplateSolutionFile(Path.GetTempPath() + "2", 0, "bla.sln"));
        }

        [Fact]
        public async Task LocalRelativePathDeclined()
        {
            var file = new TemplateSolutionFile(typeof(TemplateSolutionFileTests).Assembly.Location, 6, "Amusoft.DotnetNew.Tests.sln");
            await Verifier.Throws(() => file.GetAbsolutePath("./Amusoft.DotnetNew.Tests.sln"));
        }
    }
}