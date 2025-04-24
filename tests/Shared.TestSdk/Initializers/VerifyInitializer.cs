using System.Reflection;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Exceptions;
using DiffEngine;

namespace Shared.TestSdk.Initializers;

public class VerifyInitializer
{
	public static void Initialize()
	{
		DiffTools.UseOrder(DiffTool.TortoiseGitMerge, DiffTool.VisualStudioCode, DiffTool.WinMerge);
		Verifier.DerivePathInfo(PathInfoConfiguration);
		VerifierSettings.ScrubMember<Exception>(nameof(Exception.StackTrace));
		VerifierSettings.ScrubMember<CommandResult>(nameof(CommandResult.Runtime));
	}

	private static PathInfo PathInfoConfiguration(string sourcefile, string projectdirectory, Type type, MethodInfo method)
	{
		return new PathInfo(
			directory: Path.Combine(projectdirectory, "Snapshots"),
			typeName: type.Name,
			methodName: method.Name
		);
	}
}