using CustomUI.BeatSaber;
using CustomUI.Settings;
using NoFailCheck.Extensions;
using System.Linq;
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

        private Button _playButton;
        private string _playButtonText;
        private RectOffset _playButtonPadding;
        private Color _playButtonColour;
        private GameplayModifiersPanelController _gameplayModifiersPanelController;
        private GameplayModifierToggle[] _gameplayModifierToggles;

        internal void OnLoad()
        {
            initialized = false;

            // setup settings
            SetupUI();
        }

        private void SetupUI()
        {
            if (initialized) return;

            // setup settings
            RectTransform mainMenu = (Resources.FindObjectsOfTypeAll<MainMenuViewController>().First().rectTransform);

            // create submenu for Versus settings
            var noFailCheckSubMenu = SettingsUI.CreateSubMenu("NoFail Check");

            // add option for enabling
            var isEnabled = noFailCheckSubMenu.AddBool("Enabled", "Enable or disable the NoFail check when playing a song.");
            isEnabled.EnabledText = "YES";
            isEnabled.DisabledText = "NO";
            isEnabled.GetValue += () => { return Plugin.cfg.Enabled; };
            isEnabled.SetValue += (v) => { Plugin.cfg.Set("Enabled", v); };

            // add option for requiring double press
            var doublePress = noFailCheckSubMenu.AddBool("Required Double Press", "If No Fail is enabled, require a double press of Play to start song.");
            doublePress.EnabledText = "YES";
            doublePress.DisabledText = "NO";
            doublePress.GetValue += () => { return Plugin.cfg.DoublePress; };
            doublePress.SetValue += (v) => { Plugin.cfg.Set("DoublePress", v); };

            // add option for changing button text
            var changeText = noFailCheckSubMenu.AddBool("Change Play Button Text", "Change the Play button's text when No Fail is enabled.");
            changeText.EnabledText = "YES";
            changeText.DisabledText = "NO";
            changeText.GetValue += () => { return Plugin.cfg.ChangeText; };
            changeText.SetValue += (v) => { Plugin.cfg.Set("ChangeText", v); };

            if (Plugin.cfg.Enabled)
            {
                // get the play button
                _playButton = Resources.FindObjectsOfTypeAll<Button>().First(x => x.name == "PlayButton");

                // save default values
                _playButtonText = _playButton.GetButtonText();
                _playButtonPadding = _playButton.GetTextPaddingConstraints();
                //_playButtonColour = _playButton.GetBorderColor();

                // get gameplay modifier related objects
                _gameplayModifiersPanelController = Resources.FindObjectsOfTypeAll<GameplayModifiersPanelController>().First();
                _gameplayModifierToggles = _gameplayModifiersPanelController.GetComponentsInChildren<GameplayModifierToggle>();

                foreach (var gameplayModifierToggle in _gameplayModifierToggles)
                {
                    //Plugin.Log.Info($"{gameplayModifierToggle.gameplayModifier.name}");
                    if (gameplayModifierToggle.gameplayModifier.modifierName == "MODIFIER_NO_FAIL")
                    {
                        // add listeners to modifier toggle
                        gameplayModifierToggle.toggle.onValueChanged.RemoveListener(new UnityAction<bool>(HandleNoFailToggle));
                        gameplayModifierToggle.toggle.onValueChanged.AddListener(new UnityAction<bool>(HandleNoFailToggle));

                        // set initial state
                        NoFailEnabled = gameplayModifierToggle.toggle.isOn;

                        // setup default button state
                        SetButtonState(gameplayModifierToggle.toggle.isOn);
                    }
                }
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
            if (Plugin.cfg.ChangeText)
            {
                if (state)
                {
                    _playButton.SetTextPadding(new RectOffset(6, 6, 0, 0));
                    _playButton.SetButtonText("No Fail!");
                    //_playButton.SetBorderColor(Color.cyan);
                }
                else
                {
                    _playButton.SetTextPadding(_playButtonPadding);
                    _playButton.SetButtonText(_playButtonText);
                    //_playButton.SetBorderColor(_playButtonColour);
                }
            }
        }
    }
}
