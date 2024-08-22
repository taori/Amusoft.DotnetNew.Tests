using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Templating;

namespace Amusoft.DotnetNew.Tests.Scaffolding;

/// <summary>
/// 
/// </summary>
public class Scaffold : IDisposable
{
	private readonly ITempDirectory _tempDirectory;
	private readonly IDotnetCli _cli;

	internal Scaffold(ITempDirectory tempDirectory, IDotnetCli cli)
	{
		_tempDirectory = tempDirectory;
		_cli = cli;
	}

	/// <summary>
	/// Builds the project at a given path
	/// </summary>
	/// <param name="relativePath">relative path of the project/solution you want to build within the scaffold. e.g. src/SolutionName.sln</param>
	/// <param name="arguments">build arguments</param>
	/// <param name="cancellationToken">cancellation token</param>
	/// <param name="verbosity"></param>
	/// <param name="restore"></param>
	public async Task BuildAsync(string relativePath, string? arguments, CancellationToken cancellationToken, Verbosity verbosity = default, bool restore = false)
	{
		await _cli.BuildAsync(_tempDirectory.Path.PathTranslator.GetAbsolutePath(relativePath).OriginalPath, arguments, verbosity, cancellationToken, restore).ConfigureAwait(false);
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
		await _cli.RestoreAsync(_tempDirectory.Path.PathTranslator.GetAbsolutePath(relativePath).OriginalPath, arguments, verbosity, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// Builds the project at a given path
	/// </summary>
	/// <param name="relativePath">relative path of the project/solution you want to build within the scaffold. e.g. src/SolutionName.sln</param>
	/// <param name="arguments">build arguments</param>
	/// <param name="cancellationToken">cancellation token</param>
	/// <param name="verbosity"></param>
	public async Task TestAsync(string relativePath, string? arguments, CancellationToken cancellationToken, Verbosity verbosity = default)
	{
		await _cli.TestAsync(_tempDirectory.Path.PathTranslator.GetAbsolutePath(relativePath).OriginalPath, arguments, verbosity, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// Returns the file contents for a given relative path
	/// </summary>
	/// <param name="relativePath">path relative to scaffold folder</param>
	/// <param name="cancellationToken">cancellation token</param>
	/// <returns></returns>
	public async Task<string> GetFileContentAsync(string relativePath, CancellationToken cancellationToken = default)
	{
		return await _tempDirectory.GetFileContentAsync(relativePath, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// Returns the content of all scaffolded files
	/// </summary>
	/// <param name="filter">true removes a file from the results, false keeps it</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async IAsyncEnumerable<FileContent> GetAllFileContentsAsync(RelativeFileFilter? filter = default, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var filterInstance = filter ?? TemplatingDefaults.Instance.GetAllFileContentsFilter;
		foreach (var relativePath in GetRelativeDirectoryPaths())
		{
			if(filterInstance(relativePath))
				continue;
			
			cancellationToken.ThrowIfCancellationRequested();
			var content = await GetFileContentAsync(relativePath, cancellationToken);
			yield return new FileContent(relativePath, content);
		}
	}

	/// <summary>
	/// Gets all paths within the temp directory with their relative paths
	/// </summary>
	/// <returns></returns>
	public IEnumerable<string> GetRelativeDirectoryPaths()
	{
		return _tempDirectory.GetRelativePaths().OrderBy(d => d);
	}

	private bool _disposed;
	/// <summary>
	/// 
	/// </summary>
	public void Dispose()
	{
		if (_disposed)
			return;
		_disposed = true;
		
		_tempDirectory.Dispose();
		GC.SuppressFinalize(this);
	}
}