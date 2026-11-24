using Il2Cpp;

namespace medick_CameraZoom
{
    // Input blocking is re-asserted every update while wanted and released
    // once on close, only if this mod set forceDisableInput. Terrible
    // Cooldowns runs the identical guard, so both sides respect each other's
    // lock. Without the game's flag, panel drags also click-to-move.
    internal static class InputGuard
    {
        static bool _applied;

        public static void Apply(bool want)
        {
            try
            {
                var mgr = EpochInputManager.instance;
                if (mgr == null)
                {
                    if (!want) _applied = false;   // manager gone = nothing to restore
                    return;
                }
                if (want)
                {
                    // Re-assert while blocking: a scene change spawns a fresh
                    // manager with the flag reset, and a sibling mod's guard
                    // (Terrible Cooldowns runs the same technique) can flip it
                    // under us. Trusting our own last write is how input locks
                    // silently die; one Il2Cpp read per frame is cheap.
                    // council 2026-09-11 #1
                    if (!mgr.forceDisableInput)
                    {
                        mgr.forceDisableInput = true;
                        _applied = true;
                    }
                }
                else if (_applied)
                {
                    mgr.forceDisableInput = false;   // release once
                    _applied = false;
                }
            }
            catch { }
        }

        public static void Restore() => Apply(false);
    }
}
