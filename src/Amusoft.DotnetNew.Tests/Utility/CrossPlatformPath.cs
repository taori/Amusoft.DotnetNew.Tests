using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Amusoft.DotnetNew.Tests.Utility;

/// <summary>
/// Unix and Windows systems have different path seperators which makes testing templates harder
/// if you for example develop on windows but run your tests on unix
/// </summary>
public class CrossPlatformPath : IEquatable<CrossPlatformPath>
{
	private readonly string _originalPath;
	private readonly string _virtualPath;
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="originalPath"></param>
	public CrossPlatformPath(string originalPath)
	{
		_originalPath = originalPath;
		_virtualPath =
			RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
			? originalPath.Replace('\\', '/')
			: originalPath;
	}

	/// <summary>
	/// Native path
	/// </summary>
	public string OriginalPath => _originalPath;

	/// <summary>
	/// Path that is the same accross multiple operating systems
	/// </summary>
	public string VirtualPath => _virtualPath;

	/// <summary>
	/// Why are you looking at this tooltip?
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return $"{OriginalPath} -> {VirtualPath}";
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="other"></param>
	/// <returns></returns>
	[ExcludeFromCodeCoverage]
	public bool Equals(CrossPlatformPath? other)
	{
		if (ReferenceEquals(null, other))
			return false;
		if (ReferenceEquals(this, other))
			return true;
		return _virtualPath == other._virtualPath;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	[ExcludeFromCodeCoverage]
	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj))
			return false;
		if (ReferenceEquals(this, obj))
			return true;
		if (obj.GetType() != this.GetType())
			return false;
		return Equals((CrossPlatformPath)obj);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public override int GetHashCode()
	{
		return _virtualPath.GetHashCode();
	}
}