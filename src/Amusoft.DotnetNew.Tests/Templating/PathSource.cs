using System.IO;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Utility;

namespace Amusoft.DotnetNew.Tests.Templating;

/// <summary>
/// Solution path details
/// </summary>
public class PathSource : IPathSource
{
	internal PathSource(string fullPath)
	{
		var paths = GetPathDetails(fullPath);
		File = new CrossPlatformPath(paths.File);
		Directory = new CrossPlatformPath(paths.Directory);
		PathTranslator = new PathTranslator(paths.Directory);
	}

	private (string File, string Directory) GetPathDetails(string fullPath)
	{
		if (System.IO.File.Exists(fullPath) || System.IO.File.Exists(fullPath))
		{
			return System.IO.File.GetAttributes(fullPath).HasFlag(FileAttributes.Directory) switch
			{
				true => (File: fullPath, Directory: fullPath),
				false => (File: fullPath, Directory: Path.GetDirectoryName(fullPath)!),
			};
		}
		
		// Path does not really exist, so we can only guess by extension
		return Path.GetExtension(fullPath) is { Length: > 0 }
			? (fullPath, Path.GetDirectoryName(fullPath)!)
			: (fullPath, fullPath);
	}

	/// <summary>
	/// Path translator for navigating the file system
	/// </summary>
	public PathTranslator PathTranslator { get; }

	/// <summary>
	/// Path to solution directory
	/// </summary>
	public CrossPlatformPath Directory { get; }

	/// <summary>
	/// Path to solution file
	/// </summary>
	public CrossPlatformPath File { get; }
}