using Amusoft.DotnetNew.Tests.Diagnostics;
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
}