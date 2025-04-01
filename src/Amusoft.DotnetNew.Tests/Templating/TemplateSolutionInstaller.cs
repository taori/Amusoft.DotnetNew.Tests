using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Internals;
using Amusoft.DotnetNew.Tests.Rewriters;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.DotnetNew.Tests.Utility;

namespace Amusoft.DotnetNew.Tests.Templating;

/// <summary>
/// Template solution
/// </summary>
public class TemplateSolution
{
	/// <summary>
	/// 
	/// </summary>
	public PathSource Solution { get; }
	
	/// <summary>
	/// Constructor that uses the path to the solution file
	/// </summary>
	/// <param name="solutionPath"></param>
	public TemplateSolution(string solutionPath)
	{
		if (!solutionPath.EndsWith(".sln"))
			throw new ArgumentException("Solution files are expected to end with the extension sln");
		if (!File.Exists(solutionPath))
			throw new FileNotFoundException(solutionPath);
		
		Solution = new PathSource(solutionPath);

		LoggingScope.TryAddRewriter(BackslashRewriter.Instance);
		LoggingScope.TryAddRewriter(new SolutionDirectoryRewriter(Solution.Directory));
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="searchDirectoryStart">directory indicating where to start looking for the solution file</param>
	/// <param name="maxParentJumps"></param>
	/// <param name="solutionName">filename of the solution</param>
	public TemplateSolution(string searchDirectoryStart, int maxParentJumps, string solutionName) : this(GetSolutionPathFromAssembly(searchDirectoryStart, maxParentJumps, solutionName))
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
	/// <param name="relativePath">relative path to the template</param>
	/// <param name="cancellationToken"></param>
	public async Task<TemplateInstallation> InstallTemplateAsync(string relativePath, CancellationToken cancellationToken)
	{
		var fullPath = Solution.PathTranslator.GetAbsolutePath(relativePath);
		if (!Directory.Exists(fullPath.OriginalPath))
			throw new DirectoryNotFoundException($"{Solution.Directory.VirtualPath} + {relativePath}");

		return await TemplateInstallation.CreateAsync(new ProjectTemplatingContext(fullPath), cancellationToken).ConfigureAwait(false);
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
			throw new DirectoryNotFoundException($"{Solution.Directory.VirtualPath} + {searchFolder}");

		var templateFiles = Directory.EnumerateFiles(folder.OriginalPath, "template.json", SearchOption.AllDirectories);
		
		return templateFiles
			.Select(path => PathHelper.AbsoluteTrimPathEnd(path, 2))
			.Select(path => Solution.PathTranslator.GetRelativePath(path))
			.OrderBy(d => d.VirtualPath)
			.ToArray();
	}

	/// <summary>
	/// Installs all templates that can be found in the given directory
	/// </summary>
	/// <param name="relativePath"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<TemplateInstallationGroup> InstallTemplatesFromDirectoryAsync(string relativePath, CancellationToken cancellationToken)
	{
		var group = new TemplateInstallationGroup();
		foreach (var projectPath in DiscoverTemplates(relativePath))
		{
			cancellationToken.ThrowIfCancellationRequested();
			group.Add(await InstallTemplateAsync(projectPath.OriginalPath, cancellationToken));
		}

		return group;
	}
}