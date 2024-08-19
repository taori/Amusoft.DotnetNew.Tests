using System.Collections.Generic;
using Amusoft.DotnetNew.Tests.Interfaces;

namespace Amusoft.DotnetNew.Tests.Scopes;

/// <summary>
/// Rewriter API to remove non generic information from messages so it can be tested
/// in a predictable and immutable way
/// </summary>
public interface IRewriteContext
{
	/// <summary>
	/// Rewrites the given input
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	string Rewrite(string input);
}