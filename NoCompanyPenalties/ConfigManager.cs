using System.Collections.Generic;
using BepInEx.Configuration;

namespace NoCompanyPenalties
{
	public class ConfigManager
	{
		public static ConfigManager Instance { get; internal set; }
		public static ConfigFile configFile;

		public static Dictionary<SelectableLevel, ConfigEntry<bool>> CompanyMoonConfigs = [];

		public static void Init(ConfigFile config)
		{
			Instance = new ConfigManager(config);
		}

		public ConfigManager(ConfigFile config)
		{
			configFile = config;
		}
	}
}
