// ================================================================
//  SettingsUi.cs — the in-game "Terrible Tooltips" settings category.
//
//  Row order is display order (each row is inserted after the last).
//  Titles keep their mythic-pink color tags — brand voice.
//  Legend rows are honest info rows (no checkbox, no apology).
//  The 69 stays. Forever.
// ================================================================

namespace medick_Terrible_Tooltips;

internal static class SettingsUi
{
    private const string Cat = BuildInfo.DisplayName;

    [HarmonyPatch(typeof(SettingsPanelTabNavigable), nameof(SettingsPanelTabNavigable.Awake))]
    internal static class Patch_SettingsPanel
    {
        private static void Postfix(SettingsPanelTabNavigable __instance)
        {
            try { Build(__instance); }
            catch (Exception ex)
            {
                NativeSettings.WarnDegradedOnce("settings build error: " + ex.Message);
            }
        }
    }

    // GLM/Astra 2026-09-10: preference callbacks invalidate and, where needed, rebuild live text.
    private static void SaveAndRefresh(bool reRender = false)
    {
        Prefs.Save();
        TooltipRecolor.MarkDirty();
        if (reRender) TooltipRecolor.ReRenderNow();
    }

    private static void Build(SettingsPanelTabNavigable settings)
    {
        if (!NativeSettings.TemplatesAvailable(settings)) return;

        // ── Tooltip ───────────────────────────────────────────────────
        NativeSettings.CreateToggle(settings, Cat, "TT - Master",
            "<color=#FF44FF>物品提示</color>",
            "所有物品提示着色功能的总开关。关闭后恢复游戏默认提示。",
            Prefs.EnableTooltips.Value,
            v => { Prefs.EnableTooltips.Value = v; SaveAndRefresh(); });

        NativeSettings.CreateToggle(settings, Cat, "TT - Tier Colors",
            "<color=#FF44FF>提示：层级颜色</color>",
            "按制作层级为层级标记和词缀文字着色。T1 为最弱，T7 为最强可制作层级。",
            Prefs.TooltipTierColors.Value,
            v => { Prefs.TooltipTierColors.Value = v; SaveAndRefresh(); });

        NativeSettings.CreateToggle(settings, Cat, "TT - Rank Colors",
            "<color=#FF44FF>提示：评级颜色</color>",
            "按浮动品质为 S/A/B/C/F 评级着色。F 为最低，S 为接近完美。即使层级相同，数值强度也可能差距很大。",
            Prefs.TooltipRankColors.Value,
            v => { Prefs.TooltipRankColors.Value = v; SaveAndRefresh(); });

        // ── v3 clean line — the layout toy box ────────────────────────
        // "by us giving the players the option to do B, do C, do A —
        //  an EHG dev can toy with it and create ideas" — the ship gate.
        NativeSettings.CreateEnumDropdown(settings, Cat, "TT - Layout",
            "<color=#FF44FF>提示布局</color>",
            "设置层级与评级标记在词缀行中的位置：左侧、右侧或行尾。",
            Prefs.Layout,
            i => { Prefs.Layout.Value = (TooltipLayout)i; SaveAndRefresh(reRender: true); });

        NativeSettings.CreateEnumDropdown(settings, Cat, "TT - Signal Style",
            "<color=#FF44FF>标记样式</color>",
            "徽章样式使用彩色底板显示层级与评级，纯文本样式只显示彩色文字。",
            Prefs.Style,
            i => { Prefs.Style.Value = (SignalStyle)i; SaveAndRefresh(reRender: true); });

        NativeSettings.CreateEnumDropdown(settings, Cat, "TT - Name Color",
            "<color=#FF44FF>词缀名称颜色</color>",
            "高阶词缀只给 T6/T7 着色；层级颜色按各层级着色；游戏默认仅给层级与评级标记着色。",
            Prefs.NameColorMode,
            i => { Prefs.NameColorMode.Value = (AffixNameColorMode)i; SaveAndRefresh(reRender: true); });

        NativeSettings.CreateToggle(settings, Cat, "TT - Show Signal",
            "<color=#FF44FF>显示层级和评级</color>",
            "在每个词缀行显示层级与浮动评级。关闭后会隐藏整个标记、分隔线和边框。",
            Prefs.ShowSignal.Value,
            v => { Prefs.ShowSignal.Value = v; SaveAndRefresh(reRender: true); });

        NativeSettings.CreateToggle(settings, Cat, "TT - Compact Tier",
            "<color=#FF44FF>层级缩写 (T7)</color>",
            "开启显示 T7，关闭显示“层级 7”。不影响地面标签。",
            Prefs.TierWord.Value == TierWordStyle.Compact,
            v => { Prefs.TierWord.Value = v ? TierWordStyle.Compact : TierWordStyle.Spelled; SaveAndRefresh(reRender: true); });

        NativeSettings.CreateToggle(settings, Cat, "TT - Unit Border",
            "<color=#FF44FF>层级与评级边框</color>",
            "在纯文本样式中给层级与评级标记绘制边框。",
            Prefs.UnitBorder.Value,
            v => { Prefs.UnitBorder.Value = v; SaveAndRefresh(reRender: true); });

        NativeSettings.CreateToggle(settings, Cat, "TT - Grade Letters",
            "<color=#FF44FF>显示评级字母</color>",
            "在每个词缀行显示 S/A/B/C/F 浮动评级。只关心层级时可以关闭。",
            Prefs.ShowGradeLetters.Value,
            v => { Prefs.ShowGradeLetters.Value = v; SaveAndRefresh(reRender: true); });

        NativeSettings.CreateToggle(settings, Cat, "TT - Pin Ranges",
            "<color=#FF44FF>始终显示范围</color>",
            "永久显示数值范围。默认关闭，悬停时按住 Alt 可临时查看。",
            Prefs.AlwaysShowRanges.Value,
            v => { Prefs.AlwaysShowRanges.Value = v; SaveAndRefresh(reRender: true); });

        NativeSettings.CreateToggle(settings, Cat, "TT - Pin Tier Details",
            "<color=#FF44FF>始终显示层级详情</color>",
            "永久显示完整层级说明。默认关闭，悬停时按住 Alt 可临时查看。",
            Prefs.AlwaysShowTierDetails.Value,
            v => { Prefs.AlwaysShowTierDetails.Value = v; SaveAndRefresh(reRender: true); });

        NativeSettings.CreateInfoRow(settings, Cat, "TT - Alt Hint",
            "<color=#FF44FF>按住 Alt 查看详情</color>",
            "悬停物品时按住 Alt，可展开数值范围、制作信息等完整内容。松开后恢复简洁显示。");

        // ── Color legend (honest info rows) ───────────────────────────
        NativeSettings.CreateInfoRow(settings, Cat, "TT - Tier Legend",
            "<color=#FF44FF>T7</color>  >  <color=#FA9E3D>T6</color>  >  <color=#A807FF>T5</color>  >  <color=#77ACFF>T4</color>  >  <color=#16FF0E>T3</color>  >  <color=#E1E1E1>T2</color>  >  <color=#DADADA>T1</color>",
            "层级颜色参考。T1 灰色最弱，T7 神话粉色最强。层级越高，数值上限越高。");

        NativeSettings.CreateInfoRow(settings, Cat, "TT - Rank Legend",
            "<color=#FF44FF>完美</color>    <color=#FF44FF>S</color>  >  <color=#FA9E3D>A</color>  >  <color=#A807FF>B</color>  >  <color=#77ACFF>C</color>  >  <color=#DADADA>F</color>    <color=#DADADA>最低</color>",
            "评级参考，表示词缀在所属层级中的浮动品质。F 为范围下限，S 为接近完美。");

        // ── Ground labels ─────────────────────────────────────────────
        NativeSettings.CreateEnumDropdown(settings, Cat, "TT - Label Style",
            "<color=#FF44FF>地面标签样式</color>",
            "设置掉落物标签内容：层级和评级、仅层级、仅评级或关闭。",
            Prefs.LabelStyle,
            i => { Prefs.LabelStyle.Value = (GroundLabelStyle)i; SaveAndRefresh(); });

        NativeSettings.CreateToggle(settings, Cat, "TT - Filter Only",
            "<color=#FF44FF>地面标签：仅过滤器物品</color>",
            "开启后只给过滤器高亮的物品显示层级和评级，减少地面信息干扰。",
            Prefs.LabelFilterOnly.Value,
            v => { Prefs.LabelFilterOnly.Value = v; SaveAndRefresh(); });

        NativeSettings.CreateToggle(settings, Cat, "TT - Hold Alt",
            "<color=#FF44FF>地面标签：按住 Alt 显示</color>",
            "默认隐藏地面标签，按住左 Alt 或右 Alt 时显示。",
            Prefs.LabelAltKey.Value,
            v => { Prefs.LabelAltKey.Value = v; SaveAndRefresh(); });

        // ── Filter rule # ─────────────────────────────────────────────
        NativeSettings.CreateEnumDropdown(settings, Cat, "TT - Rule Display",
            "<color=#FF44FF>提示：显示过滤器规则编号</color>",
            "悬停物品时显示命中的过滤器规则编号，可选择仅编号或编号加名称。",
            Prefs.ShowFilterRuleNumber,
            i => { Prefs.ShowFilterRuleNumber.Value = (FilterRuleDisplay)i; SaveAndRefresh(); });

        NativeSettings.CreateEnumDropdown(settings, Cat, "TT - Rule Position",
            "<color=#FF44FF>地面标签：规则编号位置</color>",
            "设置游戏过滤器规则编号相对层级和评级标签的位置。仅在地面标签启用时生效。",
            Prefs.LabelRulePosition,
            i => { Prefs.LabelRulePosition.Value = (RuleNumberPosition)i; SaveAndRefresh(); });

        NativeSettings.CreateInfoRow(settings, Cat, "TT - Position Example",
            "默认：腰带 <color=#FA9E3D>(69)</color> [<color=#A807FF>5</color><color=#FA9E3D>A</color>]      开头：<color=#FA9E3D>(69)</color> 腰带 [<color=#A807FF>5</color><color=#FA9E3D>A</color>]      末尾：腰带 [<color=#A807FF>5</color><color=#FA9E3D>A</color>] <color=#FA9E3D>(69)</color>",
            "位置预览。橙色编号是过滤器规则，彩色方括号是物品提示的层级和评级。");

        // ── Easter egg — always LAST ──────────────────────────────────
        NativeSettings.CreateButton(settings, Cat, "TT - Aarons House",
            "<color=#FA9E3D>去亚伦家</color>",
            "（不要按这个按钮）致敬 AaronActionRPG。若按钮无效，请先打开地图，点击神圣纪元，再关闭地图后重试。",
            () => AaronsHouse.TeleportToBazaar());
    }
}
