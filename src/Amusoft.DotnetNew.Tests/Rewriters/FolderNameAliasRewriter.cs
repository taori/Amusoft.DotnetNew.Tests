using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Utility;

namespace Amusoft.DotnetNew.Tests.Rewriters;

internal class FolderNameAliasRewriter : ICommandRewriter, IEquatable<FolderNameAliasRewriter>
{
	public CrossPlatformPath Folder { get; }
	public string Alias { get; }

	public FolderNameAliasRewriter(CrossPlatformPath folder, string alias)
	{
		Folder = folder;
		Alias = alias;
	}
	
	public int ExecutionOrder { get; } = 3;
	
	public void Rewrite(StringBuilder stringBuilder)
	{
		stringBuilder.Replace(Folder.VirtualPath, $"{{{Alias}}}");
		stringBuilder.Replace(Folder.OriginalPath, $"{{{Alias}}}");
	}
	
	[ExcludeFromCodeCoverage]
	public bool Equals(FolderNameAliasRewriter? other)
	{
		if (other is null)
			return false;
		
		return Folder.Equals(other.Folder) && Alias.Equals(other.Alias);
	}
}