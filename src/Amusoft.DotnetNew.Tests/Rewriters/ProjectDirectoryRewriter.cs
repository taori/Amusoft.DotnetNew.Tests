using System.IO;
using System.Text;

namespace Amusoft.DotnetNew.Tests.Rewriters;

internal class ProjectDirectoryRewriter : ICommandRewriter
{
	private readonly ProjectTemplatingContext _context;

	public ProjectDirectoryRewriter(ProjectTemplatingContext context)
	{
		_context = context;
	}
	
	public int ExecutionOrder { get; } = int.MinValue + 2;
	
	public void Rewrite(StringBuilder stringBuilder)
	{
		var dir = Path.GetFileName(_context.ProjectTemplatePath.VirtualPath);
		stringBuilder.Replace(_context.ProjectTemplatePath.VirtualPath, $"{{ProjectDir:{dir}}}");
	}
}