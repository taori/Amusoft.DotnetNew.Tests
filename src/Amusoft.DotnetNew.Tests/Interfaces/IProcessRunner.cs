using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;

namespace Amusoft.DotnetNew.Tests.Interfaces;

internal interface IProcessRunner
{
	Task<bool> RunAsync(string arguments, DotnetCommandOptions? commandOptions, CancellationToken cancellationToken, int[] successStatusCodes);
}