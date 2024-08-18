using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Interfaces;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.DotnetNew.Tests.UnitTests.Configuration;
using Amusoft.DotnetNew.Tests.UnitTests.Toolkit;
using Shouldly;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class LoggingScopeTests : TestBase
{
	[Fact]
	public void InvocationConnected()
	{
		var invocation = new TestInvocation("inner");
		
		using var outer = new LoggingScope(true);
		using var inner = new LoggingScope(true);

		inner.AddInvocation(invocation);
		outer.ToFullString(PrintKind.All).ShouldContain("inner");
	}
	
	[Fact]
	public void InvocationDisconnected()
	{
		var invocation = new TestInvocation("inner");
		
		using var outer = new LoggingScope(true);
		using var inner = new LoggingScope(false);

		inner.AddInvocation(invocation);
		outer.ToFullString(PrintKind.All).ShouldNotContain("inner");
	}
	
	[Fact]
	public void InvocationConnectedRecursive()
	{
		var invocation = new TestInvocation("inner");
		
		using var scope1 = new LoggingScope(true);
		using var scope2 = new LoggingScope(true);
		using var scope3 = new LoggingScope(true);

		scope3.AddInvocation(invocation);
		scope1.ToFullString(PrintKind.All).ShouldContain("inner");
	}
	
	[Fact]
	public void ResultConnected()
	{
		var item = new TestResult("inner");
		
		using var outer = new LoggingScope(true);
		using var inner = new LoggingScope(true);

		inner.AddResult(item);
		outer.ToFullString(PrintKind.All).ShouldContain("inner");
	}
	
	[Fact]
	public void ResultDisconnected()
	{
		var item = new TestResult("inner");
		
		using var outer = new LoggingScope(true);
		using var inner = new LoggingScope(false);

		inner.AddResult(item);
		outer.ToFullString(PrintKind.All).ShouldNotContain("inner");
	}
	
	[Fact]
	public void ResultConnectedRecursive()
	{
		var item = new TestResult("inner");
		
		using var scope1 = new LoggingScope(true);
		using var scope2 = new LoggingScope(true);
		using var scope3 = new LoggingScope(true);

		scope3.AddResult(item);
		scope1.ToFullString(PrintKind.All).ShouldContain("inner");
	}
	
	[Fact]
	public void RewriterConnected()
	{
		var item = new TestRewriter("inner");
		
		using var outer = new LoggingScope(true);
		using var inner = new LoggingScope(true);

		inner.AddRewriter(item);
		outer.ToFullString(PrintKind.All).ShouldContain("inner");
	}
	
	[Fact]
	public void RewriterDisconnected()
	{
		var item = new TestRewriter("inner");
		
		using var outer = new LoggingScope(true);
		using var inner = new LoggingScope(false);

		inner.AddRewriter(item);
		outer.ToFullString(PrintKind.All).ShouldNotContain("inner");
	}
	
	[Fact]
	public void RewriterConnectedRecursive()
	{
		var item = new TestRewriter("inner");
		
		using var scope1 = new LoggingScope(true);
		using var scope2 = new LoggingScope(true);
		using var scope3 = new LoggingScope(true);

		scope3.AddRewriter(item);
		scope1.ToFullString(PrintKind.All).ShouldContain("inner");
	}

	[Fact]
	public async Task CombinedPrintTest()
	{
		var scope = new LoggingScope();
		scope.AddInvocation(new TestInvocation("invocation"));
		scope.AddResult(new TestResult("result"));
		scope.AddRewriter(new TestRewriter("rewriter"));

		var results = Enum.GetValues<PrintKind>()
			.Select(kind => (kind, new StringBuilder()))
			.ToDictionary(d => d.kind, d => d.Item2);
		
		foreach (var kindWithBuilder in results)
		{
			scope.Print(kindWithBuilder.Value, kindWithBuilder.Key);
		}

		await Verifier.Verify(new
		{
			All = results[PrintKind.All],
			Invocations = results[PrintKind.Invocations],
			Responses = results[PrintKind.Responses],
		});
	}
	

	internal class TestRewriter : ICommandRewriter
	{
		public TestRewriter(string content)
		{
			Content = content;
		}

		public string Content { get; }
		
		public int ExecutionOrder { get; }
		
		public void Rewrite(StringBuilder stringBuilder)
		{
			stringBuilder.Append(Content);
		}
	}
	
	internal class TestResult : ICommandResult
	{
		public TestResult(string content)
		{
			Content = content;
		}

		public string Content { get; }
		
		public void Print(StringBuilder stringBuilder)
		{
			stringBuilder.Append(Content);
		}
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

	public LoggingScopeTests(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
	{
	}
}