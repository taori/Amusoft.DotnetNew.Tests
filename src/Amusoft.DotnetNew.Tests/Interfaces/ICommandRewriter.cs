using System.Text;

namespace Amusoft.DotnetNew.Tests;

internal interface ICommandRewriter
{
	public int ExecutionOrder { get; }
	void Rewrite(StringBuilder stringBuilder);
}