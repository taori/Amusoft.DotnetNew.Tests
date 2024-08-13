using System;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Rewriters;

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
		await LoggedDotnetCli.RunDotnetCommandAsync(projectTemplatingContext, $"new install \"{projectTemplatingContext.ProjectTemplatePath.OriginalPath}\"", cancellationToken);
		var result = new TemplateInstallation(projectTemplatingContext);
		projectTemplatingContext.SolutionContext.CommandLogger.AddRewriter(new ProjectDirectoryRewriter(projectTemplatingContext));
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

		await LoggedDotnetCli.RunDotnetCommandAsync(Context, $"new uninstall \"{Context.ProjectTemplatePath.OriginalPath}\"", cancellationToken);

		_disposed = true;
	}
}