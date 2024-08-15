using System.Linq;
using System.Text;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Scopes;
using Shouldly;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class LoggingScopeTests
{
	[Fact]
	void InnerForwardsToOuter()
	{
		var invocation = new TestInvocation("");
		
		using var outer = new LoggingScope(true);
		using var inner = new LoggingScope(true);

		inner.Logger.AsReceiver().AddInvocation(invocation);
		outer.Logger.CommandData.Contains(invocation).ShouldBeTrue();
	}
	
	internal class TestInvocation : ICommandInvocation
	{
		public TestInvocation(string content)
		{
			Content = content;
		}

		public string Content { get; }
		
		public void Print(StringBuilder stringBuilder)
		{
			stringBuilder.Append(Content);
		}
	}
}