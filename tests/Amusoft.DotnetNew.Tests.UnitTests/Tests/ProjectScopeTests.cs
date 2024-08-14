using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.DotnetNew.Tests.Templating;
using Shouldly;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class ProjectScopeTests
{
	[Theory]
	[InlineData("a", "b")]
	[InlineData("c", "b")]
	private void NonThreaded(string v1, string v2)
	{
		ProjectScope.Current.ShouldBeNull();
		using(var outerScope = new ProjectScope(v1))
		{
			ProjectScope.Current.Path.ShouldBe(v1);
			
			using(var innerScope = new ProjectScope(v2))
			{
				ProjectScope.Current.Path.ShouldBe(v2);
				ProjectScope.Current.ParentScope!.Path.ShouldBe(v1);
			}
			
			ProjectScope.Current.Path.ShouldBe(v1);
		}
		
		ProjectScope.Current.ShouldBeNull();
	}
	
	[Theory]
	[InlineData("a", "b")]
	[InlineData("c", "b")]
	private async Task ThreadedBehavior(string v1, string v2)
	{
		ProjectScope.Current.ShouldBeNull();
		using(var outerScope = new ProjectScope(v1))
		{
			ProjectScope.Current!.Path.ShouldBe(v1);
			
			using(var innerScope = new ProjectScope(v2))
			{
				await Task.Run(() =>
					{
						ProjectScope.Current.Path.ShouldBe(v2);
						ProjectScope.Current.ParentScope!.Path.ShouldBe(v1);
					}
				);
				
				innerScope.Path.ShouldBe(v2);
				outerScope.Path.ShouldBe(v1);
				
				innerScope.ParentScope.ShouldBe(outerScope);
			}
			
			ProjectScope.Current.Path.ShouldBe(v1);
		}
		
		ProjectScope.Current.ShouldBeNull();
	}
	
	[Theory(Timeout = 10000)]
	[InlineData(10, 10)]
	[InlineData(40, 10)]
	[InlineData(10, 40)]
	private async Task ParallelBehavior(int parallelism, int charCount)
	{
		var testSet = Enumerable
			.Range(97, charCount)
			.Select(n => ((char)n).ToString())
			.ToArray();

		var expected = Enumerable.Range(1, charCount)
			.Select(length => Enumerable.Range(97, length))
			.Reverse()
			.Select(d => new string(d.Select(c => ((char)c)).ToArray()))
			.ToArray();
		
		var tasks = new List<Task>();
		for (int i = 0; i < parallelism; i++)
		{
			tasks.Add(Task.Run(async() =>
			{
				await ProcessTestSet(testSet, 1, charCount + 1, expected);
			}));
		}

		await Task.WhenAll(tasks);

		async Task ProcessTestSet(string[] data, int take, int limit, string[] expected)
		{
			var currentJoin = string.Join("", data.Take(take));
			using (var scope = new ProjectScope(currentJoin))
			{
				// await Task.Delay(10);
				var scopePaths = ProjectScope.Current?.ParentScopes.Select(d => d.Path).ToArray();

				if (take < limit)
					await ProcessTestSet(data, take + 1, limit, expected);
				else
					scopePaths.ShouldBe(expected);
			}
		}
	}
}