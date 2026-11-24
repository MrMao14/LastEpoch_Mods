namespace medick_FogOfWar
{
    // Single source of truth for identity/version. Internal name stays
    // medick_The_fogOFwar forever (DLL, prefs category, cfg path, Nexus
    // upgrade path); the brand is what players see.
    internal static class BuildInfo
    {
        public const string Name         = "medick_The_fogOFwar";
        // The OF stands alone on purpose. You know why. ("bastos" branding)
        public const string DisplayName  = "战争迷雾";   // settings category title
        public const string OfficialName = "MedicK 战争迷雾";
        public const string Version      = "1.0.2";
        public const string Author       = "medick";
    }
}
