using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Utility;
using VerifyXunit;
using Xunit;

namespace Amusoft.DotnetNew.Tests.UnitTests.Tests;

public class NewLineEncoderTests
{
	[Fact]
	public async Task VerifyMaxOutputCharactersPerInputCharacter()
	{
		var enconder = new CustomJsonEncoder();
		await Verifier.Verify(enconder.MaxOutputCharactersPerInputCharacter);
	} 
}