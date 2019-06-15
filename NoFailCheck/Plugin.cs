﻿using System;
using IPA;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using System.Linq;
using System.IO;
using UnityEngine;
using IPA.Utilities;
using CustomUI.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.UI;
using CustomUI.MenuButton;
using Harmony;
using IPA.Config;

namespace NoFailCheck
{
    public class Plugin : IBeatSaberPlugin
    {
        public static SemVer.Version Version => IPA.Loader.PluginManager.GetPlugin("NoFailCheck").Metadata.Version;

        internal static HarmonyInstance harmony;

        public static IPALogger Log { get; internal set; }

        public static Settings cfg;
        public static string DataPath = Path.Combine(Environment.CurrentDirectory, "UserData");

        public void Init(object thisIsNull, IPALogger log)
        {
            Log = log;
        }

        public void OnApplicationStart()
        {
            harmony = HarmonyInstance.Create("com.nate1280.BeatSaber.NoFailCheck");
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

            // load settings
            cfg = Settings.Load();

            BSEvents.OnLoad();
            BSEvents.menuSceneLoadedFresh += OnMenuSceneLoadedFresh;
        }

        private void OnMenuSceneLoadedFresh()
        {
            NoFailCheck.Instance.OnLoad();
        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene) { }

        public void OnSceneLoaded(Scene scene, LoadSceneMode arg1) { }

        public void OnSceneUnloaded(Scene scene) { }

        public void OnApplicationQuit()
        {
            harmony.UnpatchAll("com.nate1280.BeatSaber.NoFailCheck");
        }

        public void OnLevelWasLoaded(int level) { }

        public void OnLevelWasInitialized(int level) { }

        public void OnUpdate() { }

        public void OnFixedUpdate() { }
    }
}