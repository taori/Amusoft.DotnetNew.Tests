﻿using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Exceptions;
using Amusoft.DotnetNew.Tests.Extensions;
using Amusoft.DotnetNew.Tests.Rewriters;
using Amusoft.DotnetNew.Tests.Scaffolding;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.DotnetNew.Tests.Templating;
using Amusoft.DotnetNew.Tests.Utility;

namespace Amusoft.DotnetNew.Tests.CLI;

/// <summary>
/// Dotnet new CLI tool
/// </summary>
public static class DotnetNew
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="template"></param>
	/// <param name="arguments"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ScaffoldingFailedException"></exception>
	public static async Task<Scaffold> NewAsync(string template, string? arguments, CancellationToken cancellationToken)
	{
		var tempDirectory = new TempDirectory();
		var scaffold = new Scaffold(tempDirectory);
		var fullArgs = arguments is not null
			? $"new {template} -o \"{tempDirectory.Path}\" {arguments}"
			: $"new {template} -o \"{tempDirectory.Path}\"";
		
		LoggingScope.TryAddRewriter(new FolderNameAliasRewriter(new CrossPlatformPath(tempDirectory.Path), "Scaffold"));
		var result = await LoggedDotnetCli.RunDotnetCommandAsync(fullArgs, cancellationToken);
		var output = LoggingScope.ToString(PrintKind.All);
		if (!result)
			throw new BuildFailedException(fullArgs, output);
		
		return scaffold;
	}

	/// <summary>
	/// Tries to build with the given arguments
	/// </summary>
	/// <param name="fullPath">path for the build operation</param>
	/// <param name="arguments">build arguments</param>
	/// <param name="verbosity"></param>
	/// <param name="cancellationToken">cancellation token</param>
	/// <exception cref="FileNotFoundException"></exception>
	/// <exception cref="DirectoryNotFoundException"></exception>
	/// <exception cref="ScaffoldingFailedException"></exception>
	public static async Task BuildAsync(string fullPath, string? arguments, Verbosity verbosity, CancellationToken cancellationToken)
	{
		if (!File.Exists(fullPath) && !Directory.Exists(fullPath))
		{
			if (!File.Exists(fullPath))
				throw new FileNotFoundException(fullPath);
			throw new DirectoryNotFoundException(fullPath);
		}
		
		var fullArgs = arguments is null
			? $"build {fullPath} --no-restore -v {verbosity.ToVerbosityText()}"
			: $"build {fullPath} --no-restore -v {verbosity.ToVerbosityText()} {arguments}";
		var result = await LoggedDotnetCli.RunDotnetCommandAsync(fullArgs, cancellationToken);
		var output = LoggingScope.ToString(PrintKind.All);
		if (!result)
			throw new BuildFailedException(fullArgs, output);
	}

	/// <summary>
	/// Restores the project
	/// </summary>
	/// <param name="fullPath"></param>
	/// <param name="arguments"></param>
	/// <param name="verbosity"></param>
	/// <param name="cancellationToken"></param>
	/// <exception cref="FileNotFoundException"></exception>
	/// <exception cref="DirectoryNotFoundException"></exception>
	/// <exception cref="BuildFailedException"></exception>
	public static async Task RestoreAsync(string fullPath, string? arguments, Verbosity verbosity, CancellationToken cancellationToken)
	{
		if (!File.Exists(fullPath) && !Directory.Exists(fullPath))
		{
			if (!File.Exists(fullPath))
				throw new FileNotFoundException(fullPath);
			throw new DirectoryNotFoundException(fullPath);
		}
		
		var fullArgs = arguments is null
			? $"restore \"{fullPath}\" -v {verbosity.ToVerbosityText()} --disable-parallel"
			: $"restore \"{fullPath}\" -v {verbosity.ToVerbosityText()} --disable-parallel {arguments}";
		var result = await LoggedDotnetCli.RunDotnetCommandAsync(fullArgs, cancellationToken);
		var output = LoggingScope.ToString(PrintKind.All);
		if (!result)
			throw new BuildFailedException(fullArgs, output);
	}
}