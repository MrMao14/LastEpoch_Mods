namespace medick_Terrible_Tooltips;

// Enum member names and order are FROZEN — the cfg round-trips them by name.
public enum GroundLabelStyle
{
    None,         // disabled — brackets never shown
    TierAndRank,  // [5A 3C 7S]   ← default
    TierOnly,     // [5 3 7]
    RankOnly      // [A C S]
}

public enum FilterRuleDisplay
{
    Off,           // nothing shown
    NumberOnly,    // "Rule#69"
    NumberAndName  // "Rule #69: Maxroll told me to pick this up blah blah"
}

public enum RuleNumberPosition
{
    EHGDefault,  // leave wherever EHG writes it (between name and our brackets)
    Start,       // "(69) PLATED BELT [5A 1F 4C]"
    End          // "PLATED BELT [5A 1F 4C] (69)"
}

// v3 clean-line layouts — the ship gate: several must work at release.
public enum TooltipLayout
{
    BadgeLeft,    // "Tier 5·A  58% increased Lightning Damage"  ← default (Andrew's pick)
    SignalRight,  // "58% increased Lightning Damage        Tier 5 A"
    Trailing      // "58% increased Lightning Damage — Tier 5 A"
}

public enum AffixNameColorMode
{
    TierColor,   // affix text wears its tier color (the WoW retina read)
    GameDefault, // the game's own text color; only the Tier·Grade signal is colored
    GreaterAffix // only Tier 6/7 affix text wears the ruled greater-affix tint ← default
}

public enum SignalStyle
{
    Badge,      // colored chip behind "Tier 7" / grades — the label look
    PlainText   // colored text only, no chips ← default
}

public enum TierWordStyle
{
    Spelled,
    Compact
}

public enum UnitSeparatorStyle
{
    Bar,
    Dot
}

public enum DividerStyle
{
    Strip,
    Glyph
}

public enum BorderColorMode
{
    Neutral,
    TierColor
}

// All persisted settings. Category name, entry names and the cfg path are
// FROZEN — the in-place upgrade path for v1.x users.
internal static class Prefs
{
    public static MelonPreferences_Category Category;

    // Tooltip
    public static MelonPreferences_Entry<bool> EnableTooltips;
    public static MelonPreferences_Entry<bool> TooltipTierColors;
    public static MelonPreferences_Entry<bool> TooltipRankColors;

    // v3 clean line
    public static MelonPreferences_Entry<TooltipLayout>       Layout;
    public static MelonPreferences_Entry<SignalStyle>         Style;
    public static MelonPreferences_Entry<AffixNameColorMode>  NameColorMode;
    public static MelonPreferences_Entry<bool>                ShowSignal;
    public static MelonPreferences_Entry<bool>                ShowGradeLetters;
    public static MelonPreferences_Entry<bool>                AlwaysShowRanges;
    public static MelonPreferences_Entry<bool>                AlwaysShowTierDetails;

    // Ground labels
    public static MelonPreferences_Entry<GroundLabelStyle> LabelStyle;
    public static MelonPreferences_Entry<bool>             LabelFilterOnly;
    public static MelonPreferences_Entry<bool>             LabelAltKey;

    // Filter rule
    public static MelonPreferences_Entry<FilterRuleDisplay>  ShowFilterRuleNumber;
    public static MelonPreferences_Entry<RuleNumberPosition> LabelRulePosition;

    // New in v2
    public static MelonPreferences_Entry<bool> DebugLog;
    public static MelonPreferences_Entry<string> GreaterAffixTint;
    public static MelonPreferences_Entry<bool> UnitBorder;
    public static MelonPreferences_Entry<TierWordStyle> TierWord;
    public static MelonPreferences_Entry<UnitSeparatorStyle> UnitSeparator;
    public static MelonPreferences_Entry<DividerStyle> DividerStyle;
    public static MelonPreferences_Entry<BorderColorMode> BorderColorMode;
    public static MelonPreferences_Entry<int> BorderThickness;
    public static MelonPreferences_Entry<int> BorderPadX;
    public static MelonPreferences_Entry<int> BorderPadY;
    public static MelonPreferences_Entry<bool> BorderDebug;

    public static void Init()
    {
        Category = MelonPreferences.CreateCategory("medick_Terrible_Tooltips", BuildInfo.DisplayName);

        EnableTooltips    = Category.CreateEntry("EnableTooltips",    true,
            "物品提示", "物品提示颜色与评级功能的总开关");
        TooltipTierColors = Category.CreateEntry("TooltipTierColors", true,
            "提示层级颜色", "按制作层级为词缀名称着色");
        TooltipRankColors = Category.CreateEntry("TooltipRankColors", true,
            "提示评级颜色", "按词缀浮动品质为评级字母着色");

        // v3 — THE CLEAN LINE (one line per affix; the essay dies)
        Layout = Category.CreateEntry("TooltipLayout", TooltipLayout.BadgeLeft,
            "提示布局", "设置层级与评级标记在词缀行中的位置");
        Style = Category.CreateEntry("SignalStyle", SignalStyle.PlainText,
            "标记样式", "纯文本使用彩色文字，徽章样式使用彩色底板");
        NameColorMode = Category.CreateEntry("AffixNameColor", AffixNameColorMode.GreaterAffix,
            "词缀名称颜色", "高阶词缀仅给 T6/T7 着色，层级颜色按各层级着色，游戏默认保留原色");
        ShowSignal = Category.CreateEntry("ShowSignal", true,
            "显示层级和评级", "在每个词缀行显示层级、评级及边框");
        ShowGradeLetters = Category.CreateEntry("ShowGradeLetters", true,
            "显示评级字母", "显示每个词缀的 S/A/B/C/F 浮动评级");
        AlwaysShowRanges = Category.CreateEntry("AlwaysShowRanges", false,
            "始终显示范围", "永久显示数值范围；默认隐藏，按住 Alt 查看");
        AlwaysShowTierDetails = Category.CreateEntry("AlwaysShowTierDetails", false,
            "始终显示层级详情", "永久显示完整层级说明；默认折叠，按住 Alt 查看");

        LabelStyle      = Category.CreateEntry("GroundLabelStyle", GroundLabelStyle.TierAndRank,
            "地面标签样式", "设置地面物品标签显示层级、评级或两者");
        LabelFilterOnly = Category.CreateEntry("GroundLabelFilterOnly", false,
            "地面标签：仅过滤器物品", "只给过滤器高亮的物品显示地面标签");
        LabelAltKey     = Category.CreateEntry("GroundLabelAltKey", false,
            "地面标签：按住 Alt 显示", "默认隐藏地面标签，按住 Alt 时显示");

        // Default changed Off → NumberOnly in v2.0.0 — the April decision,
        // finally shipped ("make a note to set... number only setting on by default").
        ShowFilterRuleNumber = Category.CreateEntry("ShowFilterRuleNumber", FilterRuleDisplay.NumberOnly,
            "提示：显示过滤器规则编号", "悬停物品时显示命中的过滤器规则编号");
        LabelRulePosition = Category.CreateEntry("LabelRulePosition", RuleNumberPosition.EHGDefault,
            "地面标签：规则编号位置", "设置过滤器规则编号在地面标签中的位置");

        DebugLog = Category.CreateEntry("DebugLog", false, "输出详细调试日志");
        GreaterAffixTint = Category.CreateEntry("GreaterAffixTint", Colors.GreaterAffixTintDefault,
            "高阶词缀颜色，用于 T6/T7 词缀文本。");
        UnitBorder = Category.CreateEntry("UnitBorder", true,
            "在纯文本样式的层级与评级标记外绘制细边框。");
        TierWord = Category.CreateEntry("TierWord", TierWordStyle.Spelled,
            "提示标记使用“层级 7”或缩写“T7”。不影响地面标签。");
        UnitSeparator = Category.CreateEntry("UnitSeparator", UnitSeparatorStyle.Bar,
            "选择层级与评级之间的分隔样式。不影响地面标签。");
        DividerStyle = Category.CreateEntry("DividerStyle", global::medick_Terrible_Tooltips.DividerStyle.Strip,
            "色条使用满高分隔图，符号模式显示所选分隔符。");
        BorderColorMode = Category.CreateEntry("BorderColorMode", global::medick_Terrible_Tooltips.BorderColorMode.Neutral,
            "边框颜色可选择中性色或当前层级颜色。");
        BorderThickness = Category.CreateEntry("BorderThickness", 2,
            "边框粗细，限制在 1 到 4。");
        BorderPadX = Category.CreateEntry("BorderPadX", 6,
            "边框横向内边距，限制在 2 到 12。");
        BorderPadY = Category.CreateEntry("BorderPadY", 2,
            "边框纵向内边距，限制在 0 到 6。");
        BorderDebug = Category.CreateEntry("BorderDebug", false,
            "用洋红色绘制边框并记录每次布局，便于排查位置问题。");

        Category.SetFilePath("UserData/medick_Terrible_Tooltips.cfg", autoload: true);
        WarnOrphanedKeys();
    }

    public static void Save()
    {
        // printmsg: false — settings clicks must not spam the console;
        // "one startup line" is the family logging contract.
        try { Category.SaveToFile(false); Dbg.Log("prefs saved"); }
        catch (Exception ex)
        {
            // GLM/Kimi 2026-09-10: persistence failure must not be silent.
            if (s_saveWarningLogged) return;
            s_saveWarningLogged = true;
            MelonLogger.Warning("prefs save failed: " + ex.Message);
        }
    }

    // GLM/Kimi 2026-09-10: warn about cfg keys that have no registered preference.
    private static void WarnOrphanedKeys()
    {
        if (s_orphanWarningChecked) return;
        s_orphanWarningChecked = true;

        try
        {
            const string path = "UserData/medick_Terrible_Tooltips.cfg";
            if (!System.IO.File.Exists(path)) return;

            var registered = new HashSet<string>(StringComparer.Ordinal)
            {
                "EnableTooltips", "TooltipTierColors", "TooltipRankColors",
                "TooltipLayout", "SignalStyle", "AffixNameColor", "ShowSignal", "ShowGradeLetters",
                "AlwaysShowRanges", "AlwaysShowTierDetails", "GroundLabelStyle",
                "GroundLabelFilterOnly", "GroundLabelAltKey", "ShowFilterRuleNumber",
                "LabelRulePosition", "DebugLog",
                "GreaterAffixTint", "UnitBorder", "TierWord", "UnitSeparator", "DividerStyle",
                "BorderColorMode", "BorderThickness", "BorderPadX", "BorderPadY", "BorderDebug"
            };
            var orphans = new HashSet<string>(StringComparer.Ordinal);

            foreach (string raw in System.IO.File.ReadAllLines(path))
            {
                string line = raw.Trim();
                if (line.Length == 0 || line[0] == '#' || line[0] == '[') continue;
                int equals = line.IndexOf('=');
                if (equals <= 0) continue;
                string key = line.Substring(0, equals).Trim();
                if (key.Length > 0 && !registered.Contains(key)) orphans.Add(key);
            }

            if (orphans.Count > 0)
                MelonLogger.Warning("unregistered preference keys in cfg: " +
                                    string.Join(", ", orphans.OrderBy(k => k)));
        }
        catch (Exception ex) { Dbg.Log("orphan preference scan failed: " + ex.Message); }
    }

    private static bool s_saveWarningLogged;
    private static bool s_orphanWarningChecked;
}

internal static class Dbg
{
    public static void Log(string msg)
    {
        if (Prefs.DebugLog != null && Prefs.DebugLog.Value)
            MelonLogger.Msg("[debug] " + msg);
    }
}
