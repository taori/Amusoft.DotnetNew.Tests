using System.IO;
using Amusoft.DotnetNew.Tests.Utility;

namespace Amusoft.DotnetNew.Tests.Templating;

/// <summary>
/// Solution path details
/// </summary>
public class Solution
{
	internal Solution(string absoluteSlnPath)
	{
		File = new CrossPlatformPath(absoluteSlnPath);
		Directory = new CrossPlatformPath(Path.GetDirectoryName(absoluteSlnPath)!);
		PathTranslator = new PathTranslator(Path.GetDirectoryName(absoluteSlnPath)!);
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