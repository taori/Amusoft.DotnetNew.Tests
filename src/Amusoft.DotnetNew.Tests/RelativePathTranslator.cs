using System;
using System.IO;

namespace Amusoft.DotnetNew.Tests;

internal class RelativePathTranslator
{
	private readonly string _referenceDirectory;

	public RelativePathTranslator(string referenceDirectory)
	{
		_referenceDirectory = referenceDirectory.TrimEnd(Path.DirectorySeparatorChar);
	}
	
	public CrossPlatformPath GetAbsolutePath(string relativePath)
	{
		if (relativePath.StartsWith("./") || relativePath.StartsWith(".\\"))
			throw new ArgumentException("Relative paths should not start with the current location pattern or resolution would fail.");
		
		var absoluteUri = new Uri(new Uri(_referenceDirectory + Path.DirectorySeparatorChar, UriKind.Absolute), new Uri(relativePath, UriKind.Relative));
		return new CrossPlatformPath(absoluteUri.AbsolutePath);
	}
}