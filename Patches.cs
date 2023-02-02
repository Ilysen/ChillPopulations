using HarmonyLib;
using HistoryKit;
using Qud.API;
using System;
using System.Reflection;
using XRL.World;
using XRL.World.Parts;
using XRL.World.ZoneBuilders;

namespace ChillPopulations.HarmonyPatches
{
    // warning: here be dragons
    // many of these methods have inline logic that's pretty hardcoded and there's no clean way to just postfix onto an existing conditional except for thralls
    // we do the best we can, but it's pretty bad

    /// <summary>
    /// Prevent creatures with the "NoThrall" tag from appearing as psychic thralls.
    /// Not needed for creatures that have a mental shield, like non-sapient oozes, fungi, etc.
    /// </summary>
    [HarmonyPatch(typeof(HasThralls), nameof(HasThralls.IsSuitableThrall))]
    class ThrallPatch
    {
        static void Postfix(GameObjectBlueprint BP, ref bool __result)
        {
            if (__result && BP.HasTagOrProperty("NoThrall"))
                __result = false;
        }
    }

    /// <summary>
    /// Prevent creatures with the "NoPariah" tag from appearing as pariahs.
    /// </summary>
    [HarmonyPatch(typeof(PariahSpawner), nameof(PariahSpawner.GeneratePariah), typeof(int), typeof(bool), typeof(bool))]
    class PariahPatch
    {
        // We override the base method here, which is quite destructive
        // It's probably very possible to do a transpiler instead and make the original function calls use a predefined filter,
        // but my head is empty and I'm not quite at that level
        static bool Prefix(int level, bool AlterName, bool IsUnique, ref GameObject __result)
        {
            Predicate<GameObjectBlueprint> filter = c => !c.HasTagOrProperty("NoPariah");
            GameObject filteredCreature = (level == -1 ? EncountersAPI.GetACreature(filter) : EncountersAPI.GetCreatureAroundLevel(level, filter));
            __result = PariahSpawner.GeneratePariah(filteredCreature, AlterName, IsUnique);
            return false;
        }
    }

    /// <summary>
    /// Prevent creatures with the "NoCultist" tag from appearing as cultists in historical sites.
    /// </summary>
    [HarmonyPatch]
    class HistoricSiteCultistPatch
    {
        static MethodBase TargetMethod() => AccessTools.DeclaredMethod(typeof(SultanDungeon), "ProcessBlueprintForCultRole");
        static bool Prefix(GameObjectBlueprint B) => !B.HasTagOrProperty("NoCultist");
    }

    /// <summary>
    /// Prevent creatures with the "NoCultist" tag from appearing as cultists in the Tomb of the Eaters.
    /// </summary>
    [HarmonyPatch]
    class TombCultistPatch
    {
        static MethodBase TargetMethod() => AccessTools.DeclaredMethod(typeof(CultistSpawner), "GetLikedFactionMember");

        static void Prefix(HistoricEntitySnapshot SultanSnapshot, ref Predicate<GameObjectBlueprint> filter)
        {
            Predicate<GameObjectBlueprint> f = filter;
            filter = s => !s.HasTagOrProperty("NoCultist") && f.Invoke(s);
        }
    }
}
