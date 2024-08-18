using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
[ExcludeFromCodeCoverage(Justification = "https://github.com/taori/Amusoft.DotnetNew.Tests/issues/1")]
public class Scaffold : IDisposable
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
	/// Builds the project at a given path
	/// </summary>
	/// <param name="relativePath">relative path of the project/solution you want to build within the scaffold. e.g. src/SolutionName.sln</param>
	/// <param name="arguments">build arguments</param>
	/// <param name="cancellationToken">cancellation token</param>
	/// <param name="verbosity"></param>
	/// <param name="restore"></param>
	public async Task BuildAsync(string relativePath, string? arguments, CancellationToken cancellationToken, Verbosity verbosity = default, bool restore = false)
	{
		await Dotnet.BuildAsync(_tempPath.PathTranslator.GetAbsolutePath(relativePath).OriginalPath, arguments, verbosity, cancellationToken, restore).ConfigureAwait(false);
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
		await Dotnet.RestoreAsync(_tempPath.PathTranslator.GetAbsolutePath(relativePath).OriginalPath, arguments, verbosity, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// Returns the file contents for a given relative path
	/// </summary>
	/// <param name="relativePath">path relative to scaffold folder</param>
	/// <param name="cancellationToken">cancellation token</param>
	/// <returns></returns>
	public async Task<string> GetFileContentAsync(string relativePath, CancellationToken cancellationToken = default)
	{
		var path = _tempPath.PathTranslator.GetAbsolutePath(relativePath);
		return await File.ReadAllTextAsync(path.OriginalPath, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// Gets all paths within the temp directory with their relative paths
	/// </summary>
	/// <returns></returns>
	public IEnumerable<string> GetRelativeDirectoryPaths()
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