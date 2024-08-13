using System.Text;

namespace Amusoft.DotnetNew.Tests.Interfaces;

internal interface ICommandRewriter
{
	public int ExecutionOrder { get; }
	void Rewrite(StringBuilder stringBuilder);
}