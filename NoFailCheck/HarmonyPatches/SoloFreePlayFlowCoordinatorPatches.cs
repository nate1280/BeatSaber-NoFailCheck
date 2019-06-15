using Harmony;

namespace NoFailCheck.HarmonyPatches
{
    [HarmonyPatch(typeof(SoloFreePlayFlowCoordinator))]
    [HarmonyPatch("HandleLevelDetailViewControllerDidPressPlayButton", MethodType.Normal)]
    class SoloFreePlayFlowCoordinatorPatches
    {
        static bool Prefix(StandardLevelDetailViewController viewController)
        {
            if (Plugin.cfg.Enabled)
            {
                if (NoFailCheck.NoFailEnabled)
                {
                    if (Plugin.cfg.DoublePress && NoFailCheck.NoFailPressCount == 0)
                    {
                        NoFailCheck.NoFailPressCount++;
                        return false;
                    }
                    else if (Plugin.cfg.DoublePress && NoFailCheck.NoFailPressCount == 1)
                    {
                        NoFailCheck.NoFailPressCount = 0;
                    }
                }
            }
            return true;
        }
    }
}
