using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Extensions;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.DotnetNew.Tests.Templating;
using CliWrap;
using CliWrap.Buffered;
using CliWrap.Builders;

namespace Amusoft.DotnetNew.Tests.CLI;

internal static class LoggedDotnetCli
{
	internal static Task<bool> RunDotnetCommandAsync(Action<ArgumentsBuilder> arguments, CancellationToken cancellationToken, int[] acceptAsSuccess, Action<Command>? configure = default)
	{
		var builder = new ArgumentsBuilder();
		arguments(builder);
		return RunDotnetCommandAsync(builder.Build(), cancellationToken, acceptAsSuccess, configure);
	}
	
	internal static async Task<bool> RunDotnetCommandAsync(string arguments, CancellationToken cancellationToken, int[] acceptAsSuccess, Action<Command>? configure = default)
	{
		var env = new Dictionary<string, string?>()
		{
			["DOTNET_CLI_UI_LANGUAGE"] = "en",
		};

		LoggingScope.TryAddInvocation($"dotnet {arguments}");

		var command = Cli.Wrap("dotnet")
			.WithEnvironmentVariables(env)
			.WithArguments(arguments)
			.WithValidation(CommandResultValidation.None);
		configure?.Invoke(command);
		
		var bufferedCommandResult = await command
			.ExecuteBufferedAsync(cancellationToken)
			.ConfigureAwait(false);
		
		LoggingScope.TryAddResult(bufferedCommandResult.ToCommandResult());

		return bufferedCommandResult.IsSuccess || acceptAsSuccess.Contains(bufferedCommandResult.ExitCode);
	}
	
}