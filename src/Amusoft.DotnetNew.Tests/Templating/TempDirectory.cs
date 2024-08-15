using System;
using System.IO;

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
	}

	/// <summary>
	/// Path of temporary directory
	/// </summary>
	public string Path { get; set; }

	/// <summary>
	/// Dispose
	/// </summary>
	public void Dispose()
	{
		Directory.Delete(Path, true);
	}
}