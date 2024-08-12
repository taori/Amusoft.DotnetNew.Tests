using System;

namespace Amusoft.DotnetNew.Tests.Rewriters;

internal class BackslashRewriter : ICommandRewriter
{
	public int ExecutionOrder { get; } = int.MinValue;

	public string Rewrite(string data)
	{
		return data.Replace('\\', '/');
	}
}