using System;
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
				Plugin.logger.LogInfo($"Money before penalty: {MrovLib.ContentManager.Terminal.groupCredits}");

				SelectableLevel currentLevel = StartOfRound.Instance.currentLevel;
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
				Plugin.logger.LogInfo($"Money after penalty: {MrovLib.ContentManager.Terminal.groupCredits}");

				SelectableLevel currentLevel = StartOfRound.Instance.currentLevel;
				if (MrovLib.LevelHelper.CompanyMoons.Contains(currentLevel))
				{
					Plugin.logger.LogMessage("Company moon detected!");

					Plugin.logger.LogMessage("playersDead: " + playersDead);
					Plugin.logger.LogMessage("bodiesInsured: " + bodiesInsured);
				}
			}
			catch (Exception ex)
			{
				Plugin.logger.LogError($"ApplyPenalty failed: {ex}");
			}
		}
	}
}
