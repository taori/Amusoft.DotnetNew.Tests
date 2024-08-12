namespace Amusoft.DotnetNew.Tests;

internal class DotnetCommand : ICommandInvocation
{
	public string Command { get; }

	public DotnetCommand(string command)
	{
		Command = command;
	}

	string IPrintable.ToString()
	{
		return TemplatingDefaults.Instance.PrintPattern("Command", Command);
	}
}