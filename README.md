# Cocos Studio

## 仓库目录

- `src/Applications/`：CocosStudio 主程序、Cocos 启动入口和 Cocos.Update。
- `src/Launcher/`：启动器公共库。
- `src/Framework/`：Core、Basic、控件、工程管理、撤销及公共服务。
- `src/Models/`：Model、Model3D 及对应 Lua 模型。
- `src/Editor/`：CocoStudio.SourceEditor、CocoStudio.LuaBinding。
- `src/Platforms/`：CocoStudio.WindowsPlatform 应用适配层。
- `src/Modules/Communal/`、`src/Modules/UI/`：内置功能模块；目录整理不改变模块发现机制。
- `src/ExternalImport/`：外部 CSD 导入子系统，保留独立解决方案。
- `third-party/`：MonoDevelop 定制恢复源码与既有 NRefactory 源码。目录归类不代表未经修改的上游版本。
- `dlls/`：尚未替换的二进制依赖；`runtime-assets/`：运行时资产。
- `build/`、`tests/`、`docs/`：公共构建、验证脚本与依赖文档。
- 根目录 `bin/`、`obj/`：现有共享输出及中间文件；新生成的文件由忽略规则排除。

部分项目历史上已经跟踪了局部 `bin/obj` 中的 58 个产物文件，本轮只原样迁移，不混入删除清理；
它们不代表当前构建输出，后续应单独核查并清理。

## 入口与验证

主解决方案仍为根目录的 `CocosStudio.sln` 和 `CocosStudio.slnx`，模型子方案为 `CocoStudio.Model.sln`。
项目目录分组不改变程序集名、命名空间、项目 GUID 或共享 Debug/Release 输出位置。
所有文档中未特别说明的路径均相对于仓库根目录。

只检查项目布局、引用和资源路径，不还原、不编译、不运行编辑器：

```powershell
.\tests\Test-RepositoryLayout.ps1
```

源码目录迁移后，首次实际构建需要重新还原包，不能把旧 `obj` 的项目路径缓存当作新布局已验证的证据。
完整构建、回归及交互验收说明见 [恢复库说明](third-party/MonoDevelop/README.md)。

- [依赖替换记录](docs/DEPENDENCIES.md)
- [剩余依赖审计](docs/DEPENDENCY_AUDIT.md)
- [外部导入说明](src/ExternalImport/README.md)
