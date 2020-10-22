using System;
using HarmonyLib;

namespace NoFailCheck.HarmonyPatches
{
    [HarmonyPatch(typeof(StandardLevelDetailViewController))]
    [HarmonyPatch("SetData", MethodType.Normal)]
    [HarmonyPatch(new Type[]
    {
        typeof(IBeatmapLevelPack),
        typeof(IPreviewBeatmapLevel),
        typeof(bool),
        typeof(bool),
        typeof(bool),
        typeof(bool),
        typeof(string),
        typeof(BeatmapDifficultyMask),
        typeof(BeatmapCharacteristicSO[])
    })]
    class StandardLevelDetailViewControllerPatches
    {
        static bool Prefix(IBeatmapLevelPack pack, IPreviewBeatmapLevel previewBeatmapLevel, bool showPlayerStats, bool hidePracticeButton, bool hide360DegreeBeatmapCharacteristic, bool canBuyPack, ref string playButtonText, BeatmapDifficultyMask allowedBeatmapDifficultyMask, BeatmapCharacteristicSO[] notAllowedCharacteristics)
        {
            if (Plugin.cfg.Enabled && NoFailCheck.IsInSoloFreeplay)
            {
                if (NoFailCheck.NoFailEnabled && Plugin.cfg.ChangeText)
                {
                    playButtonText = "No Fail!";
                }
            }
            return true;
        }
    }
}
