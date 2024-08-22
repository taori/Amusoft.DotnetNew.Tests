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
using Amusoft.DotnetNew.Tests.Interfaces;
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
		// var runner = new CliWrapRunner();
		return await runner.RunAsync(arguments, cancellationToken, acceptAsSuccess).ConfigureAwait(false);
	}
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

		DiagnosticScope.TryAddContent($"dotnet {arguments}");
		LoggingScope.TryAddInvocation($"dotnet {arguments}");

		var command = Cli.Wrap("dotnet")
			.WithEnvironmentVariables(env)
			.WithArguments(arguments)
			.WithValidation(CommandResultValidation.None);
		
		var bufferedCommandResult = await command
			.ExecuteBufferedAsync(cancellationToken)
			.ConfigureAwait(false);
		
		DiagnosticScope.TryAddJson(new
		{
			Output = bufferedCommandResult.StandardOutput,
			Error = bufferedCommandResult.StandardError,
		});
		LoggingScope.TryAddResult(bufferedCommandResult.ToCommandResult());

		return bufferedCommandResult.IsSuccess || successStatusCodes.Contains(bufferedCommandResult.ExitCode);
	}
}

[ExcludeFromCodeCoverage]
internal class LocalProcessRunner : IProcessRunner
{
	public async Task<bool> RunAsync(string arguments, CancellationToken cancellationToken, int[] successStatusCodes)
	{
		DiagnosticScope.TryAddContent($"dotnet {arguments}");
		LoggingScope.TryAddInvocation($"dotnet {arguments}");
		
		var psi = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
			? new ProcessStartInfo("dotnet", arguments)
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				LoadUserProfile = false,
			}
			: new ProcessStartInfo("dotnet", arguments)
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
			};

		psi.Environment.Add("DOTNET_CLI_UI_LANGUAGE", "en");

		var output = new StringBuilder();
		var error = new StringBuilder();
		var process = new Process();
		process.StartInfo = psi;
		process.OutputDataReceived += (sender, args) => output.AppendLine(args.Data);
		process.ErrorDataReceived += (sender, args) => error.AppendLine(args.Data);
		process.Start();
		process.BeginOutputReadLine();
		process.BeginErrorReadLine();

		var sw = new Stopwatch();
		sw.Restart();
		try
		{
			await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
			
			sw.Stop();
		
			var success = process.HasExited && (process.ExitCode == 0 || successStatusCodes.Contains(process.ExitCode));
			DiagnosticScope.TryAddJson(new
			{
				Output = output.ToString(),
				Error = error.ToString(),
			});
			LoggingScope.TryAddResult(new CommandResult(process.ExitCode, output.ToString(), error.ToString(), success, sw.Elapsed));

			return success;
		}
		catch (OperationCanceledException)
		{
			DiagnosticScope.TryAddContent("Operation cancelled");
			LoggingScope.TryAddResult(new TextResult("Process aborted"));
			return false;
		}
	}
}