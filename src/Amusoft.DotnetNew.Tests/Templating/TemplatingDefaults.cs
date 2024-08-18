using System.Threading;

namespace Amusoft.DotnetNew.Tests.Templating;

/// <summary>
/// Core API for customization
/// </summary>
public static class TemplatingDefaults
{
	/// <summary>
	/// 
	/// </summary>
	public static TemplatingSettings Instance
	{
		get
		{
			if (_instance.Value == null)
				_instance.Value = new TemplatingSettings();
			return _instance.Value;
		}
		set => _instance.Value = value;
	}

	private static readonly AsyncLocal<TemplatingSettings> _instance = new();
}