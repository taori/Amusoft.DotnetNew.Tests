using System.Text;

namespace Amusoft.DotnetNew.Tests.Rewriters;

internal class SolutionDirectoryRewriter : ICommandRewriter
{
	private readonly SolutionTemplatingContext _solutionContext;

	public SolutionDirectoryRewriter(SolutionTemplatingContext solutionContext)
	{
		_solutionContext = solutionContext;
	}
	
	public int ExecutionOrder { get; } = int.MinValue + 1;
	
	public void Rewrite(StringBuilder stringBuilder)
	{
		stringBuilder.Replace(_solutionContext.SolutionFile.SolutionDirectory.VirtualPath, "{SolutionDir}");
	}
}