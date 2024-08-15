using System;
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
	
	/// <summary>
	/// Logger
	/// </summary>
	public CommandLogger Logger { get; } = new();

	internal static string? ToString(PrintKind kind) => Current?.Logger.ToString(kind);

	private static void ExecuteRecursive(Action<LoggingScope> action, LoggingScope? scope)
	{
		if (scope is not null)
		{
			action(scope);
			
			if (scope.ParentScope is { Connected: true } parentScope)
			{
				ExecuteRecursive(action, parentScope);			
			}
		}
	}

	internal static void TryAddRewriter(ICommandRewriter item)
	{
		ExecuteRecursive(c =>
			{
				c.Logger.AddRewriter(item);
			},
			Current
		);
	}

	internal static void TryAddInvocation(string item) => TryAddInvocation(new DotnetCommand(item));
	
	internal static void TryAddInvocation(ICommandInvocation item)
	{
		ExecuteRecursive(c =>
			{
				c.Logger.AsReceiver().AddInvocation(item);
			},
			Current
		);
	}

	internal static void TryAddResult(ICommandResponse item)
	{
		ExecuteRecursive(c =>
			{
				c.Logger.AsReceiver().AddResult(item);
			},
			Current
		);
	}
}