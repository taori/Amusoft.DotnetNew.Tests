using System;
using System.Text;

namespace Amusoft.DotnetNew.Tests.Rewriters;

internal class BackslashRewriter : ICommandRewriter
{
	public static readonly BackslashRewriter Instance = new();

	private BackslashRewriter()
	{
	}
	
	public int ExecutionOrder => int.MinValue;

	public void Rewrite(StringBuilder stringBuilder)
	{
		stringBuilder.Replace('\\', '/');
	}
}