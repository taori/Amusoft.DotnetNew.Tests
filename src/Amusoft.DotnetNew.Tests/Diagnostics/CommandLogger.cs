using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amusoft.DotnetNew.Tests.Interfaces;

namespace Amusoft.DotnetNew.Tests.Diagnostics;

/// <summary>
/// Logs the output of commands
/// </summary>
public class CommandLogger
{
	private readonly List<ICommandData> _commandData = new();
	private readonly HashSet<ICommandRewriter> _rewriters = new();
	
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
	/// prints the output as string
	/// </summary>
	/// <param name="kind"></param>
	/// <returns></returns>
	public string ToString(PrintKind kind)
	{
		var sb = new StringBuilder();
		Print(sb, kind);
		return sb.ToString();
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
			PrintKind.All => CommandData,
			PrintKind.Invocations => CommandData.OfType<ICommandInvocation>(),
			PrintKind.Responses => CommandData.OfType<ICommandResponse>(),
			_ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
		};

		foreach (var printable in data)
		{
			printable.Print(stringBuilder);
		}

		foreach (var commandRewriter in _rewriters.OrderBy(d => d.ExecutionOrder))
		{
			commandRewriter.Rewrite(stringBuilder);
		}
	}
}