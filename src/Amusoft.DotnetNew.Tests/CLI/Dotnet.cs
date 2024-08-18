using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Exceptions;
using Amusoft.DotnetNew.Tests.Extensions;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Rewriters;
using Amusoft.DotnetNew.Tests.Scaffolding;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.DotnetNew.Tests.Templating;
using Amusoft.DotnetNew.Tests.Utility;

namespace Amusoft.DotnetNew.Tests.CLI;

/// <summary>
/// Dotnet new CLI tool
/// </summary>
[ExcludeFromCodeCoverage]
public class Dotnet : IDotnetCli
{
	/// <summary>
	/// Wrapper for scaffolding and other operations
	/// </summary>
	public static readonly IDotnetCli Cli = new Dotnet();
	
	internal Dotnet()
	{
	}
	
	async Task<Scaffold> IDotnetCli.NewAsync(string template, string? arguments, CancellationToken cancellationToken)
	{
		var tempDirectory = new TempDirectory();
		var scaffold = new Scaffold(tempDirectory, new Dotnet());
		var fullArgs = arguments is not null
			? $"new {template} -o \"{tempDirectory.Path.Directory.OriginalPath}\" {arguments}"
			: $"new {template} -o \"{tempDirectory.Path.Directory.OriginalPath}\"";
		
		LoggingScope.TryAddRewriter(new FolderNameAliasRewriter(tempDirectory.Path.Directory, "Scaffold"));
		var result = await LoggedDotnetCli.RunDotnetCommandAsync(fullArgs, cancellationToken, []);
		var output = LoggingScope.ToFullString();
		if (!result)
			throw new BuildFailedException(fullArgs, output);
		
		return scaffold;
	}

	async Task IDotnetCli.BuildAsync(string fullPath, string? arguments, Verbosity verbosity, CancellationToken cancellationToken, bool restore)
	{
		if (!File.Exists(fullPath) && !Directory.Exists(fullPath))
		{
			if (!File.Exists(fullPath))
				throw new FileNotFoundException(fullPath);
			throw new DirectoryNotFoundException(fullPath);
		}

		var restoreArgument = restore
			? string.Empty
			: "--no-restore";

		using (var loggingScope = new LoggingScope(false))
		{
			var fullArgs = arguments is null
				? $"build {fullPath} {restoreArgument} -v {verbosity.ToVerbosityText()}"
				: $"build {fullPath} {restoreArgument} -v {verbosity.ToVerbosityText()} {arguments}";
			
			if (await LoggedDotnetCli.RunDotnetCommandAsync(fullArgs, cancellationToken, []))
			{
				loggingScope.ParentScope?.AddResult(new TextResult($"success: {fullArgs}"));
			}
			else
			{
				throw new BuildFailedException(fullArgs, loggingScope.ToFullString(PrintKind.All));
			}
		}
	}

	async Task IDotnetCli.RestoreAsync(string fullPath, string? arguments, Verbosity verbosity, CancellationToken cancellationToken)
	{
		if (!File.Exists(fullPath) && !Directory.Exists(fullPath))
		{
			if (!File.Exists(fullPath))
				throw new FileNotFoundException(fullPath);
			throw new DirectoryNotFoundException(fullPath);
		}

		using(var loggingScope = new LoggingScope(false))
		{
			var vText = verbosity.ToVerbosityText();
			var fullArgs = arguments is null
				? $"restore \"{fullPath}\" -v {vText} --ignore-failed-sources"
				: $"restore \"{fullPath}\" -v {vText} --ignore-failed-sources {arguments}";
			if (await LoggedDotnetCli.RunDotnetCommandAsync(fullArgs, cancellationToken, []))
			{
				loggingScope.ParentScope?.AddResult(new TextResult($"success: {fullArgs}"));
			}
			else
			{
				throw new BuildFailedException(fullArgs, loggingScope.ToFullString(PrintKind.All));
			}
		}
	}
}