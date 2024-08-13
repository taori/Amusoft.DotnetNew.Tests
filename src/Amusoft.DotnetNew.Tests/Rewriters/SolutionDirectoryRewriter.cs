using System;
using System.Text;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Templating;

namespace Amusoft.DotnetNew.Tests.Rewriters;

internal class SolutionDirectoryRewriter : ICommandRewriter, IEquatable<SolutionDirectoryRewriter>
{
	private readonly SolutionTemplatingContext _solutionContext;

	public SolutionDirectoryRewriter(SolutionTemplatingContext solutionContext)
	{
		_solutionContext = solutionContext;
	}
	
	public int ExecutionOrder { get; } = 0;
	
	public void Rewrite(StringBuilder stringBuilder)
	{
		stringBuilder.Replace(_solutionContext.SolutionInstaller.Solution.Directory.VirtualPath, "{SolutionDir}");
	}

	public bool Equals(SolutionDirectoryRewriter? other)
	{
		if (other is null)
			return false;

		return _solutionContext.SolutionInstaller.Solution.Directory.VirtualPath.Equals(other._solutionContext.SolutionInstaller.Solution.Directory.VirtualPath);
	}
}