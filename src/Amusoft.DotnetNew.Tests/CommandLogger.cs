using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amusoft.DotnetNew.Tests;

/// <summary>
/// Logs the output of commands
/// </summary>
public class CommandLogger
{
	private readonly List<ICommandData> _commandData = new();
	private readonly List<ICommandRewriter> _rewriters = new();
	
	/// <summary>
	/// Command results
	/// </summary>
	public IReadOnlyList<IPrintable> CommandData => _commandData;
	
	internal void AddRewriter(ICommandRewriter rewriter)
	{
		_rewriters.Add(rewriter);
	}

	internal void AddResult(ICommandData commandResult)
	{
		_commandData.Add(commandResult);
	}
	
	internal void AddInvocation(string command)
	{
		_commandData.Add(new DotnetCommand(command));
	}

	/// <summary>
	/// Prints the collected data
	/// </summary>
	/// <param name="stringBuilder"></param>
	/// <param name="kind"></param>
	public void Print(StringBuilder stringBuilder, PrintKind kind)
	{
		IEnumerable<IPrintable> data = kind switch
		{
			PrintKind.All => _commandData,
			PrintKind.Invocations => _commandData.OfType<ICommandInvocation>(),
			PrintKind.Responses => _commandData.OfType<ICommandResponse>(),
			_ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
		};

		foreach (var printable in data)
		{
			stringBuilder.AppendLine(printable.ToString());
		}
	}
}