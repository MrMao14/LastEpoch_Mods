using System;
using HarmonyLib;
using Il2Cpp;

namespace medick_A_Terrible_Button;

internal static class SettingsUi
{
    const string Category = "枢纽按钮";

    [HarmonyPatch(typeof(SettingsPanelTabNavigable), nameof(SettingsPanelTabNavigable.Awake))]
    internal static class Patch_SettingsPanel
    {
        static void Postfix(SettingsPanelTabNavigable __instance)
        {
            try
            {
                if (!NativeSettings.TemplatesAvailable(__instance)) return;

                NativeSettings.CreateToggle(__instance, Category, "NB - Show Button",
                    "显示枢纽按钮", "关闭后会隐藏按钮，只保留快捷键。",
                    Prefs.ShowButton.Value, on => { Prefs.ShowButton.Value = on; Prefs.Save(); });
                NativeSettings.CreateToggle(__instance, Category, "NB - Hotkey",
                    "枢纽快捷键 (N)", "按 N 键打开枢纽。默认关闭，确认 N 键未被占用后再启用。",
                    Prefs.HotkeyEnabled.Value, on => { Prefs.HotkeyEnabled.Value = on; Prefs.Save(); });
                var x = NativeSettings.CreateSlider(__instance, Category, "NB - Offset X",
                    "枢纽按钮横向偏移", -300f, 300f, Prefs.OffsetX.Value,
                    value => { Prefs.OffsetX.Value = value; SaveAndRefresh(); });
                var y = NativeSettings.CreateSlider(__instance, Category, "NB - Offset Y",
                    "枢纽按钮纵向偏移", -150f, 150f, Prefs.OffsetY.Value,
                    value => { Prefs.OffsetY.Value = value; SaveAndRefresh(); });
                NativeSettings.CreateButton(__instance, Category, "NB - Reset Position",
                    "重置枢纽按钮位置", () =>
                    {
                        Prefs.OffsetX.Value = Prefs.OffsetY.Value = 0f;
                        x.SetValue(0f);
                        y.SetValue(0f);
                        SaveAndRefresh();
                    });
                MelonLoader.MelonLogger.Msg("settings: '枢纽按钮' section built");
            }
            catch (Exception ex)
            {
                NativeSettings.RemoveSection(__instance, Category);
                NativeSettings.WarnDegradedOnce("settings build failed: " + ex.Message);
            }
        }
    }

    static void SaveAndRefresh()
    {
        NexusButton.FollowCrest();
        Prefs.Save();
    }
}
