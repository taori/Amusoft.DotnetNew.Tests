using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Exceptions;
using Amusoft.DotnetNew.Tests.Scaffolding;
using Amusoft.DotnetNew.Tests.Templating;

namespace Amusoft.DotnetNew.Tests.CLI;

/// <summary>
/// Dotnet new CLI tool
/// </summary>
public static class DotnetNew
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="arguments"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ScaffoldingFailedException"></exception>
	public static async Task<Scaffold> New(string arguments, CancellationToken cancellationToken)
	{
		var result = await LoggedDotnetCli.RunDotnetCommandAsync(new CommandLogger(), arguments, cancellationToken);
		if (!result)
			throw new ScaffoldingFailedException($"Scaffolding with arguments {arguments} failed.");

		return new Scaffold();
	}
}