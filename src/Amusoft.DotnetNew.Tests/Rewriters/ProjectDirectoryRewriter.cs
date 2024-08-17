using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Templating;

namespace Amusoft.DotnetNew.Tests.Rewriters;

internal class ProjectDirectoryRewriter : ICommandRewriter, IEquatable<ProjectDirectoryRewriter>
{
	private readonly ProjectTemplatingContext _context;

	public ProjectDirectoryRewriter(ProjectTemplatingContext context)
	{
		_context = context;
	}
	
	public int ExecutionOrder { get; } = 1;
	
	public void Rewrite(StringBuilder stringBuilder)
	{
		var dir = Path.GetFileName(_context.ProjectTemplatePath.VirtualPath);
		stringBuilder.Replace(_context.ProjectTemplatePath.VirtualPath, $"{{ProjectDir:{dir}}}");
		stringBuilder.Replace(_context.ProjectTemplatePath.OriginalPath, $"{{ProjectDir:{dir}}}");
	}
	
	[ExcludeFromCodeCoverage]
	public bool Equals(ProjectDirectoryRewriter? other)
	{
		if (other is null)
			return false;

		return other._context.ProjectTemplatePath.VirtualPath.Equals(_context.ProjectTemplatePath.VirtualPath, StringComparison.OrdinalIgnoreCase);
	}
}