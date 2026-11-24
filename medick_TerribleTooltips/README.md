# MedicK 物品提示 v3.1.2

适用于 **Last Epoch** 的 [MelonLoader](https://melonwiki.xyz) 物品提示增强模组，为词缀、地面掉落和过滤器信息增加层级、评级与颜色。

> Melon 名称保持 `Terrible Tooltips`，用于兼容 Fallen Star 模组检测。  
> DLL：`medick_Terrible_Tooltips.dll`

## 主要功能

### 单行词缀

- 每个词缀显示为一行：词缀文本 + 层级 + 评级。
- 游戏原生的“层级”和“范围”长说明默认折叠。
- 悬停时按住 `Alt` 可展开完整数值范围与制作信息。
- 支持左侧徽章、右侧标记和行尾显示三种布局。

### 层级和评级颜色

- 层级颜色：T1 灰色到 T7 神话粉色。
- 评级字母：F、C、B、A、S。
- 传奇物品从游戏实际浮动值读取评级。

### 地面标签

- 地面掉落显示类似 `[5A 3C 7S]` 的层级和评级标签。
- 支持仅层级、仅评级、关闭和仅过滤器物品。
- 可选择只在按住 `Alt` 时显示。
- 独特、套装和传奇地面物品仍交给 Fallen Star 模组处理。

### 过滤器规则编号

- 悬停物品时显示命中的过滤器规则编号。
- 可显示为“规则#69”或“规则 #69: 规则名称”。
- 可调整规则编号在地面标签中的位置。

## 设置

大部分选项可在游戏设置中的 **物品提示** 分类调整。完整配置保存在：

```text
UserData\medick_Terrible_Tooltips.cfg
```

修改配置前请先关闭游戏，因为游戏退出时会重写该文件。

## 配置键

| 配置键 | 默认值 | 说明 |
|---|---|---|
| `EnableTooltips` | `true` | 所有物品提示功能的总开关 |
| `TooltipTierColors` | `true` | 按制作层级给词缀着色 |
| `TooltipRankColors` | `true` | 按浮动品质给评级字母着色 |
| `TooltipLayout` | `BadgeLeft` | 标记位置：左侧、右侧或行尾 |
| `SignalStyle` | `PlainText` | 纯文本或彩色徽章 |
| `AffixNameColor` | `GreaterAffix` | 词缀名称颜色模式 |
| `ShowSignal` | `true` | 是否显示层级与评级标记 |
| `ShowGradeLetters` | `true` | 是否显示 S/A/B/C/F |
| `AlwaysShowRanges` | `false` | 是否永久显示数值范围 |
| `AlwaysShowTierDetails` | `false` | 是否永久显示层级详情 |
| `GroundLabelStyle` | `TierAndRank` | 地面标签显示内容 |
| `GroundLabelFilterOnly` | `false` | 只给过滤器物品显示地面标签 |
| `GroundLabelAltKey` | `false` | 按住 Alt 才显示地面标签 |
| `ShowFilterRuleNumber` | `NumberOnly` | 过滤器编号显示模式 |
| `LabelRulePosition` | `EHGDefault` | 地面规则编号位置 |
| `DebugLog` | `false` | 输出详细调试日志 |
| `GreaterAffixTint` | `#C990FF` | T6/T7 高阶词缀颜色 |
| `UnitBorder` | `true` | 是否绘制层级评级边框 |
| `TierWord` | `Spelled` | 显示“层级 7”或缩写“T7” |
| `UnitSeparator` | `Bar` | 使用竖线或圆点分隔 |
| `DividerStyle` | `Strip` | 色条或字符分隔 |
| `BorderColorMode` | `Neutral` | 边框使用中性色或层级颜色 |
| `BorderThickness` | `2` | 边框粗细，1 到 4 |
| `BorderPadX` | `6` | 横向内边距，2 到 12 |
| `BorderPadY` | `2` | 纵向内边距，0 到 6 |
| `BorderDebug` | `false` | 显示边框调试信息 |

## 层级颜色

| 层级 | 颜色 |
|---|---|
| T1 | 灰色 |
| T2 | 浅灰色 |
| T3 | 绿色 |
| T4 | 蓝色 |
| T5 | 紫色 |
| T6 | 橙金色 |
| T7 | 神话粉色 |

## 评级含义

| 字母 | 含义 |
|---|---|
| F | 浮动范围最低端 |
| C | 低于平均 |
| B | 平均 |
| A | 高于平均 |
| S | 接近完美 |

## 兼容性

- 可独立使用，不依赖 KG 模组。
- 与 LeHud 共存，写入文本时会保留其物品稀有度颜色。
- 与 Fallen Star 的 Improved Tooltips 兼容。
- 每个功能独立打补丁，单个游戏方法变化不会导致整个模组失效。
- 同时兼容英文与中文“层级/范围”文本。

## 安装

把 `medick_Terrible_Tooltips.dll` 放入 `Last Epoch\Mods\`。

需要 MelonLoader 0.7.3 或更高版本。

## 彩蛋：去亚伦家

设置面板底部的 **去亚伦家** 按钮会把你传送到集市。

如果按钮无效，请先打开世界地图并点击一次 **神圣纪元**，关闭地图后再按按钮。

## 构建

```powershell
$env:LastEpochPath = "E:\SteamLibrary\steamapps\common\Last Epoch"
dotnet build .\medick_TerribleTooltips\medick_Terrible_Tooltips.csproj -c Release
```

## 致谢

原始灵感来自 **KillingGodVH / war3i4i** 的 [LastEpochImprovements](https://github.com/war3i4i/LastEpochImprovements)。地面标签逻辑、设置注入和多项核心模式均参考并改编自该项目。

