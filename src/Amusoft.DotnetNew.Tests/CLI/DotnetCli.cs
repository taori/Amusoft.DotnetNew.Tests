using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Scopes;

namespace Amusoft.DotnetNew.Tests.CLI;

internal static class DotnetCli
{
	public static async Task<bool> InstallAsync(string path, CancellationToken cancellationToken)
	{
		using (var scope = new LoggingScope(false))
		{
			// 106 == already installed
			var r = await LoggedDotnetCli.RunDotnetCommandAsync($"new install \"{path}\"", cancellationToken, null, [106]);
			scope.ParentScope?.AddResult(new TextResult(r ? $"Install for {path} succeded" : $"Install for {path} failed - {scope.ToFullString(PrintKind.All)}"));
			return r;
		}
	}
	
	public static async Task<bool> UninstallAsync(string path, CancellationToken cancellationToken)
	{
		using (var scope = new LoggingScope(false))
		{
			var r = await LoggedDotnetCli.RunDotnetCommandAsync($"new uninstall \"{path}\"", cancellationToken, null, []);
			scope.ParentScope?.AddResult(new TextResult(r ? $"Uninstall for {path} succeded" : $"Uninstall for {path} failed - {scope.ToFullString(PrintKind.All)}"));
			return r;
		}
	}
}