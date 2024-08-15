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
	/// Logger
	/// </summary>
	public CommandLogger Logger { get; } = new();

	internal static string? ToString(PrintKind kind) => Current?.Logger.ToString(kind);
	
	internal static void TryAddRewriter(ICommandRewriter rewriter) => Current?.Logger.AddRewriter(rewriter);
}