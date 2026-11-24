using System;
using System.Collections.Generic;
using MelonLoader;

namespace medick_CooldownTracker
{
    // All persisted settings. Category and entry names must stay stable across
    // versions — they are the in-place upgrade path for existing users'
    // MelonPreferences.cfg (v4.x labels and sliders carry over untouched).
    internal static class Prefs
    {
        public const float DefaultPanelX = 24f;
        public const float DefaultPanelY = 120f;

        public static MelonPreferences_Entry<float> Alpha;
        public static MelonPreferences_Entry<float> Size;
        public static MelonPreferences_Entry<float> OffsetX;
        public static MelonPreferences_Entry<float> OffsetY;
        public static MelonPreferences_Entry<float> MenuScale;
        public static MelonPreferences_Entry<int>   InputMode;   // 0=Auto 1=KB 2=Xbox 3=PS5
        public static MelonPreferences_Entry<int>   CtrlLayout;  // 0=Auto 1=Xbox 2=PS5
        public static MelonPreferences_Entry<bool>  LockInput;
        public static MelonPreferences_Entry<bool>  ButtonBadges;
        public static MelonPreferences_Entry<bool>  DebugLog;
        public static MelonPreferences_Entry<float> PanelX;
        public static MelonPreferences_Entry<float> PanelY;

        // [modeIdx 0=Keyboard 1=Xbox 2=PlayStation][slot 0-6] — fully independent per mode
        public static readonly MelonPreferences_Entry<string>[][] SlotLabels =
        {
            new MelonPreferences_Entry<string>[SlotRegistry.SlotCount],
            new MelonPreferences_Entry<string>[SlotRegistry.SlotCount],
            new MelonPreferences_Entry<string>[SlotRegistry.SlotCount],
        };
        public static readonly MelonPreferences_Entry<bool>[] SlotEnabled =
            new MelonPreferences_Entry<bool>[SlotRegistry.SlotCount];

        static readonly string[] ModePrefPfx = { "Kb", "Xb", "Ps" };
        public static readonly string[] ModeDispName = { "键盘", "Xbox", "PS5" };

        public static void Init()
        {
            var cat = MelonPreferences.CreateCategory("medick_CooldownTracker", "技能冷却");
            Alpha      = cat.CreateEntry("Alpha",      0.92f, "图标不透明度");
            Size       = cat.CreateEntry("Size",       64f,   "图标大小（像素）");
            OffsetX    = cat.CreateEntry("OffsetX",    0f,    "图标相对玩家的横向偏移");
            OffsetY    = cat.CreateEntry("OffsetY",    -160f, "图标相对玩家的纵向偏移");
            MenuScale  = cat.CreateEntry("MenuScale",  1.0f,  "设置面板缩放");
            InputMode  = cat.CreateEntry("InputMode",  0,     "0=自动 1=键盘 2=Xbox 3=PS5");
            CtrlLayout = cat.CreateEntry("CtrlLayout", 0,     "0=自动 1=Xbox 2=PS5");
            LockInput  = cat.CreateEntry("LockInputWhenOpen", false, "设置面板打开时阻止角色移动");
            ButtonBadges = cat.CreateEntry("ButtonBadges", true, "使用手柄按键徽章替代文字标签");
            DebugLog   = cat.CreateEntry("DebugLog",   false, "输出详细调试日志");
            PanelX     = cat.CreateEntry("PanelX",     DefaultPanelX, "设置面板横向位置");
            PanelY     = cat.CreateEntry("PanelY",     DefaultPanelY, "设置面板纵向位置");

            for (int m = 0; m < 3; m++)
                for (int i = 0; i < SlotRegistry.SlotCount; i++)
                    SlotLabels[m][i] = cat.CreateEntry(
                        $"SlotLabel{ModePrefPfx[m]}{i}", "",
                        $"自定义标签 [{ModeDispName[m]}] 技能槽 {i}");

            for (int i = 0; i < SlotRegistry.SlotCount; i++)
                SlotEnabled[i] = cat.CreateEntry($"SlotEnabled{i}", true, $"追踪技能槽 {i}");

            Sanitize();
        }

        // council 2026-09-11 #3
        static void Sanitize()
        {
            var repaired = new List<string>();
            RepairFinite(Alpha, "Alpha", repaired);
            RepairFinite(Size, "Size", repaired);
            RepairFinite(OffsetX, "OffsetX", repaired);
            RepairFinite(OffsetY, "OffsetY", repaired);
            RepairFinite(MenuScale, "MenuScale", repaired);
            RepairFinite(PanelX, "PanelX", repaired);
            RepairFinite(PanelY, "PanelY", repaired);

            int inputMode = Math.Clamp(InputMode.Value, 0, 3);
            if (inputMode != InputMode.Value)
            {
                InputMode.Value = inputMode;
                repaired.Add("InputMode");
            }

            int ctrlLayout = Math.Clamp(CtrlLayout.Value, 0, 2);
            if (ctrlLayout != CtrlLayout.Value)
            {
                CtrlLayout.Value = ctrlLayout;
                repaired.Add("CtrlLayout");
            }

            if (repaired.Count > 0)
                MelonLogger.Warning("Repaired invalid preferences: " + string.Join(", ", repaired));
        }

        static void RepairFinite(MelonPreferences_Entry<float> entry, string key, List<string> repaired)
        {
            if (float.IsFinite(entry.Value)) return;
            entry.Value = entry.DefaultValue;
            repaired.Add(key);
        }

        public static string CustomLabel(int modeIdx, int slot)
        {
            if ((uint)modeIdx >= (uint)SlotLabels.Length) return "";
            var row = SlotLabels[modeIdx];
            if (row == null || (uint)slot >= (uint)row.Length) return "";
            return row[slot]?.Value ?? "";
        }

        public static void SetCustomLabel(int modeIdx, int slot, string value)
        {
            if ((uint)modeIdx >= (uint)SlotLabels.Length) return;
            var row = SlotLabels[modeIdx];
            if (row == null || (uint)slot >= (uint)row.Length) return;
            if (row[slot] != null) row[slot].Value = value;
        }

        public static bool IsSlotEnabled(int slot) =>
            (uint)slot >= (uint)SlotEnabled.Length || (SlotEnabled[slot]?.Value ?? true);

        public static void SetSlotEnabled(int slot, bool on)
        {
            if ((uint)slot < (uint)SlotEnabled.Length && SlotEnabled[slot] != null)
                SlotEnabled[slot].Value = on;
        }

        public static void Save()
        {
            try { MelonPreferences.Save(); } catch { }
        }
    }
}
