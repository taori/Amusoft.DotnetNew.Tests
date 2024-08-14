using System;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Rewriters;
using Amusoft.DotnetNew.Tests.Scopes;

namespace Amusoft.DotnetNew.Tests.Templating;

/// <summary>
/// 
/// </summary>
public class TemplateInstallation : IAsyncDisposable
{
	/// <summary>
	/// 
	/// </summary>
	public readonly ProjectTemplatingContext Context;

	private TemplateInstallation(ProjectTemplatingContext context)
	{
		Context = context;
	}

	internal static async Task<TemplateInstallation> CreateAsync(ProjectTemplatingContext projectTemplatingContext, CancellationToken cancellationToken)
	{
		await LoggedDotnetCli.RunDotnetCommandAsync($"new install \"{projectTemplatingContext.ProjectTemplatePath.OriginalPath}\"", cancellationToken);
		var result = new TemplateInstallation(projectTemplatingContext);
		var logger = LoggingScope.Current?.Logger;
		logger?.AddRewriter(new ProjectDirectoryRewriter(projectTemplatingContext));
		return result;
	}

	/// <summary>
	/// Dispose member
	/// </summary>
	public async ValueTask DisposeAsync()
	{
		await UninstallAsync(CancellationToken.None);
	}

	private bool _disposed;

	/// <summary>
	/// Uninstalls the template
	/// </summary>
	/// <param name="cancellationToken"></param>
	public async Task UninstallAsync(CancellationToken cancellationToken)
	{
		if (_disposed)
			return;

		await LoggedDotnetCli.RunDotnetCommandAsync($"new uninstall \"{Context.ProjectTemplatePath.OriginalPath}\"", cancellationToken);

		_disposed = true;
	}
}