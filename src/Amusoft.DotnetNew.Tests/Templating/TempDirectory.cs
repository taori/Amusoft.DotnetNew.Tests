using System;
using System.IO;

namespace Amusoft.DotnetNew.Tests.Templating;

internal class TempDirectory : IDisposable
{
	public TempDirectory()
	{
		Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString("N")).TrimEnd(System.IO.Path.DirectorySeparatorChar);
		Directory.CreateDirectory(Path);
	}

	public string Path { get; set; }

	public void Dispose()
	{
		Directory.Delete(Path, true);
	}
}