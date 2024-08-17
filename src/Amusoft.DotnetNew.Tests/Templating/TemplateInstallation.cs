using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Rewriters;
using Amusoft.DotnetNew.Tests.Scopes;

namespace Amusoft.DotnetNew.Tests.Templating;

/// <summary>
/// 
/// </summary>
public class TemplateInstallation : IDisposable
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
		await DotnetCli.InstallAsync(projectTemplatingContext.ProjectTemplatePath.OriginalPath, cancellationToken);
		var result = new TemplateInstallation(projectTemplatingContext);
		var dirName = Path.GetFileName(projectTemplatingContext.ProjectTemplatePath.VirtualPath);
		LoggingScope.TryAddRewriter(new FolderNameAliasRewriter(projectTemplatingContext.ProjectTemplatePath, $"ProjectDir:{dirName}"));
		// var logger = LoggingScope.Current?.Logger;
		// logger?.AddRewriter(new ProjectDirectoryRewriter(projectTemplatingContext));
		// logger?.AddRewriter(new ProjectDirectoryRewriter(projectTemplatingContext));
		return result;
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

		await DotnetCli.UninstallAsync(Context.ProjectTemplatePath.OriginalPath, cancellationToken);

		_disposed = true;
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Dispose
	/// </summary>
	public void Dispose()
	{
		UninstallAsync(CancellationToken.None).GetAwaiter().GetResult();
	}
}