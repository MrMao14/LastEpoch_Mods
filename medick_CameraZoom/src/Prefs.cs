using System.Collections.Generic;
using MelonLoader;

namespace medick_CameraZoom
{
    // All persisted settings. Category and entry names must stay stable
    // across versions — they are the in-place upgrade path for existing
    // users' MelonPreferences.cfg (v1.x values carry over untouched).
    internal static class Prefs
    {
        public const float DefaultPanelX = 24f;
        public const float DefaultPanelY = 120f;

        // One (Lo, Hi, Default) triple per tunable — the single source for
        // the CreateEntry default, the sanitizer clamp, and the slider range.
        public const float ZoomMinLo = -200f,  ZoomMinHi = -1f,  ZoomMinDefault = -40f;
        public const float PerScrollLo = 0.1f, PerScrollHi = 20f, PerScrollDefault = 3f;
        public const float SpeedLo = 0.5f,     SpeedHi = 30f,     SpeedDefault = 10f;
        public const float AngleLo = 20f,      AngleHi = 85f,     AngleDefaultPref = 55f;
        public const float MenuScaleLo = 0.7f, MenuScaleHi = 2.0f;

        public static MelonPreferences_Entry<float> ZoomMin;
        public static MelonPreferences_Entry<float> ZoomPerScroll;
        public static MelonPreferences_Entry<float> ZoomSpeed;
        public static MelonPreferences_Entry<bool>  LockAngle;
        public static MelonPreferences_Entry<float> Angle;
        public static MelonPreferences_Entry<float> MenuScale;
        public static MelonPreferences_Entry<float> PanelX;
        public static MelonPreferences_Entry<float> PanelY;
        public static MelonPreferences_Entry<bool>  DebugLog;

        public static void Init()
        {
            var cat = MelonPreferences.CreateCategory("medick_CameraZoom", "镜头缩放");
            // council 2026-09-11 #6
            ZoomMin       = cat.CreateEntry("ZoomMin",       ZoomMinDefault,   "最远缩放范围。数值越小可拉得越远。");
            ZoomPerScroll = cat.CreateEntry("ZoomPerScroll", PerScrollDefault, "每格滚轮的缩放变化量。");
            ZoomSpeed     = cat.CreateEntry("ZoomSpeed",     SpeedDefault,     "镜头缩放的平滑速度。");
            LockAngle     = cat.CreateEntry("LockAngle",     false,            "锁定镜头俯仰角，禁止倾斜。");
            Angle         = cat.CreateEntry("Angle",         AngleDefaultPref, "锁定时的镜头倾角，单位为度。");
            MenuScale     = cat.CreateEntry("MenuScale",     1.0f, "设置面板缩放比例");
            PanelX        = cat.CreateEntry("PanelX", DefaultPanelX, "设置面板横向位置");
            PanelY        = cat.CreateEntry("PanelY", DefaultPanelY, "设置面板纵向位置");
            DebugLog      = cat.CreateEntry("DebugLog",     false, "输出详细调试日志");
            Sanitize();
        }

        // council 2026-09-11 #3
        static void Sanitize()
        {
            var repaired = new List<string>();
            RepairFinite(ZoomMin, "ZoomMin", repaired);
            RepairFinite(ZoomPerScroll, "ZoomPerScroll", repaired);
            RepairFinite(ZoomSpeed, "ZoomSpeed", repaired);
            RepairFinite(Angle, "Angle", repaired);
            RepairFinite(MenuScale, "MenuScale", repaired);
            RepairFinite(PanelX, "PanelX", repaired);
            RepairFinite(PanelY, "PanelY", repaired);

            if (repaired.Count > 0)
                MelonLogger.Warning("Repaired non-finite preferences: " + string.Join(", ", repaired));
        }

        static void RepairFinite(MelonPreferences_Entry<float> entry, string key, List<string> repaired)
        {
            if (float.IsFinite(entry.Value)) return;
            entry.Value = entry.DefaultValue;
            repaired.Add(key);
        }

        public static void Save()
        {
            try { MelonPreferences.Save(); } catch { }
        }
    }

    internal static class Dbg
    {
        public static void Log(string msg)
        {
            if (Prefs.DebugLog != null && Prefs.DebugLog.Value)
                MelonLoader.MelonLogger.Msg("[debug] " + msg);
        }
    }
}
