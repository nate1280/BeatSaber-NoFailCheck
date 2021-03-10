using BS_Utils.Utilities;
using NoFailCheck.Extensions;
using System;
using System.Linq;
using Polyglot;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NoFailCheck
{
    class NoFailCheck : MonoBehaviour
    {
        public bool initialized = false;

        private static NoFailCheck _instance = null;
        public static NoFailCheck Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = new GameObject("NoFailCheck").AddComponent<NoFailCheck>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        internal static int NoFailPressCount = 0;
        internal static bool NoFailEnabled = false;

        internal static bool IsInSoloFreeplay = false;

        internal static MainMenuViewController MainMenuViewController;
        internal static PromoViewController PromoViewController;
        internal static SoloFreePlayFlowCoordinator SoloFreePlayFlowCoordinator;
        internal static StandardLevelDetailView StandardLevelDetailView;

        private GameplayModifiersPanelController _gameplayModifiersPanelController;
        private GameplayModifierToggle[] _gameplayModifierToggles;

        internal void OnLoad()
        {
            initialized = false;

            // remove potential for duplicate event calls
            BSEvents.levelSelected -= BSEvents_levelSelected;

            // attach to level selected event if we are enabled, and double presses are required
            if (Plugin.cfg.Enabled && Plugin.cfg.DoublePress)
            {
                BSEvents.levelSelected += BSEvents_levelSelected;
            }

            MainMenuViewController = Resources.FindObjectsOfTypeAll<MainMenuViewController>().FirstOrDefault();
            MainMenuViewController.didFinishEvent += MainMenuViewController_didFinishEvent;

            // get PromoViewController
            PromoViewController = Resources.FindObjectsOfTypeAll<PromoViewController>().FirstOrDefault();
            PromoViewController.promoButtonWasPressedEvent += PromoViewController_promoButtonWasPressedEvent;

            SoloFreePlayFlowCoordinator = Resources.FindObjectsOfTypeAll<SoloFreePlayFlowCoordinator>().FirstOrDefault();
            SoloFreePlayFlowCoordinator.didFinishEvent += SoloFreePlayFlowCoordinator_didFinishEvent;

            StandardLevelDetailView = Resources.FindObjectsOfTypeAll<StandardLevelDetailView>().LastOrDefault(v => v.name.IndexOf("Clone", StringComparison.OrdinalIgnoreCase) == -1);

            // setup settings
            SetupUI();
        }

        private void SoloFreePlayFlowCoordinator_didFinishEvent(LevelSelectionFlowCoordinator obj)
        {
            IsInSoloFreeplay = false;
            //Plugin.Log.Info($"SoloFreePlayFlowCoordinator was dismissed");
        }

        private void PromoViewController_promoButtonWasPressedEvent(PromoViewController promoViewController, IAnnotatedBeatmapLevelCollection annotatedBeatmapLevelCollection, IPreviewBeatmapLevel previewBeatmapLevel)
        {
            IsInSoloFreeplay = true;
            //Plugin.Log.Info($"Promo button was pressed, transitioning to SoloFreePlayController");
        }

        private void MainMenuViewController_didFinishEvent(MainMenuViewController vc, MainMenuViewController.MenuButton menuButton)
        {
            IsInSoloFreeplay = menuButton == MainMenuViewController.MenuButton.SoloFreePlay;
            //Plugin.Log.Info($"MainMenuViewController Menu Button Pressed ==> {menuButton.ToString()}");

            if (Plugin.cfg.Enabled)
            {
                if (IsInSoloFreeplay)
                {
                    foreach (var gameplayModifierToggle in _gameplayModifierToggles)
                    {
                        //Plugin.Log.Info($"{gameplayModifierToggle.gameplayModifier.modifierNameLocalizationKey} - {gameplayModifierToggle.gameplayModifier.name} - {Localization.Get(gameplayModifierToggle.gameplayModifier.modifierNameLocalizationKey)}");
                        if (gameplayModifierToggle.gameplayModifier.modifierNameLocalizationKey == "MODIFIER_NO_FAIL_ON_0_ENERGY")
                        {
                            // add listeners to modifier toggle
                            gameplayModifierToggle.toggle.onValueChanged.RemoveListener(new UnityAction<bool>(HandleNoFailToggle));
                            gameplayModifierToggle.toggle.onValueChanged.AddListener(new UnityAction<bool>(HandleNoFailToggle));

                            // set initial state
                            NoFailEnabled = gameplayModifierToggle.toggle.isOn;

                            // exit loop, only 1 no fail toggle
                            break;
                        }
                    }
                }

                // setup default button state
                SetButtonState(NoFailEnabled);
            }
        }

        private void BSEvents_levelSelected(LevelCollectionViewController vc, IPreviewBeatmapLevel level)
        {
            if (NoFailPressCount != 0)
            {
                // reset press count
                NoFailPressCount = 0;
            }            
        }

        private void SetupUI()
        {
            if (initialized) return;

            if (Plugin.cfg.Enabled)
            {
                _gameplayModifiersPanelController = Resources.FindObjectsOfTypeAll<GameplayModifiersPanelController>().First();
                _gameplayModifierToggles = _gameplayModifiersPanelController.GetComponentsInChildren<GameplayModifierToggle>();
            }

            initialized = true;
        }

        private void HandleNoFailToggle(bool value)
        {
            // reset count
            NoFailPressCount = 0;

            // set boolean
            NoFailEnabled = value;

            // set button text
            SetButtonState(value);
        }

        private void SetButtonState(bool state)
        {
            if (!Plugin.cfg.ChangeText || StandardLevelDetailView == null) return;

            if (state && IsInSoloFreeplay)
            {
                StandardLevelDetailView.actionButton.SetButtonText("No Fail!");
            }
            else
            {
                StandardLevelDetailView.actionButton.SetButtonText(Localization.Get("BUTTON_PLAY"));
            }
        }
    }
}
