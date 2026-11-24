using MelonLoader;

namespace medick_Terrible_Inventory
{
    // Persisted settings — NEW in v2 (v1 had no preferences at all).
    // Delivers the April 2026 dropped wish: "allow the users to select what
    // they want showing".
    internal static class Prefs
    {
        public static MelonPreferences_Category Category;
        public static MelonPreferences_Entry<bool> ShowStash;
        public static MelonPreferences_Entry<bool> ShowStashAll;
        public static MelonPreferences_Entry<bool> ShowVendor;
        public static MelonPreferences_Entry<bool> ShowTeleport;
        public static MelonPreferences_Entry<bool> DebugLog;

        public static void Init()
        {
            Category     = MelonPreferences.CreateCategory("medick_Terrible_Inventory", "实用背包");
            ShowStash    = Category.CreateEntry("ShowStash",    true,  "显示仓库按钮");
            ShowStashAll = Category.CreateEntry("ShowStashAll", true,  "显示全部存入按钮");
            ShowVendor   = Category.CreateEntry("ShowVendor",   false, "显示商人按钮");
            ShowTeleport = Category.CreateEntry("ShowTeleport", true,  "显示快速传送菜单");
            DebugLog     = Category.CreateEntry("DebugLog",     false, "输出详细调试日志");
        }

        public static void Save()
        {
            try { Category.SaveToFile(false); Dbg.Log("prefs saved"); } catch { }
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
