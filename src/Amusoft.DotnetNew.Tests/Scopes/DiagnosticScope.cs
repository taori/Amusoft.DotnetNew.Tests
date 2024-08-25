using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using Amusoft.Toolkit.Threading;

namespace Amusoft.DotnetNew.Tests.Scopes;

/// <summary>
/// This class can be used to observe the raw input and output that is used
/// for any CLI interaction
/// </summary>
[ExcludeFromCodeCoverage]
public class DiagnosticScope : AmbientScope<DiagnosticScope>
{
	/// <summary>
	/// Content of the diagnostic scope
	/// </summary>
	public StringBuilder Content = new();

	/// <summary>
	/// Adds content if a scope is present
	/// </summary>
	/// <param name="content"></param>
	public static void TryAddContent(string content)
	{
		Current?.Content.AppendLine(content);
	}

	/// <summary>
	/// Adds content if a scope is present
	/// </summary>
	/// <param name="content"></param>
	public static void TryAddJson<T>(T content)
	{
		var options = new JsonSerializerOptions(){WriteIndented = true};
		Current?.Content.AppendLine(JsonSerializer.Serialize(content, options));
	}
}