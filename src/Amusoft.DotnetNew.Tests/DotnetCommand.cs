using System.Text;

namespace Amusoft.DotnetNew.Tests;

internal class DotnetCommand : ICommandInvocation
{
	public string Command { get; }

	public DotnetCommand(string command)
	{
		Command = command;
	}

	public void Print(StringBuilder stringBuilder)
	{
		stringBuilder.Append(TemplatingDefaults.Instance.PrintPattern("Command", Command));
	}
}