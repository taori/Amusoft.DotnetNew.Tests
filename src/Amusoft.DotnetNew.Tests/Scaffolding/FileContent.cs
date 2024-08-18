namespace Amusoft.DotnetNew.Tests.Scaffolding;

/// <summary>
/// 
/// </summary>
/// <param name="RelativePath"></param>
/// <param name="Content"></param>
public record class FileContent(
	string RelativePath,
	string Content
);