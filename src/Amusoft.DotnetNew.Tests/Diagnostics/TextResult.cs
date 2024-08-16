using System;
using System.Linq;
using System.Text;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Templating;

namespace Amusoft.DotnetNew.Tests.Diagnostics;

internal class TextResult : ICommandResult
{
	internal TextResult(string text)
	{
		Text = text;
	}

	public string Text { get; }
	
	public void Print(StringBuilder stringBuilder)
	{
		stringBuilder.Append(TemplatingDefaults.Instance.PrintPattern("Result", Text));
	}
}