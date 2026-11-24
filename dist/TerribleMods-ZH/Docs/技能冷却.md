# MedicK 技能冷却 v1.0.2

适用于 **Last Epoch** 的 [MelonLoader](https://melonwiki.xyz) 技能冷却追踪模组。技能冷却时，图标会悬浮在角色头顶。

> Nexus Mods：[Cooldown Tracker](https://www.nexusmods.com/lastepoch/mods/26)  
> 内部名称：`medick_CooldownTracker`  
> DLL：`medick_CooldownTracker.dll`

## 功能

- 从游戏内存中读取技能图标和 UI 图集，不附带额外图片资源。
- 显示冷却遮罩、就绪闪烁边框和按键标签。
- 支持键盘、Xbox 和 PS5 输入模式。
- 可绘制手柄按键徽章。
- 支持拖动调整图标组位置。
- 菜单或登录界面没有存活角色时不绘制图标。
- 设置面板位置自动保存。
- 自动检测输入设备，也可手动覆盖布局。
- 键盘、Xbox 和 PS5 标签可分别保存。
- 支持 Xbox/PS5 按键选择器。
- 可阻止面板打开时的角色移动输入。
- 每个技能槽可独立启用或禁用。
- 支持第 6 个闪避技能槽。

## 操作

| 输入 | 功能 |
|---|---|
| `Home` | 打开或关闭设置面板 |
| 拖动标题栏 | 移动设置面板 |
| 移动 → 拖动图标 → 锁定 | 调整悬浮图标位置 |

## 默认按键标签

| 技能槽 | 0 | 1 | 2 | 3 | 4 | 5 | 6 |
|---|---|---|---|---|---|---|---|
| 键盘 | Q | W | E | R | 右键 | T | 空格 |
| Xbox | X | Y | RB | LT | L | RT | B |
| PS5 | □ | △ | R1 | L2 | L3 | R2 | ○ |

键盘模式下会尽量读取游戏动作栏中的实际按键绑定。

## 安装

1. 安装 MelonLoader 0.7.3 或更高版本。
2. 把 `medick_CooldownTracker.dll` 放入 `Last Epoch\Mods\`。
3. 进入游戏场景。
4. 按 `Home` 打开设置。

旧版本标签和滑块配置可直接沿用。

## 构建

```powershell
$env:LastEpochPath = "E:\SteamLibrary\steamapps\common\Last Epoch"
dotnet build .\medick_CooldownTracker\medick_CooldownTracker.csproj -c Release
```

## 源码结构

```text
src/
  CooldownTrackerMod.cs          生命周期与 GUI 调度
  CooldownTrackerMod.Patches.cs  AbilityBarIcon 补丁
  SlotRegistry.cs                技能槽状态与标签缓存
  InputDetection.cs              输入设备检测与标签解析
  InputBlocker.cs                EpochInputManager 输入阻止
  UI/
    Theme.cs                     主题、纹理与中文字体
    Widgets.cs                   控件
    SettingsPanel.cs             Home 键设置面板
    ButtonPicker.cs              手柄按键选择器
    OverheadRenderer.cs          悬浮图标绘制与拖动
```

