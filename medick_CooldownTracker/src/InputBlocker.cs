using Il2Cpp;

namespace medick_CooldownTracker
{
    // Uses the game's EpochInputManager.forceDisableInput flag instead of
    // patching UnityEngine.Input, which the game does not route through.
    // While wanted, the lock is re-asserted every update; on close it is
    // released once, and only when this mod was the one that set the flag.
    // council 2026-09-11 #1
    internal static class InputBlocker
    {
        static bool _applied;

        public static void Apply(bool want)
        {
            try
            {
                var mgr = EpochInputManager.instance;
                if (mgr == null)
                {
                    if (!want) _applied = false;   // manager gone = nothing left to restore
                    return;
                }
                if (want)
                {
                    // Re-assert while locking: a scene change spawns a fresh
                    // manager with the flag reset (Andrew's report: panel open
                    // across a transition = lock silently off), and a sibling
                    // mod's guard (Terrible Zoom runs the same technique) can
                    // flip the shared flag under us. Trusting our own last
                    // write is how input locks silently die.
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
