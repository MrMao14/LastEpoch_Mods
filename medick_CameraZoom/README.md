# MedicK 镜头缩放 v1.0.2

适用于 **Last Epoch** 的 [MelonLoader](https://melonwiki.xyz) 镜头增强模组。

> Nexus Mods：[Camera Zoom](https://www.nexusmods.com/lastepoch/mods/27)  
> 内部名称：`medick_CameraZoom`  
> DLL：`medick_CameraZoom.dll`

## 功能

- 扩展最远缩放范围，默认值从游戏原始范围进一步拉远。
- 调整滚轮灵敏度、镜头平滑速度和实时缩放。
- 将镜头倾角锁定在 20° 至 85°。
- 解锁倾角时恢复游戏当前场景的真实限制。
- 自动阻止非法镜头数值，并在数值异常时恢复游戏默认值。
- 一键恢复全部镜头参数。
- 设置面板位置和缩放比例自动保存。

## 工作原理

镜头状态由 `Il2Cpp.CameraManager` 管理。模组只修改 `zoomMin`、`zoomPerScroll`、`zoomSpeed`、`targetZoom` 和 `cameraAngle*` 等字段，不直接写 `Camera.main.fieldOfView`。

所有写入都会先比较当前值，只有数值确实不同才修改，因此可以和其他镜头模组及场景重置共存。

## 操作

| 输入 | 功能 |
|---|---|
| 鼠标滚轮 | 缩放镜头 |
| `End` | 打开或关闭设置面板 |
| 拖动标题栏 | 移动设置面板，位置自动保存 |

## 安装

1. 安装 MelonLoader 0.7.3 或更高版本。
2. 把 `medick_CameraZoom.dll` 放入 `Last Epoch\Mods\`。
3. 进入游戏场景，滚动滚轮缩放，按 `End` 调整参数。

旧版本设置可直接沿用。

## 注意

镜头模组会改变可见范围，建议在离线模式使用。

## 构建

```powershell
$env:LastEpochPath = "E:\SteamLibrary\steamapps\common\Last Epoch"
dotnet build .\medick_CameraZoom\medick_CameraZoom.csproj -c Release
```

## 源码结构

```text
src/
  CameraZoomMod.cs          MelonMod 生命周期
  CameraZoomMod.Patches.cs  CameraManager.Start 补丁
  CameraState.cs            捕获、应用、清洗和恢复镜头状态
  BuildInfo.cs              名称与版本
  Prefs.cs                  持久化配置
  UI/
    Theme.cs                面板主题与中文字体
    Widgets.cs              滑块、开关和行控件
    SettingsPanel.cs        End 键设置面板
```

