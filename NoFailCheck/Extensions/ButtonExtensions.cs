using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NoFailCheck.Extensions
{
    public static class ButtonExtensions
    {
        public static string GetButtonText(this Button btn)
        {
            return btn.GetComponentInChildren<TextMeshProUGUI>().text;
        }

        public static Color GetBorderColor(this Button button)
        {
            return button.gameObject.GetComponentInChildren<UnityEngine.UI.Image>().color;
        }

        public static void SetBorderColor(this Button button, Color color)
        {
            button.gameObject.GetComponentInChildren<UnityEngine.UI.Image>().color = color;
        }

        public static RectOffset GetTextPaddingConstraints(this Button btn)
        {
            RectTransform textTransform = btn.GetComponentsInChildren<RectTransform>(true).First(c => c.name == "Text");

            // set padding around icon
            HorizontalLayoutGroup hgroup = textTransform.parent.GetComponent<HorizontalLayoutGroup>();
            return hgroup.padding;
        }

        public static void SetTextPadding(this Button btn, RectOffset offset)
        {
            var _iconLayout = btn.GetComponentsInChildren<HorizontalLayoutGroup>().First(x => x.name == "Content");
            _iconLayout.padding = offset;
        }

        public static void SetButtonText(this Button _button, string _text)
        {
            Polyglot.LocalizedTextMeshProUGUI localizer = _button.GetComponentInChildren<Polyglot.LocalizedTextMeshProUGUI>();
            if (localizer != null)
                GameObject.Destroy(localizer);
            TextMeshProUGUI tmpUgui = _button.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpUgui != null)
                tmpUgui.text = _text;
        }
    }
}
