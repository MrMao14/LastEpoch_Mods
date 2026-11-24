using HarmonyLib;
using Il2Cpp;
using Il2CppLE.Services.Cosmetics;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

[assembly: MelonInfo(typeof(LastEpoch_CosmeticUnlock.Main), "LastEpoch_CosmeticUnlock", "1.0.1", "RCInet")]
[assembly: MelonGame("Eleventh Hour Games", "Last Epoch")]

namespace LastEpoch_CosmeticUnlock
{
    public class Main : MelonMod
    {
        private static readonly string[] MenuScenes = { "ClientSplash", "PersistentUI", "Login", "CharacterSelectScene" };
        private string currentScene = "";
        private bool menuProcessed;

        public override void OnInitializeMelon()
        {
            CosmeticSaveRepair.QuarantineInvalidSave();
        }

        public override void OnUpdate()
        {
            string scene = SceneManager.GetActiveScene().name;
            if (currentScene != scene)
            {
                currentScene = scene;
                menuProcessed = false;
            }

            if (!menuProcessed && IsGameScene(scene)) { menuProcessed = HideCosmeticsButton(); }
        }

        private static bool IsGameScene(string scene)
        {
            return !string.IsNullOrWhiteSpace(scene) && (System.Array.IndexOf(MenuScenes, scene) < 0);
        }

        private static bool HideCosmeticsButton()
        {
            UIBase ui = UIBase.instance;
            if ((ui == null) || (ui.bottomScreenMenu == null)) { return false; }

            GameObject panel = GetChild(ui.bottomScreenMenu.gameObject, "BottomScreenMenuPanel");
            GameObject cosmetics = GetChild(panel, "Cosmetics");
            if (cosmetics == null) { return false; }

            cosmetics.active = false;
            return true;
        }

        private static GameObject GetChild(GameObject obj, string name)
        {
            if (obj == null) { return null; }

            for (int i = 0; i < obj.transform.childCount; i++)
            {
                GameObject child = obj.transform.GetChild(i).gameObject;
                if (child.name == name) { return child; }
            }

            return null;
        }
    }

    internal static class CosmeticSaveRepair
    {
        private const string FallbackFileName = "offline_cosmetic_data_equips.json";
        private static readonly object sync = new object();

        public static void QuarantineInvalidSave()
        {
            lock (sync)
            {
                string fileName = FallbackFileName;
                try
                {
                    string gameFileName = OfflineUserInventory.OfflineEquipsCosmeticsFileName;
                    if (!string.IsNullOrWhiteSpace(gameFileName)) { fileName = gameFileName; }
                }
                catch (System.Exception exception)
                {
                    MelonLogger.Warning($"Could not resolve the equipped cosmetics file name: {exception.Message}");
                }

                string path = System.IO.Path.Combine(Application.persistentDataPath, fileName);
                if (!System.IO.File.Exists(path)) { return; }

                long length;
                try { length = new System.IO.FileInfo(path).Length; }
                catch (System.Exception exception)
                {
                    MelonLogger.Warning($"Could not inspect the equipped cosmetics save: {exception.Message}");
                    return;
                }

                if (length > 0) { return; }

                string backupPath = $"{path}.corrupt-{System.DateTime.Now:yyyyMMdd-HHmmss-fff}.bak";
                try
                {
                    System.IO.File.Move(path, backupPath);
                    MelonLogger.Warning($"Moved invalid equipped cosmetics save to '{backupPath}'.");
                }
                catch (System.Exception exception)
                {
                    MelonLogger.Error($"Could not repair the equipped cosmetics save: {exception.Message}");
                }
            }
        }
    }

    [HarmonyPatch(typeof(OfflineUserInventory), "LoadEquips")]
    internal static class OfflineUserInventory_LoadEquips
    {
        [HarmonyPrefix]
        private static void Prefix()
        {
            CosmeticSaveRepair.QuarantineInvalidSave();
        }
    }

    [HarmonyPatch(typeof(OfflineUserInventory), "OfflineLoadAndUpdateEquips")]
    internal static class OfflineUserInventory_OfflineLoadAndUpdateEquips
    {
        [HarmonyPrefix]
        private static void Prefix()
        {
            CosmeticSaveRepair.QuarantineInvalidSave();
        }
    }

    [HarmonyPatch(typeof(CosmeticsManager), "GetOwnedCosmetics")]
    internal static class CosmeticsManager_GetOwnedCosmetics
    {
        private static Il2CppSystem.Collections.Generic.List<string> ownedCosmetics;

        [HarmonyPrefix]
        private static bool Prefix(ref Il2CppCysharp.Threading.Tasks.UniTask<Il2CppSystem.Collections.Generic.List<string>> __result)
        {
            if (ownedCosmetics == null)
            {
                ownedCosmetics = new Il2CppSystem.Collections.Generic.List<string>();
                foreach (Cosmetic cosmetic in Resources.FindObjectsOfTypeAll<Cosmetic>())
                {
                    string id = cosmetic.BackendID;
                    if ((!string.IsNullOrEmpty(id)) && (!ownedCosmetics.Contains(id))) { ownedCosmetics.Add(id); }
                }
            }

            __result = new Il2CppCysharp.Threading.Tasks.UniTask<Il2CppSystem.Collections.Generic.List<string>>(ownedCosmetics);
            return false;
        }
    }
}
