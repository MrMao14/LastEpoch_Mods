using System;
using Il2Cpp;
using MelonLoader;

namespace medick_Terrible_Inventory
{
    // The "Terrible Inventory" category in the game's settings screen.
    // Five honest toggles (taxonomy: all real controls, no info rows, no
    // dropdowns needed in this mod) — delivering the April 2026 wish:
    // "allow the users to select what they want showing".
    internal static class SettingsUi
    {
        const string Category = BuildInfo.DisplayName;

        public static void Inject(SettingsPanelTabNavigable panel)
        {
            if (panel == null) return;
            try
            {
                if (!NativeSettings.TemplatesAvailable(panel)) return;

                NativeSettings.CreateToggle(panel, Category, "medick_Inv_ShowStash",
                    "仓库按钮",
                    "可在任意位置打开仓库。关闭后会隐藏按钮。",
                    Prefs.ShowStash.Value,
                    v => SetAndApply(Prefs.ShowStash, v));

                NativeSettings.CreateToggle(panel, Category, "medick_Inv_ShowStashAll",
                    "全部存入按钮",
                    "把背包内全部物品存入仓库，并遵循词缀仓库页规则。",
                    Prefs.ShowStashAll.Value,
                    v => SetAndApply(Prefs.ShowStashAll, v));

                NativeSettings.CreateToggle(panel, Category, "medick_Inv_ShowVendor",
                    "商人按钮",
                    "可在任意位置打开 NPC 商人，方便出售杂物和获取金币。",
                    Prefs.ShowVendor.Value,
                    v => SetAndApply(Prefs.ShowVendor, v));

                NativeSettings.CreateToggle(panel, Category, "medick_Inv_ShowTeleport",
                    "快速传送菜单",
                    "背包面板上的可折叠菜单，包含阵营、地下城和枢纽。",
                    Prefs.ShowTeleport.Value,
                    v => SetAndApply(Prefs.ShowTeleport, v));

                NativeSettings.CreateToggle(panel, Category, "medick_Inv_DebugLog",
                    "调试日志",
                    "输出详细的 MelonLoader 日志，便于反馈问题。",
                    Prefs.DebugLog.Value,
                    v => { Prefs.DebugLog.Value = v; Prefs.Save(); });
            }
            catch (Exception ex)
            {
                // Latched: this Awake re-fires per panel instance.
                NativeSettings.WarnDegradedOnce("settings injection failed: " + ex.Message);
            }
        }

        static void SetAndApply(MelonPreferences_Entry<bool> pref, bool value)
        {
            pref.Value = value;
            Prefs.Save();
            InventoryUi.ApplyVisibility();   // live — no reopen needed
        }
    }
}
