using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Amusoft.DotnetNew.Tests.CLI;

namespace Amusoft.DotnetNew.Tests.Extensions;

internal static class VerbosityExtensions
{
	private static readonly Dictionary<Verbosity, string> Values;

	static VerbosityExtensions()
	{
		Values = GetValues();
	}

	private static Dictionary<Verbosity,string> GetValues()
	{
		var values = new Dictionary<Verbosity, string>();
		foreach (var verbosity in Enum.GetValues<Verbosity>())
		{
			var description = typeof(Verbosity).GetMember(verbosity.ToString())[0].GetCustomAttribute<DescriptionAttribute>() 
			                  ?? throw new Exception($"{verbosity} is missing a description value");
			
			values.Add(verbosity, description.Description);
		}

		return values;
	}

	public static string ToVerbosityText(this Verbosity source)
	{
		return Values[source];
	}
}