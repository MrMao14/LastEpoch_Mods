# ModUI 设置框架

这是 Last Epoch HUD 使用的声明式设置框架。

## 添加设置

在 `ModSettings.cs` 中写一行即可：

```csharp
public static readonly BoolSetting ForceSeal = Group.Bool("ForceSeal");
```

框架会自动处理：

- 序列化到 `SaveModUI.json`。
- 绑定共享 `hud.prefab` 中的开关。
- 维护脏标记并延迟自动保存。

## 所有工厂方法

```csharp
Group.Bool("ForceSeal")                                             // 开关
Group.Bool("LoadOnStart", defaultValue: true)                       // 开关，默认开启
Group.Float("ZoomMinimum")                                          // 开关 + 滑块
Group.Float("MobsDensity", format: DisplayFormat.Percent)           // 开关 + 滑块，百分比显示
Group.Range("Implicits", 0, 255, format: DisplayFormat.Percent)     // 开关 + 最小/最大滑块
Group.Range("AffixTiers", 0, 6, format: DisplayFormat.Tier)         // 层级从 1 开始显示
Group.Range("AffixCount", 0, 6, defaultMax: 4, panel: "NbAffixes") // 自定义默认值和面板名
Group.Dropdown("Type")                                              // 下拉框，选项运行时设置
Group.Dropdown("Monolith", options: new[] { "Select", "Outcast" })  // 静态下拉框
Group.Radio("ForceRarity", "Force", "ForceUnique", "ForceSet", "ForceLegendary") // 互斥开关
Group.Button("Reset")                                               // 可点击操作，不保存状态
Group.Header("Header", label: "AutoCast")                           // 静态本地化分组标题
Group.Keybind("ModifierKey", defaultBinding: "kb:LeftControl", label: "Modifier Key")  // 可重绑定输入
```

如果预制体面板名称与设置键不同，可指定 `panel:`：

```csharp
public static readonly RangeSetting AffixCount =
    Group.Range("AffixCount", 0, 6, panel: "NbAffixes");
```

范围设置的默认值通常使用 `minLimit` 和 `maxLimit`。可通过 `defaultMin:` 和 `defaultMax:` 覆盖。

## 标签与本地化

每个工厂方法都接受可选的 `label:` 参数，值为英文显示文本。绑定时框架会在 `Locales.current_dictionary` 中查找翻译，并写入控件的 `Label` 文本。找不到翻译时保留英文。

如果 `label:` 为 `null`，预制体中已有的文本不会被修改。

```csharp
public static readonly FloatSetting TreePoints =
    Group.Float("TreePoints", defaultValue: 50f, label: "Weaver Tree Points");
```

`LocaleRegistry` 会记录所有已应用的 `(Text, English label)` 组合。切换语言时会从标准英文键重新翻译，因此可以在英语、法语和中文之间来回切换。

### 添加本地化标签

1. 声明设置时添加 `label: "English Label"`。
2. 在 `LastEpoch_Hud/Locales/base.json` 中加入该键，值可为空。
3. 在 `en.json`、`fr.json` 和 `zh.json` 中补充翻译。
4. 重新构建 DLL。

标签保存在代码中，不需要重建 AssetBundle。

## 在功能代码中读取设置

```csharp
using LastEpoch_Hud.Scripts.ModUI;

// 直接读取
if (ModSettings.ItemsDrop.ForceSeal.Value) { /* ... */ }
float min = ModSettings.ItemsDrop.Implicits.Min;
bool enabled = ModSettings.ItemsDrop.Implicits.Enabled;

// 单选：检查选中项
if (ModSettings.ItemsDrop.ForceRarity.IsSelected(0)) { /* ForceUnique */ }

// 响应变化
ModSettings.ItemsDrop.ForceRarity.Changed += () => RecalculateDropTable();

// 响应按钮点击
ModSettings.ScenesCamera.Reset.Clicked += () => ResetCameraValues();
```

设置属性为 `{ get; internal set; }`。功能代码可以直接读取，但写入必须使用 `.Set()`、`.SetMin()`、`.SetMax()` 或 `.SetEnabled()`，这样才会触发脏标记和 `Changed` 事件。

## 创建新分组

### 使用独立标签页

```csharp
public static class ScenesCamera
{
    public static readonly SettingsGroup Group = new SettingsGroup("ScenesCamera")
        .Tab("Scenes", "Btn_Menu_Scenes")                 // 创建 HUD 标签页
        .Content("Scenes_Content")                         // 预制体内容面板
        .Viewport("Camera", "Scenes_Camera_Content")       // 可滚动区域
        .Prefix("Scenes_Camera_");                         // UI 元素查找前缀

    public static readonly FloatSetting ZoomMinimum = Group.Float("ZoomMinimum");
    public static readonly ActionBinding Reset      = Group.Button("Reset");
}
```

### 绑定自定义预制体

先把预制体注册为命名根节点，再用 `.Root("name")` 声明分组，并通过绝对路径查找控件。

```csharp
// 功能代码中，实例化预制体后执行：
SaveManager.BindRoot("PortalPrompt", portalPromptObj);

// ModSettings.cs：
public static class PortalPromptUI
{
    public static readonly SettingsGroup Group =
        new SettingsGroup("PortalPrompt").Root("PortalPrompt");

    public static readonly HeaderSetting Header =
        Group.Header("Header", label: "Quick Portal", path: "Title/Header");

    public static readonly DropdownSetting Destination =
        Group.Dropdown("Destination", path: "Panel/DestinationDropdown", label: "Destination");

    public static readonly BoolSetting Include =
        Group.Bool("Include", path: "Panel/IncludeUncompleted", label: "Include Uncompleted");

    public static readonly ActionBinding Portal =
        Group.Button("Portal", path: "Panel/PortalButton", label: "Portal");
}

PortalPromptUI.Portal.Clicked +=
    () => TeleportTo(PortalPromptUI.Destination.SelectedText);
```

`path:` 从注册根节点开始计算，因此一个分组可以覆盖多个兄弟子树。设置会按分组名称持久化到 `SaveModUI.json`。重新实例化 GameObject 后再次调用 `BindRoot` 即可，也支持 Canvas 根节点。

复合设置（`Float`、`Range`、`Keybind`、`Radio`）可一次绑定多个控件：

```csharp
Group.Float("Damage", paths: new FloatPaths(slider: "Settings/Damage/Slider"))
Group.Float("Damage", paths: new FloatPaths(
    slider: "Settings/Damage/Slider",
    toggle: "Settings/Damage/Enable"))

Group.Range("Tier", paths: new RangePaths(
    minSlider: "Tier/Min",
    maxSlider: "Tier/Max",
    toggle: "Tier/Enable"))

Group.Keybind("Modifier", paths: new KeybindPaths(
    captureButton: "Mod/Capture",
    resetButton: "Mod/Reset",
    label: "Mod/Label"))

Group.RadioWithPaths("Rarity",
    new RadioPaths("Rarity/Common", "Rarity/Magic", "Rarity/Rare", "Rarity/Unique"),
    "Common", "Magic", "Rare", "Unique")
```

必需子路径通过构造函数传入。可选路径默认为 `null`。如果滑块不需要开关，可以省略 Float/Range 的 toggle；如果绑定不需要恢复默认按钮，可以省略 Keybind 的 resetButton。Radio 开关数量必须与选项数量一致。

### 不使用独立标签页

子面板可以共享另一个分组的内容区域：

```csharp
public static class ItemsPickup
{
    public static readonly SettingsGroup Group = new SettingsGroup("ItemsPickup")
        .Content("Items_Content")                    // 与 ItemsDrop 共享内容
        .Viewport("Items_Pickup", "Items_Pickup_Content")
        .Prefix("Items_Pickup_");

    public static readonly BoolSetting AutoPickupGold =
        Group.Bool("AutoPickupGold", panel: "AutoPickup_Gold");
}
```

调用 `.Tab()` 的分组会创建菜单按钮并拥有独立内容面板。没有 `.Tab()` 的分组会在 HUD 初始化时绑定为子面板。

## OnBind：处理非标准 UI

当控件不符合命名约定时，可使用 `OnBind`：

```csharp
.OnBind((contentObj, viewportContent) =>
{
    var toggle = Prefab.ToggleInTitle(
        contentObj, "Camera", "Toggle_Scenes_Camera_Enable");

    if (toggle == null) return;
    toggle.isOn = EnableMod.Value;
    Prefab.BindToggle(toggle, new Action<bool>(v => EnableMod.Set(v)));
})
```

此回调在所有自动绑定完成后执行。应尽量少用；如果标准开关或按钮也需要它，通常说明命名约定配置有误。

## 下拉框选项

下拉框支持编译期已知的静态选项，也支持运行时从游戏数据生成的动态选项。

### 静态选项

```csharp
public static readonly DropdownSetting Monolith = Group.Dropdown(
    "Monolith",
    options: new[] { "Select", "Fall_Of_The_Outcast", "The_Stolen_Lance" });
```

### 动态选项

```csharp
public static readonly DropdownSetting Type = Group.Dropdown("Type");

// 游戏数据加载后：
var names = ItemList.get().EquippableItems.Select(i => i.BaseTypeName).ToArray();
ModSettings.ItemsForceDrop.Type.SetOptions(names);
```

### 级联下拉框

```csharp
ModSettings.ItemsForceDrop.Type.Changed += _ =>
{
    var rarities = GetRaritiesForType(
        ModSettings.ItemsForceDrop.Type.SelectedText);
    ModSettings.ItemsForceDrop.Rarity.SetOptions(rarities);
};
```

`SetOptions` 会把选择重置为索引 0，但不会触发 `Changed` 或标记脏状态，因为这是 UI 重置，不是玩家操作。

`.SelectedText` 返回当前选项文本；没有选项时返回 `null`。

## 命名约定

`SettingsBuilder` 使用 `Prefix + key` 查找 UI：

| 控件 | 名称格式 | 示例（前缀 `Items_Drop_`） |
|---|---|---|
| 开关 | `Toggle_{prefix}{key}` | `Toggle_Items_Drop_ForceSeal` |
| 滑块 | `Slider_{prefix}{key}` | `Slider_Items_Drop_Implicits_Min` |
| 下拉框 | `Dropdown_{prefix}{key}` | `Dropdown_Items_ForceDrop_Type` |
| 按钮 | `Btn_{prefix}{key}` | `Btn_Scenes_Camera_Reset` |

UI 控件位于以设置键命名的子面板中，也可以通过 `panel:` 覆盖。按钮是视口内容的直接子节点。

文本标签会先查找开关内的 `Value` 或 `Label` 子节点，再查找面板的直接子节点。

## 参考

### 设置类型

| 类型 | 存储 | UI | 读取 | 写入 |
|---|---|---|---|---|
| `BoolSetting` | bool | 开关 | `.Value` | `.Set(bool)` |
| `FloatSetting` | bool + float | 开关 + 滑块 | `.Enabled`、`.Value` | `.SetEnabled(bool)`、`.SetValue(float)` |
| `RangeSetting` | bool + min/max | 开关 + 两个滑块 | `.Enabled`、`.Min`、`.Max` | `.SetEnabled(bool)`、`.SetMin(float)`、`.SetMax(float)` |
| `DropdownSetting` | int + 标签 | 下拉框 | `.Value`、`.SelectedText` | `.Set(int)`、`.SetOptions(string[])` |
| `RadioSetting` | N 个 bool | N 个开关 | `.IsSelected(i)` | `.Select(i, bool)` |
| `ActionBinding` | 不保存 | 按钮 | 无 | 订阅 `.Clicked` |
| `HeaderSetting` | 不保存 | 静态文本行 | `.Text` | `.SetText(string)` |
| `KeybindSetting` | 带标签字符串 | 捕获按钮 + 重置按钮 | `.Value`、`.DefaultValue` | `.Set(string)`、`.ResetToDefault()` |

除 `ActionBinding` 和 `HeaderSetting` 外，所有设置都提供 `Changed` 事件。

## 按键绑定

`KeybindSetting` 可捕获一个键盘按键或手柄按钮。点击捕获后按任意键，第一个输入生效，`Escape` 取消。重置按钮恢复默认绑定。

```csharp
public static readonly KeybindSetting ModifierKey =
    Group.Keybind(
        "ModifierKey",
        defaultBinding: "kb:LeftControl",
        label: "Modifier Key");

if (KeybindMatcher.IsHeld(ModSettings.SkillsAutoCast.ModifierKey.Value))
{
    // ...
}
```

### 绑定格式

- `"kb:LeftControl"`：Unity `KeyCode` 名称。
- `"gp:A"`：手柄模板名称。

支持的手柄名称包括：

`A`、`B`、`X`、`Y`、`LB`、`RB`、`Back`、`Start`、`Guide`、`LStick`、`RStick`、`DPadUp`、`DPadDown`、`DPadLeft`、`DPadRight`。

`resetLabel:` 可覆盖单个设置使用的“重置”文本。

### 标准预制体布局

```text
{panelName}/
  Label                            -- 设置标题
  Btn_{prefix}{panelName}          -- 捕获按钮，子文本显示当前绑定
  Btn_{prefix}{panelName}_Reset    -- 重置按钮，子文本通常为“重置”
```

任意层级预制体应使用 `paths: new KeybindPaths(...)`。

### 显示格式

| 格式 | 示例 | 用途 |
|---|---|---|
| `Raw` | `50` | 整数 |
| `Tier` | `3` | 从 1 开始显示的层级 |
| `Percent` | `75 %` | 百分比，最大值不是 100 时自动缩放 |
| `Seconds` | `5 sec` | 时间间隔 |

### SettingsGroup 流式 API

```csharp
new SettingsGroup("Name")           // JSON 键，自动注册保存
    .Root("Hud")                    // 绑定根节点，默认 Hud
    .Tab("TabId", "Btn_Menu_Name")  // 创建 HUD 标签页
    .Content("Panel_Name")          // 预制体内容面板
    .Viewport("Panel", "Content")   // 可滚动视口
    .Prefix("Ui_Prefix_")           // 控件名称前缀
    .OnBind((content, viewport) => { }) // 非标准元素扩展入口
```

### 绑定模式

| 模式 | 使用场景 | 写法 |
|---|---|---|
| **命名约定** | HUD 行遵循 `Toggle_{prefix}{key}` 等命名 | `Group.Bool("MyToggle")` |
| **路径** | 任意层级的自定义预制体 | `Group.Bool("MyToggle", path: "Panel/Settings/MyToggle")` |
| **OnBind** | 标题栏主开关等一次性特殊元素 | `.OnBind((content, vp) => { ... })` |

### 文件结构

```text
ModUI/
  ModSettings.cs         主要编辑文件，声明全部设置、标签页和面板
  Internal/              框架内部实现
    SettingTypes.cs      设置类型、ActionBinding、显示格式
    SettingsGroup.cs     流式 API、工厂方法、序列化和 UI 绑定
    SettingsBuilder.cs   命名约定与路径绑定
    TabManager.cs        标签切换和菜单按钮
    Prefab.cs            空安全子节点和路径查找
    SliderHook.cs        IL2CPP 滑块事件补丁
    SaveManager.cs       SaveModUI.json 持久化
    BindingRoots.cs      根节点注册表
    LocaleRegistry.cs    本地化标签往返记录
  Keybind/               按键绑定辅助代码
    KeybindStrings.cs    集中本地化文本
    KeybindFormat.cs     绑定显示格式
    KeybindRewired.cs    Rewired 手柄按钮表
    KeybindCapture.cs    捕获状态机
    KeybindMatcher.cs    IsHeld 查询 API
```

### 速查表

| 目标 | 做法 |
|---|---|
| 添加开关、滑块或范围 | 在 `ModSettings.cs` 中使用 `Group.Bool/Float/Range(...)` |
| 添加下拉框 | 使用 `Group.Dropdown("Key")`，可附带 `options:` |
| 运行时填充下拉框 | 游戏数据加载后调用 `.SetOptions(string[])` |
| 添加操作按钮 | 使用 `Group.Button("Key")` 并订阅 `.Clicked` |
| 添加新标签页 | 新建带 `.Tab()` 的 SettingsGroup |
| 添加子面板 | 新建只带 `.Content()` 的 SettingsGroup |
| 读取设置 | `ModSettings.Section.Setting.Value` |
| 响应设置变化 | 订阅 `.Changed` |
| 响应按钮点击 | 订阅 `.Clicked` |
| 绑定非标准元素 | 使用 `.OnBind()` |
| 覆盖预制体面板名 | 工厂方法传入 `panel:` |
| 添加新显示格式 | 修改 `Internal/SettingTypes.cs` |
| 添加新设置类型 | 修改 `SettingTypes.cs`、`SettingsGroup.cs` 和 `SettingsBuilder.cs` |
| 添加分组标题 | `Group.Header("Header", label: "Section Name")` |
| 添加可重绑定输入 | `Group.Keybind("Key", defaultBinding: "kb:LeftControl", label: "Modifier")` |
| 读取按键状态 | `KeybindMatcher.IsHeld(ModSettings.X.Y.Value)` |
| 绑定自定义预制体 | 先 `SaveManager.BindRoot(...)`，再 `Group.Root(...)` |
| 按路径绑定控件 | `Group.Bool("Key", path: "Panel/Settings/Toggle")` |
