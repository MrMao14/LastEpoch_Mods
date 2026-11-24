using System;
using MelonLoader;

namespace medick_FogOfWar
{
    // All persisted settings. Category name, entry name, and the custom cfg
    // path are frozen — they are the in-place upgrade path for existing
    // users' UserData/medick_The_fogOFwar.cfg.
    internal static class Prefs
    {
        public static MelonPreferences_Category Category;
        public static MelonPreferences_Entry<MapVisionLevel> FogLevel;
        public static MelonPreferences_Entry<bool> DebugLog;
        static bool _saveFailedWarned;

        public static void Init()
        {
            Category = MelonPreferences.CreateCategory("medick_The_fogOFwar", "地图视野");
            FogLevel = Category.CreateEntry(
                "VisionLevel",
                MapVisionLevel.NORMAL,
                "地图视野等级",
                "0=失明  1=困难  2=受限  3=正常  4=侦察  5=神谕");
            DebugLog = Category.CreateEntry("DebugLog", false, "输出详细调试日志");
            Category.SetFilePath("UserData/medick_The_fogOFwar.cfg", autoload: true);
        }

        public static void Save()
        {
            // printmsg: false — every save would otherwise log a console
            // line per settings click; "one startup line" is the contract.
            try { Category.SaveToFile(false); Dbg.Log("prefs saved"); }
            catch (Exception ex)
            {
                // council 2026-09-11 #7 — report persistence failure once per session.
                if (_saveFailedWarned) return;
                _saveFailedWarned = true;
                MelonLogger.Warning($"prefs save failed ({ex.Message}) — your vision level may not persist to the next launch");
            }
        }
    }

    internal static class Dbg
    {
        public static void Log(string msg)
        {
            if (Prefs.DebugLog != null && Prefs.DebugLog.Value)
                MelonLogger.Msg("[debug] " + msg);
        }
    }
}
