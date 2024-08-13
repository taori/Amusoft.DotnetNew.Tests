using System;
using System.IO;

namespace Amusoft.DotnetNew.Tests.Utility;

/// <summary>
/// Utility class to provide pathing details
/// </summary>
public class PathTranslator
{
	private readonly CrossPlatformPath _referenceDirectory;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="referenceDirectory">absolute path to directory</param>
	internal PathTranslator(string referenceDirectory)
	{
		_referenceDirectory = new CrossPlatformPath(referenceDirectory.TrimEnd(Path.DirectorySeparatorChar));
	}
	
	/// <summary>
	/// Retrieves the absolute path based on the relative path in relation to the reference path
	/// </summary>
	/// <param name="relativePath">e.g. ../a/b/c</param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public CrossPlatformPath GetAbsolutePath(string relativePath)
	{
		if (relativePath.StartsWith("./") || relativePath.StartsWith(".\\"))
			throw new ArgumentException("Relative paths should not start with the current location pattern or resolution would fail.");
		
		var absoluteUri = new Uri(new Uri(_referenceDirectory.VirtualPath + Path.DirectorySeparatorChar, UriKind.Absolute), new Uri(relativePath, UriKind.Relative));
		
		return new CrossPlatformPath(absoluteUri.AbsolutePath);
	}

	/// <summary>
	/// Retrieves the relative path based on the absolute path in relation to the reference path
	/// </summary>
	/// <param name="absolutePath"></param>
	/// <returns></returns>
	public CrossPlatformPath GetRelativePath(string absolutePath)
	{
		var baseUri = new Uri(_referenceDirectory.VirtualPath.TrimEnd('/') + '/', UriKind.Absolute);
		var refUri = new Uri(new CrossPlatformPath(absolutePath).VirtualPath.TrimEnd('/') + '/', UriKind.Absolute);
		var relUri = baseUri.MakeRelativeUri(refUri);
		return new CrossPlatformPath(relUri.OriginalString.TrimEnd('/'));
	}
}