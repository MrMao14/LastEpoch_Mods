# LastEpoch_CosmeticUnlock

从 `LastEpoch_Hud` 中提取的独立全外观解锁插件，不依赖原 HUD 项目代码。

## 功能

- 解锁游戏已加载的全部 Cosmetics。
- 通过 Harmony 拦截 `CosmeticsManager.GetOwnedCosmetics()`。
- 隐藏游戏底部的 `Cosmetics` 入口，避免当前版本外观面板报错。

> 该插件只修改客户端运行时的外观拥有列表，不修改存档或服务器账号数据。

## 构建环境

- Windows
- .NET 6 SDK
- 已安装 MelonLoader 的 Last Epoch
- 环境变量 `LastEpochPath` 指向 Last Epoch 游戏根目录，例如：

```text
D:\SteamLibrary\steamapps\common\Last Epoch
```

## 构建

在仓库根目录执行：

```powershell
$env:LastEpochPath = "D:\SteamLibrary\steamapps\common\Last Epoch"
dotnet build .\LastEpoch_CosmeticUnlock\LastEpoch_CosmeticUnlock.csproj -c Release
```

也可以不设置环境变量，直接通过参数传入游戏路径：

```powershell
dotnet build .\LastEpoch_CosmeticUnlock\LastEpoch_CosmeticUnlock.csproj -c Release /p:LastEpochPath="D:\SteamLibrary\steamapps\common\Last Epoch"
```

## 构建产物

```text
LastEpoch_CosmeticUnlock\bin\Release\net6.0\LastEpoch_CosmeticUnlock.dll
```

项目已将游戏程序集和 Harmony 配置为非复制依赖，构建后只需要部署插件自身生成的：

```text
LastEpoch_CosmeticUnlock.dll
```

## 安装

将以下文件复制到 Last Epoch 的 `Mods` 目录：

```text
LastEpoch_CosmeticUnlock.dll
```

目录结构示例：

```text
Last Epoch\
└─ Mods\
   └─ LastEpoch_CosmeticUnlock.dll
```

## 说明

- `LastEpochPath` 必须指向包含 `MelonLoader` 和游戏程序集的游戏根目录。
- 如果游戏程序集路径发生变化，请同步修改 `LastEpoch_CosmeticUnlock.csproj` 中的 `HintPath`。
- 该功能面向离线/客户端环境，线上模式是否受服务器二次校验影响不在插件保障范围内。
