using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Toolkit;

namespace Amusoft.DotnetNew.Tests.Scopes;

/// <summary>
/// Logging scope to capture any logs
/// </summary>
public class LoggingScope : AmbientScope<LoggingScope>
{
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="connected">whether to connect the current logging scope to its parent</param>
	public LoggingScope(bool connected = true)
	{
		Connected = connected;
	}

	/// <summary>
	/// Whether to forward rewriters, invocations and results to parent scopes
	/// </summary>
	public bool Connected { get; }
	
	private readonly List<ICommandData> _commandData = new();
	private readonly HashSet<ICommandRewriter> _rewriters = new();
	
	internal void AddRewriter(ICommandRewriter rewriter)
	{
		_rewriters.Add(rewriter);
		if(Connected && ParentScope is { } parentScope)
			parentScope.AddRewriter(rewriter);
	}

	internal void AddResult(ICommandResult result)
	{
		_commandData.Add(result);
		if(Connected && ParentScope is { } parentScope)
			parentScope.AddResult(result);
	}
	
	internal void AddInvocation(ICommandInvocation command)
	{
		_commandData.Add(command);
		if(Connected && ParentScope is { } parentScope)
			parentScope.AddInvocation(command);
	}

	/// <summary>
	/// prints the output as string
	/// </summary>
	/// <param name="kind"></param>
	/// <returns></returns>
	public string ToFullString(PrintKind kind)
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
	internal void Print(StringBuilder stringBuilder, PrintKind kind)
	{
		IEnumerable<IPrintable> data = kind switch
		{
			PrintKind.All => _commandData,
			PrintKind.Invocations => _commandData.OfType<ICommandInvocation>(),
			PrintKind.Responses => _commandData.OfType<ICommandResult>(),
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

	internal static void TryAddRewriter(ICommandRewriter item) => Current?.AddRewriter(item);

	internal static void TryAddInvocation(string item) => TryAddInvocation(new DotnetCommand(item));
	
	internal static void TryAddInvocation(ICommandInvocation item) => Current?.AddInvocation(item);

	internal static void TryAddResult(ICommandResult item) => Current?.AddResult(item);
	
	internal static string? ToFullString() => Current?.ToFullString(PrintKind.All);
}