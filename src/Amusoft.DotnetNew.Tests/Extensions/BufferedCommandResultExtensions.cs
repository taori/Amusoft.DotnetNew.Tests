using System.Diagnostics.CodeAnalysis;
using Amusoft.DotnetNew.Tests.Diagnostics;
using CliWrap.Buffered;

namespace Amusoft.DotnetNew.Tests.Extensions;

[ExcludeFromCodeCoverage]
internal static class BufferedCommandResultExtensions
{
	internal static CommandResult ToCommandResult(this BufferedCommandResult source)
	{
		return new CommandResult(source.ExitCode, source.StandardOutput, source.StandardError, source.IsSuccess, source.RunTime);
	}
}