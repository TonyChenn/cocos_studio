# Mono.TextEditor 恢复设计

更新日期：2026-09-07。本文件是下一批源码恢复的实施边界，不表示当前已经切换到源码构建。

## 固定基准与审计结果

- 当前文件：`dlls/Mono.TextEditor.dll`
- 程序集身份：`Mono.TextEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null`
- SHA-256：`47DAA700220831CD86F0135E8CC57B920D4D8BF76AB734D781DE6BC27D0800B6`
- Git Blob：`460ef7fe657cf8ef2ba22b59aa2d91619dc4a955`
- 固定历史：`632862e2e3dc6485fadc3f30d4454f97a9187c2d`

固定提交、当前 HEAD 和工作区文件的 Blob 一致。`tests/RestoredBaseline.ps1` 已把该文件纳入统一基准，仍采用 Git 二进制流提取并校验 SHA-256；外部基准目录同样必须通过哈希校验。正式构建不得从 `obj/RestoredBaseline` 解析依赖。

`tests/Audit-MonoTextEditor.ps1` 只做静态读取，报告写入被忽略的 `obj/MonoTextEditorAudit/audit.json`。当前结果为 421 个类型、248 个公开或受保护类型、3397 个公开或受保护成员定义、39 项嵌入资源、4 个可序列化类型、12 个原生模块名。成员统计不把属性和事件访问器重复计数。

反编译候选位于被忽略的 `obj/MonoTextEditorAudit/original`，由 ILSpy `11.0.0.9375` 以项目模式、C# 7.3 导出，共 228 个 C# 文件。反编译无法恢复原始注释、提交历史、工程条件和授权来源，因此该目录只是审计输入，不是可直接提交的源码来源。

[MonoDevelop 5.4 固定提交中的 Mono.Texteditor 源码目录](https://github.com/mono/monodevelop/tree/01786bc67c7024ec33d327ed27e4416d7a846f4e/main/src/core/Mono.Texteditor)可作为结构和许可对照，但没有证据证明当前 DLL 与它逐字节或逐方法一致，不能直接覆盖本地版本。

## 依赖边界

当前源码中有 8 个直接引用项目：

- `src/Editor/CocoStudio.LuaBinding/CocoStudio.LuaBinding.Restored.csproj`
- `src/Editor/CocoStudio.SourceEditor/CocoStudio.SourceEditor.Restored.csproj`
- `src/Framework/CocoStudio.Gtk.Extend/CocoStudio.Gtk.Extend.csproj`
- `src/Modules/Communal/Modules.Communal.Render/Modules.Communal.Render.csproj`
- `third-party/MonoDevelop/MonoDevelop.Debugger/MonoDevelop.Debugger.Restored.csproj`
- `third-party/MonoDevelop/MonoDevelop.DesignerSupport/MonoDevelop.DesignerSupport.Restored.csproj`
- `third-party/MonoDevelop/MonoDevelop.Refactoring/MonoDevelop.Refactoring.Restored.csproj`
- `third-party/MonoDevelop/MonoDevelop.SourceEditor2/MonoDevelop.SourceEditor2.Restored.csproj`

迁移前 `bin/Debug` 快照中的 9 个消费者为 CocoStudio.Gtk.Extend、CocoStudio.LuaBinding、CocoStudio.SourceEditor、Modules.Communal.Render、MonoDevelop.Debugger、MonoDevelop.DesignerSupport、MonoDevelop.Ide、MonoDevelop.Refactoring、MonoDevelop.SourceEditor2。它们只说明已有二进制调用关系，不证明目录迁移后的构建状态。

原库依赖 GTK# 2.12、强名称 NRefactory 5.0、Xwt 0.1、Mono.Posix 2.0，并同时引用 Mono.Cairo 2.0 与 4.0。恢复时保持这些程序集身份和现有目标框架，不在同批升级 GTK、NRefactory、Xwt、Cairo 或 Posix，也不以绑定重定向隐藏身份差异。

正式源码目录固定为 `third-party/MonoDevelop/Mono.TextEditor/`。接入顺序为：先建立独立项目并复现程序集身份、API 和资源，再将上述 8 个直接引用逐项改为 ProjectReference，最后通过主方案的间接依赖覆盖 MonoDevelop.Ide。Mono.TextEditor 不得反向引用 SourceEditor2、Debugger、DesignerSupport、Refactoring 或 CocoStudio 应用项目，以免形成循环。

Core、Ide 继续使用现有 DLL；本批不扩展到它们。SourceEditor2 已存在 Cocos 定制入口，包括对 `CocoStudio.SourceEditor` 的友元授权，以及 `ProcessSaveText`、`ProcessLoadText`、`PrepareToSetCaret`。后续替换 Mono.TextEditor 时必须让这些调用链保持原行为，不能因采用上游代码而覆盖本地定制。

## API、资源与平台约束

后续候选必须与原库比较完整程序集表面，而不是只检查能否编译：类型、方法、字段、属性、事件、可见性、程序集属性、友元程序集和序列化标记均须一致。原库友元程序集为 `MonoDevelop.TextEditor.Tests`；4 个序列化类型及其字段布局须单独核对。

39 项资源的逻辑名称和字节必须保持一致，包括 `gui.stetic`、30 份语法 XML 和 8 份颜色样式 JSON。Lua、C#、JSON、XML 等语法资源不能只按文件数比较。候选还要保留 12 个 Windows/Linux/macOS 原生模块调用名称；Windows 验证不能替代 GTK Quartz 或 libc 路径的跨平台结论。

Debugger、SourceEditor2、DesignerSupport、Refactoring 和 ProjectSetting 的 `gui.stetic` 共含 5 处旧 `build/bin/Mono.TextEditor.dll` 路径。静态追踪没有发现业务 C# 在运行时读取这些路径，现阶段将其视为 Stetic 设计时元数据候选，但在未做设计器和运行时验证前不能下最终结论。为维持资源字节对照，本批不改写；如后续证明流程会解析它们，再在源码接入提交中改为与仓库根无关的统一引用方式。

## 后续验证与切换规则

获得编译测试授权后，在没有旧输出可复用的独立工作副本执行 Debug/x86 与 Release/x86 的 NuGet 还原和冷构建，并同时覆盖 `.sln`、`.slnx`。候选至少需要完成：

1. 程序集身份、完整 API、程序集属性、资源名称与资源字节对照。
2. 文本载入/保存、BOM、编码、换行、撤销重做、选区、块选择和光标移动。
3. 搜索替换、语法高亮、折叠、标记、补全、滚动、测量和布局。
4. SourceEditor2、CocoStudio.SourceEditor、LuaBinding、Debugger、DesignerSupport、Refactoring 和 Render 集成回归。
5. 编辑器打开/保存、补全弹窗、断点标记、属性面板及 Stetic 设计器的用户交互验收。

只有上述冷构建、静态对照、自动回归和交互验收全部通过，才把正式引用切到 `third-party/MonoDevelop/Mono.TextEditor/`，并在同一批删除 `dlls/Mono.TextEditor.dll`、更新文档后提交。失败时回退完整提交，不保留源码/DLL 双轨正式构建，也不从测试基准目录兜底。

当前仅完成基准和静态审计：没有 NuGet 还原、编译、运行时或交互行为证明。
