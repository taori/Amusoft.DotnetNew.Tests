using System;
using System.Collections.Generic;
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
	internal static Task<bool> RunDotnetCommandAsync(Action<ArgumentsBuilder> arguments, CancellationToken cancellationToken)
	{
		var builder = new ArgumentsBuilder();
		arguments(builder);
		return RunDotnetCommandAsync(builder.Build(), cancellationToken);
	}
	
	internal static async Task<bool> RunDotnetCommandAsync(string arguments, CancellationToken cancellationToken)
	{
		var env = new Dictionary<string, string?>()
		{
			["DOTNET_CLI_UI_LANGUAGE"] = "en",
		};

		var logger = LoggingScope.Current?.Logger;
		logger?.AddInvocation($"dotnet {arguments}");
		
		var bufferedCommandResult = await Cli.Wrap("dotnet")
			.WithEnvironmentVariables(env)
			.WithArguments(arguments)
			.WithValidation(CommandResultValidation.None)
			.ExecuteBufferedAsync(cancellationToken)
			.ConfigureAwait(false);
		
		logger?.AddResult(bufferedCommandResult.ToCommandResult());

		return bufferedCommandResult.IsSuccess;
	}
	
}