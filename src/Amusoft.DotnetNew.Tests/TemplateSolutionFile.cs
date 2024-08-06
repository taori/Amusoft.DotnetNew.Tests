using System;
using System.IO;
using System.Linq;

namespace Amusoft.DotnetNew.Tests;

/// <summary>
/// Top level class to install a template solution
/// </summary>
public class TemplateSolutionFile
{
	/// <summary>
	/// Path to the solution
	/// </summary>
	public string SolutionPath { get; }

	/// <summary>
	/// Directory of the solution file
	/// </summary>
	public string SolutionDirectory { get; }

	private readonly RelativePathTranslator _pathTranslator;

	/// <summary>
	/// Constructor that uses the path to the solution file
	/// </summary>
	/// <param name="solutionPath"></param>
	public TemplateSolutionFile(string solutionPath)
	{
		if (!solutionPath.EndsWith(".sln"))
			throw new ArgumentException("Solution files are expected to end with the extension sln");
		SolutionPath = solutionPath;
		SolutionDirectory = Path.GetDirectoryName(solutionPath)!;
		_pathTranslator = new RelativePathTranslator(SolutionDirectory);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="searchDirectoryStart">directory indicating where to start looking for the solution file</param>
	/// <param name="maxParentJumps"></param>
	/// <param name="solutionName">filename of the solution</param>
	public TemplateSolutionFile(string searchDirectoryStart, int maxParentJumps, string solutionName) : this(GetSolutionPathFromAssembly(searchDirectoryStart, maxParentJumps, solutionName))
	{
	}

	private static string GetSolutionPathFromAssembly(string searchDirectoryStart, int maxParentJumps, string solutionName)
	{
		if (Path.GetExtension(searchDirectoryStart) is { Length: > 0})
			searchDirectoryStart = Path.GetDirectoryName(searchDirectoryStart);
		if (!Directory.Exists(searchDirectoryStart))
			throw new Exception($"Directory {searchDirectoryStart} not found");

		var iterations = maxParentJumps;
		var searchPath = searchDirectoryStart;
		while (--iterations >= 0 && searchPath != null)
		{
			var slnFiles = Directory.EnumerateFiles(searchPath, "*.sln", SearchOption.AllDirectories);
			var match = slnFiles.FirstOrDefault(d => Path.GetFileName(d).Equals(solutionName, StringComparison.OrdinalIgnoreCase));
			if (match is not null)
				return match;

			searchPath = new DirectoryInfo(searchPath).Parent?.FullName;
		}

		throw new Exception($"Solution file {solutionName} not found in {searchDirectoryStart} or it's parent->child folders within search range {maxParentJumps}");
	}

	/// <summary>
	/// Returns the path of the file relative to the directory of the solution
	/// </summary>
	/// <param name="relativePath">a path like ./filename.txt or ../a/filename.txt</param>
	/// <returns></returns>
	public CrossPlatformPath GetAbsolutePath(string relativePath) => 
		_pathTranslator.GetAbsolutePath(relativePath);
}