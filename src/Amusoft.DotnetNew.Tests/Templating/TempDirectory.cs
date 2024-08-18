using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Scopes;

namespace Amusoft.DotnetNew.Tests.Templating;

/// <summary>
/// Temporary directory
/// </summary>
internal class TempDirectory : ITempDirectory
{
	/// <summary>
	/// Constructor
	/// </summary>
	internal TempDirectory()
	{
		var directory = System.IO.Path.Combine(
			System.IO.Path.GetTempPath(), 
			Guid.NewGuid().ToString("N")
		).TrimEnd(System.IO.Path.DirectorySeparatorChar);
		
		Path = new PathSource(directory);
		Directory.CreateDirectory(Path.Directory.OriginalPath);
		LoggingScope.TryAddResult(new TextResult($"Created temp directory at {Path}"));
	}

	public IPathSource Path { get; }

	private bool _disposed;
	/// <summary>
	/// Dispose
	/// </summary>
	public void Dispose()
	{
		if (_disposed)
			return;
		_disposed = true;
		
		Directory.Delete(Path.Directory.OriginalPath, true);
		LoggingScope.TryAddResult(new TextResult($"Deleting temp directory {Path.Directory.OriginalPath}"));
		GC.SuppressFinalize(this);
	}

	public async Task<string> GetFileContentAsync(string relativePath, CancellationToken cancellationToken)
	{
		return await File.ReadAllTextAsync(Path.PathTranslator.GetAbsolutePath(relativePath).OriginalPath, cancellationToken).ConfigureAwait(false);
	}

	public IEnumerable<string> GetRelativePaths()
	{
		return Directory.EnumerateFiles(Path.Directory.OriginalPath, "*", SearchOption.AllDirectories)
			.Select(d => Path.PathTranslator.GetRelativePath(d).VirtualPath);
	}
}