﻿using System;
using System.Text;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using VerifyXunit;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class CommandResultTests
{
	[Fact]
	private async Task VerifyCommandResult()
	{
		var item = new CommandResult(0, "some output", "some error", true, TimeSpan.Zero);
		var sb = new StringBuilder();
		item.Print(sb);
		await Verifier.Verify(sb.ToString());
	}

	[Fact]
	private async Task VerifyDotnetCommand()
	{
		var dotnetCommand = new DotnetCommand("wasd");
		var sb = new StringBuilder();
		dotnetCommand.Print(sb);
		await Verifier.Verify(sb.ToString());
	}

	[Fact]
	private async Task VerifyBuildResult()
	{
		var content
			= "C:\\Users\\A\\.nuget\\packages\\microsoft.build.tasks.git\\1.0.0\\build\\Microsoft.Build.Tasks.Git.targets(24,5): warning : Unable to locate repository with working directory that contains directory 'C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1'. [C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1\\Project1.csproj::TargetFramework=netstandard2.0]C:\\Users\\A\\.nuget\\packages\\microsoft.build.tasks.git\\1.0.0\\build\\Microsoft.Build.Tasks.Git.targets(47,5): warning : Unable to locate repository with working directory that contains directory 'C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1'. [C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1\\Project1.csproj::TargetFramework=netstandard2.0]C:\\Users\\A\\.nuget\\packages\\microsoft.sourcelink.common\\1.0.0\\build\\Microsoft.SourceLink.Common.targets(52,5): warning : Source control information is not available - the generated source link is empty. [C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1\\Project1.csproj::TargetFramework=netstandard2.0]  Project1 -> C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1\\bin\\Debug\\netstandard2.0\\Project1.dll  Project1.IntegrationTests -> C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\tests\\Project1.IntegrationTests\\bin\\Debug\\net6.0\\Project1.IntegrationTests.dllC:\\Users\\A\\.nuget\\packages\\microsoft.build.tasks.git\\1.0.0\\build\\Microsoft.Build.Tasks.Git.targets(24,5): warning : Unable to locate repository with working directory that contains directory 'C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1'. [C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1\\Project1.csproj::TargetFramework=net6.0]C:\\Users\\A\\.nuget\\packages\\microsoft.build.tasks.git\\1.0.0\\build\\Microsoft.Build.Tasks.Git.targets(47,5): warning : Unable to locate repository with working directory that contains directory 'C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1'. [C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1\\Project1.csproj::TargetFramework=net6.0]C:\\Users\\A\\.nuget\\packages\\microsoft.sourcelink.common\\1.0.0\\build\\Microsoft.SourceLink.Common.targets(52,5): warning : Source control information is not available - the generated source link is empty. [C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1\\Project1.csproj::TargetFramework=net6.0]  Project1 -> C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1\\bin\\Debug\\net6.0\\Project1.dll  Project1.UnitTests -> C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\tests\\Project1.UnitTests\\bin\\Debug\\net6.0\\Project1.UnitTests.dllBuild succeeded.C:\\Users\\A\\.nuget\\packages\\microsoft.build.tasks.git\\1.0.0\\build\\Microsoft.Build.Tasks.Git.targets(24,5): warning : Unable to locate repository with working directory that contains directory 'C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1'. [C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1\\Project1.csproj::TargetFramework=netstandard2.0]C:\\Users\\A\\.nuget\\packages\\microsoft.build.tasks.git\\1.0.0\\build\\Microsoft.Build.Tasks.Git.targets(47,5): warning : Unable to locate repository with working directory that contains directory 'C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1'. [C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1\\Project1.csproj::TargetFramework=netstandard2.0]C:\\Users\\A\\.nuget\\packages\\microsoft.sourcelink.common\\1.0.0\\build\\Microsoft.SourceLink.Common.targets(52,5): warning : Source control information is not available - the generated source link is empty. [C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1\\Project1.csproj::TargetFramework=netstandard2.0]C:\\Users\\A\\.nuget\\packages\\microsoft.build.tasks.git\\1.0.0\\build\\Microsoft.Build.Tasks.Git.targets(24,5): warning : Unable to locate repository with working directory that contains directory 'C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1'. [C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1\\Project1.csproj::TargetFramework=net6.0]C:\\Users\\A\\.nuget\\packages\\microsoft.build.tasks.git\\1.0.0\\build\\Microsoft.Build.Tasks.Git.targets(47,5): warning : Unable to locate repository with working directory that contains directory 'C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1'. [C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1\\Project1.csproj::TargetFramework=net6.0]C:\\Users\\A\\.nuget\\packages\\microsoft.sourcelink.common\\1.0.0\\build\\Microsoft.SourceLink.Common.targets(52,5): warning : Source control information is not available - the generated source link is empty. [C:\\Users\\A\\AppData\\Local\\Temp\\6e95ba419fa843259d960d2c4f9c4d18\\src\\Project1\\Project1.csproj::TargetFramework=net6.0]    6 Warning(s)    0 Error(s)Time Elapsed 00:00:01.07";
		var dotnetCommand = new BuildResult("cli args", content);
		var sb = new StringBuilder();
		dotnetCommand.Print(sb);
		await Verifier.Verify(sb.ToString());
	}
}