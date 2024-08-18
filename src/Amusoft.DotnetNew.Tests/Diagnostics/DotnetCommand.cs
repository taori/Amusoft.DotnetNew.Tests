using System.Diagnostics.CodeAnalysis;
using System.Text;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Templating;

namespace Amusoft.DotnetNew.Tests.Diagnostics;

internal class DotnetCommand : ICommandInvocation
{
	[ExcludeFromCodeCoverage]
	public string Command { get; }

	public DotnetCommand(string command)
	{
		Command = command;
	}

	public void Print(StringBuilder stringBuilder)
	{
		stringBuilder.Append(TemplatingDefaults.Instance.PrintPattern("Command", Command));
	}

	public static implicit operator DotnetCommand(string command) => new(command);
}