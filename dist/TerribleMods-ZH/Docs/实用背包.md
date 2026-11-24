# MedicK 实用背包 v2.0.3

适用于 **Last Epoch** 的 [MelonLoader](https://melonwiki.xyz) 背包增强模组。它把仓库、商人、批量存放和快速传送集中到背包面板。

> Nexus Mods：[Terrible Inventory](https://www.nexusmods.com/lastepoch/mods/29)  
> 内部名称：`medick_Terrible_Inventory`  
> DLL：`medick_Terrible_Inventory.dll`

## 功能

- **仓库**：在任意位置打开仓库。
- **全部存入**：把背包物品按服务器较友好的节奏存入仓库，并遵循词缀仓库页。
- **商人**：在任意位置打开 NPC 商人，默认隐藏。
- **快速传送**：
  - 阵营：命运之环、商人公会、遗忘骑士、编织者。
  - 地下城：无光密林、时空圣所、魂火堡垒。
  - 枢纽：时间尽头、冠军之门。
  - 只会前往角色已经解锁的传送点。
- **原生外观**：按钮、标题和悬停行为均克隆游戏自身 UI。
- **手柄与 Steam Deck**：手柄模式下按钮保持可见。
- **可配置可见项**：仓库、全部存入、商人和快速传送菜单可独立开关。

## 操作

- 所有功能位于背包面板。
- 点击“快速传送”可折叠菜单。
- 点击“阵营 / 地下城 / 枢纽”可折叠对应分组。

## 安装

1. 安装 MelonLoader 0.7.3 或更高版本。
2. 把 `medick_Terrible_Inventory.dll` 放入 `Last Epoch\Mods\`。
3. 打开游戏背包。

如果之前使用旧版 Advanced Inventory，请先删除 `medick_Advanced_Inventory.dll`，避免出现重复按钮。

## 注意

仓库、商人和快速传送更适合离线模式。线上模式可能受服务器校验影响。

## 构建

在仓库根目录执行：

```powershell
$env:LastEpochPath = "E:\SteamLibrary\steamapps\common\Last Epoch"
dotnet build .\medick_Advanced_Inventory\medick_Advanced_Inventory.csproj -c Release
```

使用 `-p:DeployToMods=true` 可在构建后直接复制到游戏的 `Mods` 目录。

## 源码结构

```text
src/
  TerribleInventoryMod.cs          MelonMod 生命周期
  TerribleInventoryMod.Patches.cs  背包与设置面板补丁
  InventoryUi.cs                   背包底栏按钮与全部存入
  TeleportMenu.cs                  快速传送菜单
  TravelService.cs                 解锁检查、传送点查找和预激活
  NativeClone.cs                   原生 UI 克隆
  SettingsUi.cs                    设置分类
  NativeSettings.cs                原生设置注入
  BuildInfo.cs / Prefs.cs          版本信息与配置
```

## 致谢

设置界面注入思路来自 **war3i4i / KillingGodVH** 的 LastEpochImprovements。

