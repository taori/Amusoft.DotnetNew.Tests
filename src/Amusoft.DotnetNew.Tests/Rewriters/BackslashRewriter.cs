using System;
using System.Text;
using Amusoft.DotnetNew.Tests.Interfaces;

namespace Amusoft.DotnetNew.Tests.Rewriters;

internal class BackslashRewriter : ICommandRewriter, IEquatable<BackslashRewriter>
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

	public bool Equals(BackslashRewriter? other)
	{
		return ReferenceEquals(other, this);
	}
}