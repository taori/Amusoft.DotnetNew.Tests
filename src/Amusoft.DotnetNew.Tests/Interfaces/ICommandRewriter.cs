namespace Amusoft.DotnetNew.Tests;

internal interface ICommandRewriter
{
	public int ExecutionOrder { get; }
	string Rewrite(string data);
}