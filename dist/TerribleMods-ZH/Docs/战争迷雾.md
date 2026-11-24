# MedicK 战争迷雾 v1.0.2

适用于 **Last Epoch** 的 [MelonLoader](https://melonwiki.xyz) 地图视野控制模组，在游戏原生设置面板中加入六级视野选择。

> 内部名称：`medick_The_fogOFwar`  
> DLL：`medick_The_fogOFwar.dll`

## 视野等级

| 等级 | 名称 | 效果 |
|---|---|---|
| 0 | 失明 | 隐藏小地图 |
| 1 | 困难 | 小地图可见，但不揭示新区域 |
| 2 | 受限 | 默认揭示范围的 69% |
| 3 | 正常 | 游戏默认视野 |
| 4 | 侦察 | 默认揭示范围的 3 倍 |
| 5 | 神谕 | 揭示半径 600，覆盖整个区域 |

## 使用位置

打开游戏 **设置**，找到 **战争迷雾** 分类。每一行都是可点击的等级选择项，点击后立即生效，当前等级保持选中。

所有等级切换都会即时应用。进入“失明”会隐藏小地图，离开后立即恢复。已经探索的区域通常要进入下一个区域后才会重新计算。

## 配置

如果设置面板不可用，可编辑：

```text
UserData\medick_The_fogOFwar.cfg
```

有效值：

```text
BLIND
HARD
LIMITED
NORMAL
SCOUT
ORACLE
```

## 安装

1. 安装 MelonLoader 0.7.3 或更高版本。
2. 把 `medick_The_fogOFwar.dll` 放入 `Last Epoch\Mods\`。
3. 打开游戏设置，选择“战争迷雾”。

## 注意

视野模组会改变可见范围，建议在离线模式使用。

## 构建

```powershell
$env:LastEpochPath = "E:\SteamLibrary\steamapps\common\Last Epoch"
dotnet build .\medick_FogOfWar\medick_FogOfWar.csproj -c Release
```

## 致谢

设置界面注入思路来自 **KG / war3i4i** 的 [LastEpochImprovements](https://github.com/war3i4i/LastEpochImprovements)。

