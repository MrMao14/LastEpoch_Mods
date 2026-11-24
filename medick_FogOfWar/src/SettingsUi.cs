using System;
using Il2Cpp;
using MelonLoader;
using UnityEngine.UI;

namespace medick_FogOfWar
{
    // Builds the "Terrible fog_OF_war" category in the game's settings:
    // six colour-coded legend rows that ARE the control — click a level to
    // select it; the active level's row stays checked, radio-style.
    //
    // v2.0 history: v1 had a dropdown plus dead "not clickable" legend rows;
    // the first v2 build made the rows clickable but kept the dropdown.
    // Once the rows proved out in-game, the dropdown was redundant and cut.
    internal static class SettingsUi
    {
        const string Category = BuildInfo.DisplayName;

        static readonly Toggle[] _legendToggles = new Toggle[6];

        // Title colours follow the family tier palette (gray → mythic pink).
        static readonly (int level, string row, string title, string desc)[] Legend =
        {
            (0, "fogOFwar - 0 BLIND",
                "<color=#DADADA>0  失明</color>",
                "小地图会立即隐藏，离开该模式后恢复。大地图仍可打开。点击选择。"),
            (1, "fogOFwar - 1 HARD",
                "<color=#E1E1E1>1  困难</color>",
                "小地图仍显示，但不会揭示新区域。已探索区域会保留到进入下一个区域。点击选择。"),
            (2, "fogOFwar - 2 LIMITED",
                "<color=#77ACFF>2  受限</color>",
                "揭示范围为默认值的 69%。点击选择。"),
            (3, "fogOFwar - 3 NORMAL",
                "<color=#16FF0E>3  正常</color>",
                "游戏默认状态，模组保持闲置。已揭示的迷雾会保留到进入下一个区域。点击选择。"),
            (4, "fogOFwar - 4 SCOUT",
                "<color=#FA9E3D>4  侦察</color>",
                "揭示范围提升到默认值的 3 倍，立即生效。点击选择。"),
            (5, "fogOFwar - 5 ORACLE",
                "<color=#FF44FF>5  神谕</color>",
                "揭示半径 600，覆盖整个区域，且不会明显影响帧率。点击选择。"),
        };

        public static void Inject(SettingsPanelTabNavigable panel)
        {
            if (panel == null) return;
            try
            {
                // One up-front probe: missing template/anchor → one latched
                // warning, no injection, fog still configurable via cfg.
                if (!NativeSettings.TemplatesAvailable(panel)) return;

                foreach (var (level, row, title, desc) in Legend)
                {
                    int lv = level;   // capture per iteration, not the loop variable
                    _legendToggles[lv] = NativeSettings.CreateButton(panel,
                        Category, row, title, desc, () => SelectLevel(lv));
                }

                SyncLegend((int)Prefs.FogLevel.Value);
            }
            catch (Exception ex)
            {
                MelonLogger.Warning("settings injection failed: " + ex.Message);
            }
        }

        static void SelectLevel(int level)
        {
            Prefs.FogLevel.Value = (MapVisionLevel)level;
            Prefs.Save();
            FogController.Apply();
            SyncLegend(level);
            Dbg.Log($"level selected: {Prefs.FogLevel.Value}");
        }

        // Radio-style selection state on the legend rows.
        static void SyncLegend(int level)
        {
            for (int i = 0; i < _legendToggles.Length; i++)
            {
                var t = _legendToggles[i];
                if (t == null) continue;
                try { t.SetIsOnWithoutNotify(i == level); }
                catch { }   // stripped interop method: rows stay unchecked, clicks still work
            }
        }
    }
}
