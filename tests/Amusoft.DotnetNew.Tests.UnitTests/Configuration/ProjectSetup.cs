using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Templating;
using Amusoft.DotnetNew.Tests.UnitTests.Helpers;
using VerifyTests;
using VerifyXunit;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Amusoft.DotnetNew.Tests.UnitTests.Configuration
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