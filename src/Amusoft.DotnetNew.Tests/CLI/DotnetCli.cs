using System.Threading;
using System.Threading.Tasks;

namespace Amusoft.DotnetNew.Tests.CLI;

internal static class DotnetCli
{
	public static Task<bool> InstallAsync(string path, CancellationToken cancellationToken)
	{
		return LoggedDotnetCli.RunDotnetCommandAsync($"new install \"{path}\"", cancellationToken);
	}
	public static Task<bool> UninstallAsync(string path, CancellationToken cancellationToken)
	{
		return LoggedDotnetCli.RunDotnetCommandAsync($"new uninstall \"{path}\"", cancellationToken);
	}
}