using System;

namespace Amusoft.DotnetNew.Tests;

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
		_virtualPath = originalPath.Replace('\\', '/');
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
	/// 
	/// </summary>
	/// <param name="other"></param>
	/// <returns></returns>
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