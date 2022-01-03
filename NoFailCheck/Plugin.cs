using System;
using IPA;
using Logger = IPA.Logging.Logger;
using System.IO;
using HarmonyLib;
using BS_Utils.Utilities;
using BeatSaberMarkupLanguage.Settings;
using NoFailCheck.UI;

namespace NoFailCheck
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        public static SemVer.Version Version => IPA.Loader.PluginManager.GetPlugin("NoFailCheck").Version;

        internal static Harmony harmony;

        public static Logger Log { get; internal set; }

        public static Settings cfg;
        public static string DataPath = Path.Combine(Environment.CurrentDirectory, "UserData");

        [Init]
        public void Init(Logger log)
        {
            Log = log;
        }

        [OnStart]
        public void OnStart()
        {
            // create userdata path if needed
            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }

            harmony = new Harmony("com.nate1280.BeatSaber.NoFailCheck");
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

            // load settings
            cfg = Settings.Load();

            BSEvents.OnLoad();
            BSEvents.lateMenuSceneLoadedFresh += lateMenuSceneLoadedFresh;
        }

        private void lateMenuSceneLoadedFresh(ScenesTransitionSetupDataSO obj)
        {
            // add BSML mod settings
            BSMLSettings.instance.AddSettingsMenu("NoFail Check", "NoFailCheck.Views.NoFailCheckSettings.bsml", NoFailCheckSettings.instance);

            // load main mod
            NoFailCheck.Instance.OnLoad();
        }

        [OnExit]
        public void OnExit()
        {
            harmony.UnpatchSelf();
        }
    }
}
