using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Amusoft.DotnetNew.Tests.Diagnostics;
using VerifyTests;
using VerifyXunit;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Amusoft.DotnetNew.Tests.UnitTests.Toolkit
{
	public class ProjectSetup
	{
		[ModuleInitializer]
		public static void Initialize()
		{
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
}