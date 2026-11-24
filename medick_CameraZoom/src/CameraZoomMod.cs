using Il2Cpp;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(medick_CameraZoom.CameraZoomMod),
    medick_CameraZoom.BuildInfo.Name,
    medick_CameraZoom.BuildInfo.Version,
    medick_CameraZoom.BuildInfo.Author)]
[assembly: MelonGame("Eleventh Hour Games", "Last Epoch")]

namespace medick_CameraZoom
{
    // Camera zoom/angle control for Last Epoch — writes CameraManager
    // fields, never Camera.main.fieldOfView (the manager overrides FOV
    // every frame). End toggles the settings panel; see README.md.
    public partial class CameraZoomMod : MelonMod
    {
        const int CameraManagerFailureFrames = 300;
        static int _cameraManagerFailureStreak;
        static bool _cameraManagerFailureLogged;

        public override void OnInitializeMelon()
        {
            Prefs.Init();
            MelonLogger.Msg($"{BuildInfo.OfficialName} v{BuildInfo.Version} ready — scroll zooms, End opens settings");
        }

        public override void OnUpdate()
        {
            // council 2026-09-11 #4
            if (Input.GetKeyDown(KeyCode.End) && !TypingInGameField())
            {
                if (SettingsPanel.Visible) SettingsPanel.Close();
                else                       SettingsPanel.Visible = true;
            }

            // Pause the game's own input while the pointer is on the open
            // panel (or a panel control is mid-drag) — otherwise every slider
            // drag is also a click-to-move command. Transition-only writes.
            InputGuard.Apply(SettingsPanel.Visible
                && (SettingsPanel.PointerOver || GUIUtility.hotControl != 0));

            try
            {
                var mgr = CameraManager.instance;
                if (mgr == null)
                {
                    _cameraManagerFailureStreak = 0;
                    return;
                }
                CameraState.TryCapture(mgr);
                CameraState.Apply(mgr);
                _cameraManagerFailureStreak = 0;
            }
            catch
            {
                // council 2026-09-11 #5
                if (_cameraManagerFailureStreak < int.MaxValue)
                    _cameraManagerFailureStreak++;
                if (_cameraManagerFailureStreak == CameraManagerFailureFrames
                    && !_cameraManagerFailureLogged)
                {
                    _cameraManagerFailureLogged = true;
                    MelonLogger.Warning("camera manager unreachable for 300 updates — mod idle; a game update may have changed CameraManager");
                }
            }
        }

        static bool TypingInGameField()
        {
            try
            {
                var mgr = EpochInputManager.instance;
                return mgr != null && mgr.IsInputFieldActive;
            }
            catch { return false; }
        }

        public override void OnGUI()
        {
            Theme.Ensure();
            if (SettingsPanel.Visible) SettingsPanel.Draw();
        }

        // Leave the camera exactly as the game owns it if the mod unloads.
        public override void OnDeinitializeMelon()
        {
            try { InputGuard.Restore(); } catch { }
            try { CameraState.RestoreToGame(CameraManager.instance); } catch { }
        }

        public override void OnApplicationQuit() => Prefs.Save();
    }
}
