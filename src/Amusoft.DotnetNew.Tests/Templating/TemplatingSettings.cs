using System;
using System.Collections.Generic;
using System.Linq;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Scaffolding;

namespace Amusoft.DotnetNew.Tests.Templating;

/// <summary>
/// Core API for customization
/// </summary>
public record class TemplatingSettings
{
	/// <summary>
	/// Pattern used for printing data
	/// </summary>
	public PrintDelegate PrintPattern { get; init; } = (token, content) 
		=> $"---{token}---" + Environment.NewLine + Environment.NewLine + content + Environment.NewLine + Environment.NewLine;
	
	/// <summary>
	/// Default filter applied for <see cref="Scaffold.GetAllFileContentsAsync"/> 
	/// </summary>
	public RelativeFileFilter GetAllFileContentsFilter { get; init; } = GetAllFileContentsFilterImpl;

	private static readonly HashSet<string> FilteredExtensions = new([".jpg", ".png", ".gif"]);
	
	private static bool GetAllFileContentsFilterImpl(string relativepath)
	{
		return FilteredExtensions.Any(end => relativepath.EndsWith(end));
	}
}