using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Extensions;
using Amusoft.DotnetNew.Tests.Scopes;
using CliWrap;
using CliWrap.Buffered;
using CommandResult = Amusoft.DotnetNew.Tests.Diagnostics.CommandResult;

namespace Amusoft.DotnetNew.Tests.CLI;

internal static class LoggedDotnetCli
{
	internal static async Task<bool> RunDotnetCommandAsync(string arguments, CancellationToken cancellationToken, int[] acceptAsSuccess)
	{
		var runner = new LocalProcessRunner();
		return await runner.RunAsync(arguments, cancellationToken, acceptAsSuccess).ConfigureAwait(false);
	}
}

internal interface IProcessRunner
{
	Task<bool> RunAsync(string arguments, CancellationToken cancellationToken, int[] successStatusCodes);
}

[ExcludeFromCodeCoverage]
internal class CliWrapRunner : IProcessRunner
{
	public async Task<bool> RunAsync(string arguments, CancellationToken cancellationToken, int[] successStatusCodes)
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
		
		var bufferedCommandResult = await command
			.ExecuteBufferedAsync(cancellationToken)
			.ConfigureAwait(false);
		
		LoggingScope.TryAddResult(bufferedCommandResult.ToCommandResult());

		return bufferedCommandResult.IsSuccess || successStatusCodes.Contains(bufferedCommandResult.ExitCode);
	}
}

[ExcludeFromCodeCoverage]
internal class LocalProcessRunner : IProcessRunner
{
	public async Task<bool> RunAsync(string arguments, CancellationToken cancellationToken, int[] successStatusCodes)
	{
		LoggingScope.TryAddInvocation($"dotnet {arguments}");
		
		var psi = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
			? new ProcessStartInfo("dotnet", arguments)
			{
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				LoadUserProfile = false,
			}
			: new ProcessStartInfo("dotnet", arguments)
			{
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true,
			};

		psi.Environment.Add("DOTNET_CLI_UI_LANGUAGE", "en");

		var sb = new StringBuilder();
		var process = new Process();
		process.StartInfo = psi;
		process.OutputDataReceived += (sender, args) => sb.Append(args.Data);
		process.Start();
		process.BeginOutputReadLine();

		var sw = new Stopwatch();
		sw.Restart();
		try
		{
			await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
			
			sw.Stop();
		
			var success = process.HasExited && (process.ExitCode == 0 || successStatusCodes.Contains(process.ExitCode));
			LoggingScope.TryAddResult(new CommandResult(process.ExitCode, sb.ToString(), string.Empty, success, sw.Elapsed));

			return success;
		}
		catch (OperationCanceledException)
		{
			LoggingScope.TryAddResult(new TextResult("Process aborted"));
			return false;
		}
	}
}