using System;
using BepInEx.Configuration;
using HarmonyLib;

namespace NoCompanyPenalties.Patches
{
	[HarmonyPatch(typeof(HUDManager))]
	internal class ApplyPenaltyPatch
	{
		[HarmonyPrefix]
		[HarmonyPriority(Priority.First)]
		[HarmonyPatch(typeof(HUDManager), "ApplyPenalty")]
		internal static bool ApplyPenaltyPrefix(HUDManager __instance, ref int playersDead, ref int bodiesInsured)
		{
			try
			{
				Plugin.logger.LogDebug($"Money before penalty: {MrovLib.ContentManager.Terminal.groupCredits}");

				SelectableLevel currentLevel = StartOfRound.Instance.currentLevel;
				if (ConfigManager.CompanyMoonConfigs.TryGetValue(currentLevel, out ConfigEntry<bool> configEntry) && configEntry.Value)
				{
					// skip penalty application
					Plugin.logger.LogWarning(
						$"Penalties on company moon {MrovLib.SharedMethods.GetAlphanumericName(currentLevel)} are enabled!"
					);
					return true;
				}

				if (MrovLib.LevelHelper.CompanyMoons.Contains(currentLevel))
				{
					Plugin.logger.LogDebug("Company moon detected!");
					playersDead = 0;
					bodiesInsured = 0;
				}
				else
				{
					Plugin.logger.LogDebug("Not a company moon");
				}
			}
			catch (Exception ex)
			{
				Plugin.logger.LogError($"ApplyPenalty failed: {ex}");
			}

			return true;
		}

		[HarmonyPostfix]
		[HarmonyPriority(Priority.First)]
		[HarmonyPatch(typeof(HUDManager), "ApplyPenalty")]
		internal static void ApplyPenaltyPostfix(HUDManager __instance, ref int playersDead, ref int bodiesInsured)
		{
			try
			{
				Plugin.logger.LogDebug($"Money after penalty: {MrovLib.ContentManager.Terminal.groupCredits}");

				SelectableLevel currentLevel = StartOfRound.Instance.currentLevel;
				if (MrovLib.LevelHelper.CompanyMoons.Contains(currentLevel))
				{
					Plugin.logger.LogDebug("Company moon detected!");

					Plugin.logger.LogDebug("playersDead: " + playersDead);
					Plugin.logger.LogDebug("bodiesInsured: " + bodiesInsured);
				}
			}
			catch (Exception ex)
			{
				Plugin.logger.LogError($"ApplyPenalty failed: {ex}");
			}
		}
	}
}
