using System;
using System.Text;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Utility;

namespace Amusoft.DotnetNew.Tests.Rewriters;

internal class SolutionDirectoryRewriter : ICommandRewriter, IEquatable<SolutionDirectoryRewriter>
{
	private readonly CrossPlatformPath _solutionDirectory;

	public SolutionDirectoryRewriter(CrossPlatformPath solutionDirectory)
	{
		_solutionDirectory = solutionDirectory;
	}
	
	public int ExecutionOrder { get; } = 0;
	
	public void Rewrite(StringBuilder stringBuilder)
	{
		stringBuilder.Replace(_solutionDirectory.VirtualPath, "{SolutionDir}");
	}

	public bool Equals(SolutionDirectoryRewriter? other)
	{
		if (other is null)
			return false;

		return _solutionDirectory.VirtualPath.Equals(other._solutionDirectory.VirtualPath);
	}
}