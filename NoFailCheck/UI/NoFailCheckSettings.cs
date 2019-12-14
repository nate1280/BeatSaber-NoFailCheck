using BeatSaberMarkupLanguage.Attributes;

namespace NoFailCheck.UI
{
    public class NoFailCheckSettings : PersistentSingleton<NoFailCheckSettings>
    {
        [UIValue("enabled")]
        public bool Enabled
        {
            get => Plugin.cfg.Enabled;
            set => Plugin.cfg.Set("Enabled", value);
        }

        [UIValue("double-press")]
        public bool DoublePress
        {
            get => Plugin.cfg.DoublePress;
            set => Plugin.cfg.Set("DoublePress", value);
        }

        [UIValue("change-text")]
        public bool ChangeText
        {
            get => Plugin.cfg.ChangeText;
            set => Plugin.cfg.Set("ChangeText", value);
        }
    }
}
