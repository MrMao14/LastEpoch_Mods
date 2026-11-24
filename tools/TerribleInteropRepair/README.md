# Terrible 内置 CoreModule 修复

`CoreModuleRepair.cs` 和 `EmbeddedRepairBootstrap.cs` 会直接编译进各个 Terrible 模组程序集，因此发布包中仍然只有一个模组 DLL。
`Mono.Cecil` 从 MelonLoader 现有的 `net6` 运行时加载，不会随模组打包。

模块初始化器会在 MelonLoader 发现模组时运行：此时游戏程序集已经生成，但依赖尚未加载。第一个加载的 Terrible 模组会规范化 `CoreModule` 元数据；其他模组读取 SHA256 回执后跳过未变化的文件。游戏重新生成程序集后，修复会再次执行。整个过程不会删除、重命名或合并类型。

修复工具会在 `Il2CppAssemblies` 中创建带原始哈希的备份和回执。替换前会逐项比较类型与成员签名，最后以原子方式写入文件。该过程不联网，也不包含遥测。

已在真实 Last Epoch 启动环境中验证：恢复原始有问题的 `CoreModule` 且不安装其他插件时，内置修复能够运行，四个模组均可初始化，物品提示的 9 个补丁全部加载。该验证不代表所有玩法功能都已覆盖。

本目录中的 `.csproj` 是测试库；测试项目不会编译 `EmbeddedRepairBootstrap.cs`，以免测试过程修改真实游戏安装。各模组项目会链接这两个源码文件，发布包中不包含独立修复 DLL。

运行测试：

```powershell
dotnet run --project tools\TerribleInteropRepair\tests -- <原始CoreModule路径> <临时目录>
```

如需撤销规范化：

1. 关闭游戏。
2. 删除更新后的模组 DLL。
3. 把匹配的 `.terrible-original-*.bak` 恢复为 `CoreModule`。
4. 删除对应的修复回执。
