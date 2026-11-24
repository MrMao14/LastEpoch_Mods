# MedicK 枢纽按钮 v0.1.1

适用于 **Last Epoch** 的 [MelonLoader](https://melonwiki.xyz) 快捷按钮模组。

按钮会打开游戏原生旅行菜单，其中包括地图、独石时间线、活动和阵营。外观克隆自游戏自身的“枢纽”按钮，位置在活动标签上方。

> DLL：`medick_A_Terrible_Button.dll`

## 功能

- 一键打开游戏原生枢纽菜单。
- 克隆游戏原生按钮外观。
- 可选 `N` 快捷键，默认关闭。
- 可隐藏按钮，只使用快捷键。
- 可通过设置面板调整横向和纵向位置。
- 可恢复默认位置。
- 支持 16:9、21:9 和游戏 UI 缩放。
- 兼容中文“活动”与英文 `ACTIVITIES` 标签。

## 设置

打开游戏设置中的 **枢纽按钮** 分类，可调整：

- 显示枢纽按钮
- 枢纽快捷键 N
- 横向偏移
- 纵向偏移
- 重置按钮位置

## 安装

1. 安装 MelonLoader 0.7.3 或更高版本。
2. 把 `medick_A_Terrible_Button.dll` 放入 `Last Epoch\Mods\`。

## 版本历史

- **0.1.1**：修复单独安装该模组时出现的 `Duplicate type '<>O'` 加载错误。
- **0.1.0**：首次发布。

## 构建

```powershell
$env:LastEpochPath = "E:\SteamLibrary\steamapps\common\Last Epoch"
dotnet build .\medick_TheNexusButton\medick_TheNexusButton.csproj -c Release
```

