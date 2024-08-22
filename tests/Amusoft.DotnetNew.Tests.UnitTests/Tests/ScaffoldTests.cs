using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Scaffolding;
using Amusoft.DotnetNew.Tests.Templating;
using Moq;
using Shared.TestSdk;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class ScaffoldTests : TestBase
{
	[Fact]
	public async Task GetAllFileContentsDefault()
	{
		var scaffold = CreateFakeScaffold();
		var files = await scaffold.GetAllFileContentsAsync().ToListAsync();
		await Verifier.Verify(files);
	}
	
	[Fact]
	public async Task FilterTxt()
	{
		var scaffold = CreateFakeScaffold();
		var files = await scaffold.GetAllFileContentsAsync(d => d.EndsWith(".txt")).ToListAsync();
		await Verifier.Verify(files);
	}
	
	[Fact]
	public async Task DefaultsInstanceCanOverride()
	{
		var scaffold = CreateFakeScaffold();
		var settings = TemplatingDefaults.Instance with
		{
			GetAllFileContentsFilter = path => path.Contains("a/b")
		};
		TemplatingDefaults.Instance = settings;
		var files = await scaffold.GetAllFileContentsAsync().ToListAsync();
		await Verifier.Verify(files);
	}
	
	[Fact]
	public void DoubleDisposeDoesNotThrow()
	{
		using (var scaffold = CreateFakeScaffold())
			scaffold.Dispose();
		
		Assert.True(true);
	}
	
	[Fact]
	public async Task BuildCalled()
	{
		var directoryMock = new Mock<ITempDirectory>();
		directoryMock.Setup(d => d.Path).Returns(new PathSource(Path.GetTempPath()));
		var cliMock = new Mock<IDotnetCli>();
		cliMock
			.Setup(d => d.BuildAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Verbosity>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
			;
		var scaffold = new Scaffold(directoryMock.Object, cliMock.Object);
		await scaffold.BuildAsync(String.Empty, String.Empty, CancellationToken.None, Verbosity.Minimal);
		cliMock.Verify(d => d.BuildAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Verbosity>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Once);
	}
	
	[Fact]
	public async Task RestoreCalled()
	{
		var directoryMock = new Mock<ITempDirectory>();
		directoryMock.Setup(d => d.Path).Returns(new PathSource(Path.GetTempPath()));
		var cliMock = new Mock<IDotnetCli>();
		cliMock
			.Setup(d => d.RestoreAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Verbosity>(), It.IsAny<CancellationToken>()))
			;
		var scaffold = new Scaffold(directoryMock.Object, cliMock.Object);
		await scaffold.RestoreAsync(String.Empty, String.Empty, CancellationToken.None, Verbosity.Minimal);
		cliMock.Verify(d => d.RestoreAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Verbosity>(), It.IsAny<CancellationToken>()), Times.Once);
	}
	
	[Fact]
	public async Task TestCalled()
	{
		var directoryMock = new Mock<ITempDirectory>();
		directoryMock.Setup(d => d.Path).Returns(new PathSource(Path.GetTempPath()));
		var cliMock = new Mock<IDotnetCli>();
		cliMock
			.Setup(d => d.TestAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Verbosity>(), It.IsAny<CancellationToken>()))
			;
		var scaffold = new Scaffold(directoryMock.Object, cliMock.Object);
		await scaffold.TestAsync(String.Empty, String.Empty, CancellationToken.None, Verbosity.Minimal);
		cliMock.Verify(d => d.TestAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Verbosity>(), It.IsAny<CancellationToken>()), Times.Once);
	}

	private Scaffold CreateFakeScaffold()
	{
		var values = new Dictionary<string, string>()
		{
			["a/a.txt"] = "content a",
			["a/b.txt"] = "content b",
			["c.jpg"] = "content c",
		};
		var directoryMock = new Mock<ITempDirectory>();
		directoryMock
			.Setup(d => d.GetRelativePaths())
			.Returns(values.Keys);
		directoryMock
			.Setup(d => d.GetFileContentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((string path, CancellationToken ct) => values[path]);
		var cliMock = new Mock<IDotnetCli>();
		var scaffold = new Scaffold(directoryMock.Object, cliMock.Object);
		return scaffold;
	}

	public ScaffoldTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
	{
	}
}