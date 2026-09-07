# Mono.TextEditor 恢复设计

更新日期：2026-09-07。源码、项目引用、独立冷构建、自动回归和用户交互验收均已完成；原 DLL 已删除，正式构建只使用恢复源码。

## 固定基准与审计结果

- 删除前文件：`dlls/Mono.TextEditor.dll`
- 程序集身份：`Mono.TextEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null`
- SHA-256：`47DAA700220831CD86F0135E8CC57B920D4D8BF76AB734D781DE6BC27D0800B6`
- Git Blob：`460ef7fe657cf8ef2ba22b59aa2d91619dc4a955`
- 固定历史：`632862e2e3dc6485fadc3f30d4454f97a9187c2d`

固定提交与删除前工作区文件的 Blob 一致。`tests/RestoredBaseline.ps1` 已把该文件纳入统一基准，仍采用 Git 二进制流提取并校验 SHA-256；外部基准目录同样必须通过哈希校验。正式构建不得从 `obj/RestoredBaseline` 解析依赖。

`tests/Audit-MonoTextEditor.ps1` 只做静态读取，报告写入被忽略的 `obj/MonoTextEditorAudit/audit.json`。未传候选目录时记录固定基准；传入候选目录后，程序集身份、非私有 API、序列化布局、资源哈希、引用身份和 P/Invoke 任一差异都会失败。固定基准为 421 个类型、39 项嵌入资源、4 个可序列化类型和 12 个原生模块名。

`tests/Test-MonoTextEditor.ps1` 会在两个隔离目录分别加载固定原库和候选库，比较文本编辑、行列换算、撤销重做、选区、普通/正则搜索、折叠以及内置语法和样式资源的确定性结果。测试程序只按原库 API 编译，两个进程使用相同的其余依赖，避免同名程序集复用掩盖差异。

反编译候选位于被忽略的 `obj/MonoTextEditorAudit/original`，由 ILSpy `11.0.0.9375` 以项目模式、C# 7.3 导出，共 228 个 C# 文件。反编译无法恢复原始注释、提交历史、工程条件和授权来源，因此该目录只是审计输入，不是可直接提交的源码来源。

[MonoDevelop 5.9.5.10 的 Mono.Texteditor 源码目录](https://github.com/mono/monodevelop/tree/48d16bc4f12ce3938964fc7c3d72fdc6887ad4ad/main/src/core/Mono.Texteditor)是当前恢复基线。5.9.5.10、5.9.6.65、5.9.7.22 和 5.9.8.0 的该目录树均为 `ea03c8f2513e69f82d79cb38e9c826bfb87f4401`；原 DLL 的 PE 时间戳为 `2015-09-15 02:27:53Z`，包含 5.9 新增代码而不包含 5.10 后续代码。原先记录的 5.4 只能作为更早的历史参考，不能作为本次源码基线。

正式目录现有 161 个未经格式化的上游 C# 文件，原注释和许可头均保留；其中 159 个与上游项目编译清单一致，两个历史文件保留但明确排除。没有复制反编译 C#。30 份语法 XML、8 份样式 JSON 和 `gui.stetic` 与原 DLL 的 39 项资源逐字节一致。来源、判断边界及逐文件 SHA-256 分别记录在 `third-party/MonoDevelop/Mono.TextEditor/SOURCE_PROVENANCE.md` 和 `SOURCE_MANIFEST.tsv`；`tests/Test-MonoTextEditorSource.ps1` 只做静态校验，不还原或编译。

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

正式源码目录固定为 `third-party/MonoDevelop/Mono.TextEditor/`。`Mono.TextEditor.Restored.csproj` 已按 .NET Framework 4.8、C# 7.3、AnyCPU、unsafe 和程序集版本 `1.0.0.0` 建立，上述 8 个直接引用已改为 ProjectReference；主方案将通过间接依赖覆盖 MonoDevelop.Ide。Mono.TextEditor 不反向引用 SourceEditor2、Debugger、DesignerSupport、Refactoring、MonoDevelop.Core、MonoDevelop.Ide 或 CocoStudio 应用项目。

Core、Ide 继续使用现有 DLL；本批不扩展到它们。SourceEditor2 已存在 Cocos 定制入口，包括对 `CocoStudio.SourceEditor` 的友元授权，以及 `ProcessSaveText`、`ProcessLoadText`、`PrepareToSetCaret`。后续替换 Mono.TextEditor 时必须让这些调用链保持原行为，不能因采用上游代码而覆盖本地定制。

## API、资源与平台约束

后续候选必须与原库比较完整程序集表面，而不是只检查能否编译：类型、方法、字段、属性、事件、可见性、程序集属性、友元程序集和序列化标记均须一致。原库友元程序集为 `MonoDevelop.TextEditor.Tests`；4 个序列化类型及其字段布局须单独核对。

39 项资源的逻辑名称和字节必须保持一致，包括 `gui.stetic`、30 份语法 XML 和 8 份颜色样式 JSON。Lua、C#、JSON、XML 等语法资源不能只按文件数比较。候选还要保留 12 个 Windows/Linux/macOS 原生模块调用名称；Windows 验证不能替代 GTK Quartz 或 libc 路径的跨平台结论。

Debugger、SourceEditor2、DesignerSupport、Refactoring 和 ProjectSetting 的 `gui.stetic` 共含 5 处旧 `build/bin/Mono.TextEditor.dll` 路径。静态追踪没有发现业务 C# 在运行时读取这些路径，现阶段将其视为 Stetic 设计时元数据候选，但在未做设计器和运行时验证前不能下最终结论。为维持资源字节对照，本批不改写；如后续证明流程会解析它们，再在源码接入提交中改为与仓库根无关的统一引用方式。

## 验证与切换结果

获得编译测试授权后，已在没有旧输出可复用的独立工作副本通过 `CocosStudio.slnx` 执行 Debug/x86 与 Release/x86 的 NuGet 还原和冷构建。验证范围包括：

1. 程序集身份、完整 API、程序集属性、资源名称与资源字节对照。
2. 文本载入/保存、BOM、编码、换行、撤销重做、选区、块选择和光标移动。
3. 搜索替换、语法高亮、折叠、标记、补全、滚动、测量和布局。
4. SourceEditor2、CocoStudio.SourceEditor、LuaBinding、Debugger、DesignerSupport、Refactoring 和 Render 集成回归。
5. 编辑器打开/保存、补全弹窗、断点标记、属性面板及 Stetic 设计器的用户交互验收。

正式引用已切到 `third-party/MonoDevelop/Mono.TextEditor/`。冷构建、静态对照、自动回归和交互验收全部通过后，已删除 `dlls/Mono.TextEditor.dll`；不保留源码/DLL 双轨正式构建，也不从测试基准目录兜底。

独立工作副本 `cocos_studio-mte-cold-20260907` 已完成 `CocosStudio.slnx` Debug/x86 与 Release/x86 冷构建，分别为 0 错误、212/211 个既有警告。九份源码产物和 Mono.Debugging NuGet 产物的输出哈希一致。

Mono.TextEditor 新旧对照结果：5,411 项语义程序集表面、4 个可序列化类型及字段、39 项资源、12 个原生模块和 62 个 P/Invoke 无未解释差异；全量审计的 Mono.TextEditor 成员引用为 1,170 处，均解析到独立候选输出。已显式记录并锁定四项允许迁移：目标框架 4.5 到 4.8、Xwt 0.1 到仓库现用 0.2.251、Mono.Posix 2 到仓库现用 4，以及 Gtk helper 的 Mono.Cairo 2 作用域统一到仓库现用 Mono.Cairo 4；未新增绑定重定向。

Debug/Release 候选均通过文本编辑、撤销重做、搜索替换、折叠、语法资源对照，以及编辑器、Lua、DesignerSupport、Refactoring、断点持久化、47 项模块注册和 MSBuild 桥接回归；Gtk/Cairo 的 GAC 与本地候选两种加载模式也通过。缺失历史、错误基准哈希、旧回退开关和生产构建引用测试缓存均按预期失败。

用户已完成完整编辑器的打开/保存、补全弹窗、实际断点、属性面板、长文件滚动和 Stetic 设计器交互验收。随后在独立工作副本删除旧 DLL、再次清空候选输出和中间目录，Debug/x86 与 Release/x86 均重新构建通过；两种配置的 API、资源、成员解析及关键编辑器行为回归仍无未解释差异。
