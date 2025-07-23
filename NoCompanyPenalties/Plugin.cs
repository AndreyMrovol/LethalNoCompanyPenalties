using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace NoCompanyPenalties
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		internal static ManualLogSource logger;
		internal static Harmony harmony = new(PluginInfo.PLUGIN_GUID);

		private void Awake()
		{
			logger = Logger;
			harmony.PatchAll();

			ConfigManager.Init(Config);

			MrovLib.EventManager.TerminalStart.AddListener(GenerateConfigs);

			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
		}

		private void GenerateConfigs(Terminal terminal)
		{
			List<SelectableLevel> CompanyMoons = [];
			foreach (SelectableLevel level in MrovLib.LevelHelper.CompanyMoons)
			{
				string moonName = MrovLib.SharedMethods.GetAlphanumericName(level);

				ConfigManager.CompanyMoonConfigs[level] = ConfigManager.configFile.Bind(
					"Company Moons",
					$"{moonName}",
					false,
					$"Should penalties be applied on {moonName}?"
				);
			}
		}
	}
}
