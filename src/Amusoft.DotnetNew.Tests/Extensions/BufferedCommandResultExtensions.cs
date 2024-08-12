using CliWrap.Buffered;

namespace Amusoft.DotnetNew.Tests.Extensions;

internal static class BufferedCommandResultExtensions
{
	internal static CommandResult ToCommandResult(this BufferedCommandResult source)
	{
		return new CommandResult(source.ExitCode, source.StandardOutput, source.StandardError, source.IsSuccess, source.RunTime);
	}
}