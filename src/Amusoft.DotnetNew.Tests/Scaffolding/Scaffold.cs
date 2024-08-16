﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Templating;
using Amusoft.DotnetNew.Tests.Utility;

namespace Amusoft.DotnetNew.Tests.Scaffolding;

/// <summary>
/// 
/// </summary>
public class Scaffold : IAsyncDisposable
{
	private readonly PathSource _tempPath;
	private readonly TempDirectory _tempDirectory;

	/// <summary>
	/// 
	/// </summary>
	public Scaffold(TempDirectory tempDirectory)
	{
		_tempDirectory = tempDirectory;
		_tempPath = new PathSource(_tempDirectory.Path);
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public ValueTask DisposeAsync()
	{
		_tempDirectory.Dispose();
		
		return ValueTask.CompletedTask;
	}

	/// <summary>
	/// Builds the project at a given path
	/// </summary>
	/// <param name="relativePath">relative path of the project/solution you want to build within the scaffold. e.g. src/SolutionName.sln</param>
	/// <param name="arguments">build arguments</param>
	/// <param name="cancellationToken">cancellation token</param>
	/// <param name="verbosity"></param>
	public async Task BuildAsync(string relativePath, string? arguments, CancellationToken cancellationToken, Verbosity verbosity = default)
	{
		await CLI.DotnetNew.BuildAsync(_tempPath.PathTranslator.GetAbsolutePath(relativePath).OriginalPath, arguments, verbosity, cancellationToken);
	}

	/// <summary>
	/// Builds the project at a given path
	/// </summary>
	/// <param name="relativePath">relative path of the project/solution you want to build within the scaffold. e.g. src/SolutionName.sln</param>
	/// <param name="arguments">build arguments</param>
	/// <param name="cancellationToken">cancellation token</param>
	/// <param name="verbosity"></param>
	public async Task RestoreAsync(string relativePath, string? arguments, CancellationToken cancellationToken, Verbosity verbosity = default)
	{
		await CLI.DotnetNew.RestoreAsync(_tempPath.PathTranslator.GetAbsolutePath(relativePath).OriginalPath, arguments, verbosity, cancellationToken);
	}

	/// <summary>
	/// Gets all paths within the temp directory with their relative paths
	/// </summary>
	/// <returns></returns>
	public IEnumerable<string> GetDirectoryContents()
	{
		return Files().OrderBy(d => d);

		IEnumerable<string> Files()
		{
			foreach (var fullPath in Directory.EnumerateFiles(_tempPath.Directory.OriginalPath, "*", SearchOption.AllDirectories))
			{
				yield return _tempPath.PathTranslator.GetRelativePath(fullPath).VirtualPath;
			}
		}
	}
}