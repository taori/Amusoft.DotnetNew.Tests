using System;
using System.IO;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Scopes;

namespace Amusoft.DotnetNew.Tests.Templating;

/// <summary>
/// Temporary directory
/// </summary>
public class TempDirectory : IDisposable
{
	/// <summary>
	/// Constructor
	/// </summary>
	public TempDirectory()
	{
		Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString("N")).TrimEnd(System.IO.Path.DirectorySeparatorChar);
		Directory.CreateDirectory(Path);
		LoggingScope.TryAddResult(new TextResult($"Created temp directory at {Path}"));
	}

	/// <summary>
	/// Path of temporary directory
	/// </summary>
	public string Path { get; set; }

	private bool _disposed;
	/// <summary>
	/// Dispose
	/// </summary>
	public void Dispose()
	{
		if (_disposed)
			return;
		_disposed = true;
		
		Directory.Delete(Path, true);
		LoggingScope.TryAddResult(new TextResult($"Deleting temp directory {Path}"));
		GC.SuppressFinalize(this);
	}
}