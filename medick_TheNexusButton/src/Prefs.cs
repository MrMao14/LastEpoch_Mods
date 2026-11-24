using System;
using MelonLoader;
using UnityEngine;

namespace medick_A_Terrible_Button;

internal static class Prefs
{
    internal static MelonPreferences_Category Category;
    internal static MelonPreferences_Entry<float> OffsetX;
    internal static MelonPreferences_Entry<float> OffsetY;
    internal static MelonPreferences_Entry<bool> HotkeyEnabled;
    internal static MelonPreferences_Entry<bool> ShowButton;
    internal static MelonPreferences_Entry<bool> DebugLog;
    static bool _saveWarned;

    internal static void Init()
    {
        Category = MelonPreferences.CreateCategory("medick_A_Terrible_Button", "枢纽按钮");
        OffsetX = Category.CreateEntry("OffsetX", 0f, "枢纽按钮横向偏移");
        OffsetY = Category.CreateEntry("OffsetY", 0f, "枢纽按钮纵向偏移");
        // Off by default (Andrew 20:54): players who already use N must opt in.
        HotkeyEnabled = Category.CreateEntry("HotkeyEnabled", false, "按 N 键打开枢纽");
        // Andrew 21:59: let hotkey-only players hide the button. On by default.
        ShowButton = Category.CreateEntry("ShowButton", true, "显示枢纽按钮");
        DebugLog = Category.CreateEntry("DebugLog", false, "每次点击时输出按钮布局和诊断信息");
        Category.SetFilePath("UserData/medick_A_Terrible_Button.cfg", autoload: true);
        OffsetX.Value = Normalize(OffsetX.Value, 300f);
        OffsetY.Value = Normalize(OffsetY.Value, 150f);
    }

    static float Normalize(float value, float limit)
        => float.IsFinite(value) ? Mathf.Round(Mathf.Clamp(value, -limit, limit)) : 0f;

    internal static void Save()
    {
        try { Category.SaveToFile(false); }
        catch (Exception ex)
        {
            if (_saveWarned) return;
            _saveWarned = true;
            MelonLogger.Warning("Nexus button prefs save failed: " + ex.Message);
        }
    }
}
