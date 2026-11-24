namespace medick_CameraZoom
{
    // Single source of truth for identity/version — referenced by the
    // MelonInfo assembly attribute and the settings panel chrome.
    // Internal name stays medick_CameraZoom forever (DLL, prefs, Nexus
    // upgrade path); the brand is what players see.
    internal static class BuildInfo
    {
        public const string Name         = "medick_CameraZoom";
        public const string DisplayName  = "镜头缩放";
        public const string OfficialName = "MedicK 镜头缩放";
        public const string Tagline      = "镜头模组";
        public const string Version      = "1.0.2";
        public const string Author       = "medick";
    }
}
