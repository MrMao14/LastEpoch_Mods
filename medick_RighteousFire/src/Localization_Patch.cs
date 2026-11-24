// ================================================================
//  Localization_Patch.cs  —  medick_RighteousFire
//
//  Intercepts the game's localization system to inject display
//  text for the RIGHTEOUS FIRE belt.
// ================================================================

namespace medick_RighteousFire;

public static class LocalizationHelper
{
    public static bool TryGet(string key, out string text)
    {
        if (key == Item_RighteousFire.Get_Subtype_Name())
        {
            text = "正义之火腰带"; return true;
        }
        if (key == Item_RighteousFire.Get_Unique_Name())
        {
            text = "正义之火"; return true;
        }
        if (key == Item_RighteousFire.Get_Tooltip_0())
        {
            text = "装备此腰带会点燃你。\n" +
                   "你心中燃烧的火焰，也会焚尽周围的一切。\n\n" +
                   "盾牌冲锋会自动冲向最近的敌人。\n" +
                   "火焰陷阱会自动引爆并连锁爆炸。\n" +
                   "使用药水会使光环效果翻倍，持续 8 秒。\n" +
                   "你自己也会着火，但生命回复足以抵消。\n\n" +
                   "— Pohx";
            return true;
        }
        if (key == Item_RighteousFire.Get_Unique_Lore())
        {
            text = "献给那个让“站在火里”成为正统战术的人。" +
                   "仅供离线模式使用。请自行承担风险。♥";
            return true;
        }

        text = null;
        return false;
    }
}

[HarmonyPatch(typeof(Localization), "GetText")]
public static class Localization_GetText_Patch
{
    public static bool Prefix(string key, ref string __result)
    {
        if (LocalizationHelper.TryGet(key, out string text))
        {
            __result = text;
            return false;
        }
        return true;
    }
}

[HarmonyPatch(typeof(Localization), "TryGetText")]
public static class Localization_TryGetText_Patch
{
    public static bool Prefix(string key, ref string text, ref bool __result)
    {
        if (LocalizationHelper.TryGet(key, out string value))
        {
            text     = value;
            __result = true;
            return false;
        }
        return true;
    }
}
