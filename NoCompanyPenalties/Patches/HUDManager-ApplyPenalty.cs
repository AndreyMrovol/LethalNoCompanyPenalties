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
		internal static void ApplyPenalty_postfix(HUDManager __instance, ref int playersDead, ref int bodiesInsured)
		{
			try
			{
				SelectableLevel currentLevel = StartOfRound.Instance.currentLevel;
				if (MrovLib.LevelHelper.CompanyMoons.Contains(currentLevel))
				{
					Plugin.logger.LogDebug("Company moon detected!");
					playersDead = 0;
				}
			}
			catch (Exception ex)
			{
				Plugin.logger.LogError($"ApplyPenalty failed: {ex}");
			}
		}
	}
}
