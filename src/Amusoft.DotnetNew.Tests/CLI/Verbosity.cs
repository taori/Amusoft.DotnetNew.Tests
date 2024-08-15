using System.ComponentModel;

namespace Amusoft.DotnetNew.Tests.CLI;

/// <summary>
/// Verbosity level
/// </summary>
public enum Verbosity
{
	/// <summary>
	/// Minimal
	/// </summary>
	[Description("m")]
	Minimal,
	
	/// <summary>
	/// Quiet
	/// </summary>
	[Description("q")]
	Quiet,
	
	/// <summary>
	/// Normal
	/// </summary>
	[Description("n")]
	Normal,
	
	/// <summary>
	/// Detailed
	/// </summary>
	[Description("d")]
	Detailed,
	
	/// <summary>
	/// Diagnostic
	/// </summary>
	[Description("diag")]
	Diagnostic
}