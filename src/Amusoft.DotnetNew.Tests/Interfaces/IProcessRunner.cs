using System.Threading;
using System.Threading.Tasks;

namespace Amusoft.DotnetNew.Tests.Interfaces;

internal interface IProcessRunner
{
	Task<bool> RunAsync(string arguments, CancellationToken cancellationToken, int[] successStatusCodes);
}