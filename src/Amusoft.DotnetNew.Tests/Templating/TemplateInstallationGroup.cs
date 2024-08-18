using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Amusoft.DotnetNew.Tests.Templating;

/// <summary>
/// Group of installations
/// </summary>
public class TemplateInstallationGroup : IDisposable
{
	private List<TemplateInstallation> _installations = new();
	
	/// <summary>
	/// Installations which were made
	/// </summary>
	[ExcludeFromCodeCoverage]
	public IReadOnlyList<TemplateInstallation> Installations => _installations;
	
	internal void Add(TemplateInstallation installation)
	{
		_installations.Add(installation);
	}

	private bool _disposed;

	/// <summary>
	/// Dispose
	/// </summary>
	public void Dispose()
	{
		if (_disposed)
			return;
		_disposed = true;
		
		foreach (var installation in _installations)
		{
			installation.Dispose();
		}
		
		GC.SuppressFinalize(this);
	}
}