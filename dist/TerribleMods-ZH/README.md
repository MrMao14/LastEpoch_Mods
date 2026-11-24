# MedicK 模组中文整合包

本目录已从 [medick51o/LastEpoch-Mods](https://github.com/medick51o/LastEpoch-Mods) 导入并完成界面汉化。

## 已导入模组

| 目录 | DLL | 功能 |
|---|---|---|
| `medick_Advanced_Inventory` | `medick_Terrible_Inventory.dll` | 仓库、全部存入、商人和快速传送 |
| `medick_CameraZoom` | `medick_CameraZoom.dll` | 镜头缩放、倾角锁定和恢复默认 |
| `medick_CooldownTracker` | `medick_CooldownTracker.dll` | 角色头顶技能冷却图标 |
| `medick_FogOfWar` | `medick_The_fogOFwar.dll` | 六级战争迷雾/视野范围 |
| `medick_RighteousFire` | `medick_RighteousFire.dll` | 新增“正义之火”腰带 |
| `medick_TerribleTooltips` | `medick_Terrible_Tooltips.dll` | 物品词缀层级、评级、地面标签和过滤器规则编号 |
| `medick_TheNexusButton` | `medick_A_Terrible_Button.dll` | 打开游戏原生枢纽面板的快捷按钮 |

每个模组目录下的 README、CHANGELOG 和相关开发文档均已汉化。

## 构建

默认游戏路径为：

```text
E:\SteamLibrary\steamapps\common\Last Epoch
```

路径变化时设置 `LastEpochPath`，然后构建统一解决方案：

```powershell
$env:LastEpochPath = "E:\SteamLibrary\steamapps\common\Last Epoch"
dotnet build .\TerribleMods.sln -c Release -p:DeployToMods=false
```

默认不会写入游戏的 `Mods` 目录。需要构建后自动部署时，将 `DeployToMods` 设为 `true`：

```powershell
dotnet build .\TerribleMods.sln -c Release -p:DeployToMods=true
```

## 兼容性说明

- `medick_Terrible_Tooltips` 内部的 Melon 名称保持 `Terrible Tooltips`，用于兼容 Fallen Star 模组检测。
- 所有配置键名、枚举名和场景路径保持上游原值，已有配置文件可以直接沿用。
- 不要同时启用功能重复的旧版 Tooltip、Fog 或 `ScreenPro` 类模组。
- 仓库、传送和镜头类功能建议在离线模式使用。

