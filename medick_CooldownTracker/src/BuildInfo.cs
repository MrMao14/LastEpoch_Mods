namespace medick_CooldownTracker
{
    // Single source of truth for identity/version — referenced by the
    // MelonInfo assembly attribute and the settings panel chrome.
    // Internal name stays medick_CooldownTracker forever (DLL, prefs,
    // Nexus upgrade path); the brand is what players see.
    internal static class BuildInfo
    {
        public const string Name         = "medick_CooldownTracker";
        public const string DisplayName  = "技能冷却";
        public const string OfficialName = "MedicK 技能冷却";
        public const string Tagline      = "冷却追踪";
        public const string Version      = "1.0.2"; // council 2026-09-11 #8
        public const string Author       = "medick";
    }
}
