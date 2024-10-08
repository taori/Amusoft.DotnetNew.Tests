﻿using Amusoft.Toolkit.Threading;

namespace Amusoft.DotnetNew.Tests.Scopes;

internal class ProjectScope : AmbientScope<ProjectScope>
{
	public string Path { get; }

	public ProjectScope(string path)
	{
		Path = path;
	}
}