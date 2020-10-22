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
