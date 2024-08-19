using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;

namespace Amusoft.DotnetNew.Tests.Utility;

[ExcludeFromCodeCoverage]
internal class NewLineIgnoreEncoder : JavaScriptEncoder
{
	public static readonly NewLineIgnoreEncoder Instance = new();
	
	public override unsafe int FindFirstCharacterToEncode(char* text, int textLength)
	{
		return Default.FindFirstCharacterToEncode(text, textLength);
	}

	public override unsafe bool TryEncodeUnicodeScalar(int unicodeScalar, char* buffer, int bufferLength, out int numberOfCharactersWritten)
	{
		return Default.TryEncodeUnicodeScalar(unicodeScalar, buffer, bufferLength, out numberOfCharactersWritten);
	}

	private static readonly HashSet<int> Ignores = ['\r', '\n', '\\', '<', '>'];
	
	public override bool WillEncode(int unicodeScalar)
	{
		return !Ignores.Contains(unicodeScalar) && Default.WillEncode(unicodeScalar);
	}

	public override int MaxOutputCharactersPerInputCharacter => Default.MaxOutputCharactersPerInputCharacter;
}