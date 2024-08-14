using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Rewriters;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.DotnetNew.Tests.Utility;

namespace Amusoft.DotnetNew.Tests.Templating;

/// <summary>
/// Top level class to install a template solution
/// </summary>
public class TemplateSolutionInstaller
{
	/// <summary>
	/// 
	/// </summary>
	public Solution Solution { get; }
	
	/// <summary>
	/// Constructor that uses the path to the solution file
	/// </summary>
	/// <param name="solutionPath"></param>
	public TemplateSolutionInstaller(string solutionPath)
	{
		if (!solutionPath.EndsWith(".sln"))
			throw new ArgumentException("Solution files are expected to end with the extension sln");
		
		Solution = new Solution(solutionPath);

		var logger = LoggingScope.Current?.Logger;
		logger?.AddRewriter(BackslashRewriter.Instance);
		logger?.AddRewriter(new SolutionDirectoryRewriter(Solution.Directory));
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="searchDirectoryStart">directory indicating where to start looking for the solution file</param>
	/// <param name="maxParentJumps"></param>
	/// <param name="solutionName">filename of the solution</param>
	public TemplateSolutionInstaller(string searchDirectoryStart, int maxParentJumps, string solutionName) : this(GetSolutionPathFromAssembly(searchDirectoryStart, maxParentJumps, solutionName))
	{
	}

	private static string GetSolutionPathFromAssembly(string searchDirectoryStart, int maxParentJumps, string solutionName)
	{
		if (Path.GetExtension(searchDirectoryStart) is { Length: > 0})
			searchDirectoryStart = Path.GetDirectoryName(searchDirectoryStart)!;
		if (!Directory.Exists(searchDirectoryStart))
			throw new DirectoryNotFoundException($"Directory {searchDirectoryStart} not found");

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

		throw new FileNotFoundException($"Solution file {solutionName} not found in {searchDirectoryStart} or it's parent->child folders within search range {maxParentJumps}");
	}

	/// <summary>
	/// Installs a template relative to the solution file
	/// </summary>
	/// <param name="path">relative path to the template</param>
	/// <param name="cancellationToken"></param>
	public async Task<TemplateInstallation> InstallTemplateAsync(string path, CancellationToken cancellationToken)
	{
		var fullPath = Solution.PathTranslator.GetAbsolutePath(path);
		if (!Directory.Exists(fullPath.OriginalPath))
			throw new DirectoryNotFoundException(fullPath.OriginalPath);

		return await TemplateInstallation.CreateAsync(new ProjectTemplatingContext(this, fullPath), cancellationToken).ConfigureAwait(false);
	}
	
	/// <summary>
	/// Prints the commands as specified to the given StringBuilder 
	/// </summary>
	/// <param name="stringBuilder"></param>
	/// <param name="kind"></param>
	public void Print(StringBuilder stringBuilder, PrintKind kind)
	{
		var logger = LoggingScope.Current?.Logger;
		logger?.Print(stringBuilder, kind);
	}

	/// <summary>
	/// Finds the folders with template.json files in them
	/// </summary>
	/// <param name="searchFolder">relative path to the folder you want to run discovery in</param>
	/// <returns></returns>
	/// <exception cref="DirectoryNotFoundException"></exception>
	public CrossPlatformPath[] DiscoverTemplates(string searchFolder)
	{
		var folder = Solution.PathTranslator.GetAbsolutePath(searchFolder);
		if (!Directory.Exists(folder.OriginalPath))
			throw new DirectoryNotFoundException(folder.OriginalPath);

		var templateFiles = Directory.EnumerateFiles(folder.OriginalPath, "template.json", SearchOption.AllDirectories);
		
		return templateFiles
			.Select(path => Path.Combine(path.Split(Path.DirectorySeparatorChar)[..^2]))
			.Select(path => Solution.PathTranslator.GetRelativePath(path))
			.ToArray();
	}

	/// <summary>
	/// Installs all templates that can be found in the given directory
	/// </summary>
	/// <param name="path"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<List<TemplateInstallation>> InstallTemplatesFromDirectoryAsync(string path, CancellationToken cancellationToken)
	{
		var result = new List<TemplateInstallation>();
		foreach (var projectPath in DiscoverTemplates(path))
		{
			cancellationToken.ThrowIfCancellationRequested();
			result.Add(await InstallTemplateAsync(projectPath.OriginalPath, cancellationToken));
		}

		return result;
	}
}