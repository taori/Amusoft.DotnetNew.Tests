using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Exceptions;
using Amusoft.DotnetNew.Tests.Scaffolding;
using Amusoft.DotnetNew.Tests.Utility;

namespace Amusoft.DotnetNew.Tests.Interfaces;

/// <summary>
/// Temporary directory
/// </summary>
internal interface ITempDirectory : IDisposable
{
	/// <summary>
	/// Gets the file conents using a relative path
	/// </summary>
	/// <param name="relativePath"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<string> GetFileContentAsync(string relativePath, CancellationToken cancellationToken);

	/// <summary>
	/// Returns all paths within the temporary directory
	/// </summary>
	/// <returns></returns>
	IEnumerable<string> GetRelativePaths();
	
	IPathSource Path { get; }
}

internal interface IPathSource
{
	/// <summary>
	/// Path translator for navigating the file system
	/// </summary>
	PathTranslator PathTranslator { get; }

	/// <summary>
	/// Path to solution directory
	/// </summary>
	CrossPlatformPath Directory { get; }

	/// <summary>
	/// Path to solution file
	/// </summary>
	CrossPlatformPath File { get; }
}

/// <summary>
/// Dotnet CLI surface
/// </summary>
public interface IDotnetCli
{
	/// <summary>
	/// executes dotnet build
	/// </summary>
	/// <param name="fullPath"></param>
	/// <param name="arguments"></param>
	/// <param name="verbosity"></param>
	/// <param name="cancellationToken"></param>
	/// <param name="restore"></param>
	/// <returns></returns>
	Task BuildAsync(string fullPath, string? arguments, Verbosity verbosity, CancellationToken cancellationToken, bool restore);

	/// <summary>
	/// Executes dotnet restore
	/// </summary>
	/// <param name="fullPath"></param>
	/// <param name="arguments"></param>
	/// <param name="verbosity"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task RestoreAsync(string fullPath, string? arguments, Verbosity verbosity, CancellationToken cancellationToken);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="template"></param>
	/// <param name="arguments"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ScaffoldingFailedException"></exception>
	Task<Scaffold> NewAsync(string template, string? arguments, CancellationToken cancellationToken);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="fullPath"></param>
	/// <param name="arguments"></param>
	/// <param name="verbosity"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task TestAsync(string fullPath, string? arguments, Verbosity verbosity, CancellationToken cancellationToken);
}