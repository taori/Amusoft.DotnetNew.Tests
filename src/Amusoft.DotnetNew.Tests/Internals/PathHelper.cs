
using System;
using System.IO;

namespace Amusoft.DotnetNew.Tests.Internals;

internal static class PathHelper
{
	public static string AbsoluteTrimPathEnd(string input, int count)
	{
		for (int i = 0; i < count; i++)
		{
			input = Path.GetDirectoryName(input) ?? throw new Exception($"Directory for {input} could not be found");
		}

		return input;
	}
}