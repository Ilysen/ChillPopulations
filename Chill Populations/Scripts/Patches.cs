using HarmonyLib;
using Qud.API;
using System;
using XRL.World;
using XRL.World.Parts;
using XRL.World.ZoneBuilders;

namespace Ava.ChillPopulations.HarmonyPatches
{
	// warning: here be dragons
	// many of these methods have inline logic that's pretty hardcoded and there's no clean way to just postfix onto an existing conditional except for thralls
	// we do the best we can, but it's pretty bad

	[HarmonyPatch(typeof(HasThralls), nameof(HasThralls.IsSuitableThrall))]
	class ThrallPatch
	{
		/// <summary>
		/// Prevent creatures with the "NoThrall" tag from appearing as psychic thralls.
		/// Creatures that have a mental shield, like non-sapient oozes, fungi, etc, are skipped already.
		/// </summary>
		[HarmonyPostfix]
		static void IsSuitableThrallPatch(GameObjectBlueprint BP, ref bool __result)
		{
			if (__result && BP.HasTagOrProperty("NoThrall"))
				__result = false;
		}
	}

	[HarmonyPatch(typeof(PariahSpawner), nameof(PariahSpawner.GeneratePariah), typeof(int), typeof(bool), typeof(bool))]
	class PariahPatch
	{
		/// <summary>
		/// Prevent creatures with the "NoPariah" tag from appearing as pariahs.
		/// We override the base method here, which is quite destructive.
		/// While it's probably possible to transpile it to include a new filter in place of the old,
		/// transpilers in general have good potential to break things.
		/// </summary>
		[HarmonyPrefix]
		static bool GeneratePariahOverride(int level, bool AlterName, bool IsUnique, ref GameObject __result)
		{
			Predicate<GameObjectBlueprint> filter = c => !c.HasTagOrProperty("NoPariah");
			GameObject filteredCreature = (level == -1 ? EncountersAPI.GetACreature(filter) : EncountersAPI.GetCreatureAroundLevel(level, filter));
			__result = PariahSpawner.GeneratePariah(filteredCreature, AlterName, IsUnique);
			return false;
		}
	}

	[HarmonyPatch(typeof(SultanDungeon), "ProcessBlueprintForCultRole")]
	class HistoricSiteCultistPatch
	{
		/// <summary>
		/// Prevent creatures with the "NoCultist" tag from appearing as cultists in historical sites.
		/// </summary>
		[HarmonyPrefix]
		static bool ProcessBlueprintForCultRolePatch(GameObjectBlueprint B)
		{
			return !B.HasTagOrProperty("NoCultist");
		}
	}

	[HarmonyPatch(typeof(CultistSpawner), "GetLikedFactionMember")]
	class TombCultistPatch
	{
		/// <summary>
		/// Prevent creatures with the "NoCultist" tag from appearing as cultists in the Tomb of the Eaters.
		/// </summary>
		[HarmonyPrefix]
		static void GetLikedFactionMemberPrefix(ref Predicate<GameObjectBlueprint> filter)
		{
			Predicate<GameObjectBlueprint> f = filter;
			filter = s => !s.HasTagOrProperty("NoCultist") && f(s);
		}
	}
}
