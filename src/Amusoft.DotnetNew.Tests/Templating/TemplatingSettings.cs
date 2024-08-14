using System;
using Amusoft.DotnetNew.Tests.Diagnostics;

namespace Amusoft.DotnetNew.Tests.Templating;

/// <summary>
/// Core API for customization
/// </summary>
public class TemplatingSettings
{
	/// <summary>
	/// Pattern used for printing data
	/// </summary>
	public PrintDelegate PrintPattern { get; init; } = (token, content) 
		=> $"---{token}---" + Environment.NewLine + Environment.NewLine + content + Environment.NewLine + Environment.NewLine;
}