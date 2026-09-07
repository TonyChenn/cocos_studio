# 下一阶段计划：清理历史产物并准备 Mono.TextEditor 恢复

更新日期：2026-09-07。执行基线为本地提交 `27eacca`。

执行状态：第一批历史构建产物清理已完成；第二批 Mono.TextEditor 基准与恢复审计进行中。

## 目标与边界

本阶段先完成目录整理后的静态收尾，再形成可以直接实施的 Mono.TextEditor 恢复设计。

- 删除已经受版本控制的 58 个项目局部 `bin/obj` 产物，不删除用户当前其他构建输出。
- 为 Mono.TextEditor 建立固定历史基准、当前依赖证据和恢复方案，但不把反编译源码接入正式构建。
- 不替换或删除 `dlls/Mono.TextEditor.dll`，不推进 MonoDevelop.Core、MonoDevelop.Ide 或其他高风险依赖。
- 不升级 GTK、NRefactory、Xwt、Mono.Cairo、Mono.Posix，不改写工作台或插件发现机制。
- 不编译、不还原 NuGet、不运行自动回归、不启动或关闭编辑器。
- 保留当前资源定位相关修改和三个 `.sln` 删除状态；它们不进入本阶段提交。
- 只做本地 Git 提交，不推送。

## 第一批：清理 58 个历史构建产物

固定删除范围通过 `git ls-files` 中路径包含 `/bin/` 或 `/obj/` 的文件取得，当前共 58 个：

- 36 个自动生成的 `.NETFramework,Version=*.AssemblyAttributes.cs`；
- 9 个 `CocoStudio.Lib.Prism/bin/Debug` 下的 Gtk# DLL/PDB 副本；
- 13 个程序集引用缓存、编译输入缓存、`.resources` 和 `.Up2Date` 文件。

这些文件均可由构建重新生成，静态搜索未发现项目文件或脚本以源文件方式依赖它们。实施要求：

1. 按上述 Git 清单逐文件删除，不递归删除工作区中的 `bin/obj` 目录，不触碰未跟踪的用户输出。
2. 保留现有全局 `bin`、`obj` 忽略规则；新增静态防回归检查，只要 `git ls-files` 再出现 `/bin/` 或 `/obj/` 就明确失败。
3. 更新根目录说明，移除“58 个历史产物暂留”的描述，改为记录清理提交和恢复方式。
4. 暂存时显式排除现有业务修改及三个 `.sln` 删除记录；提交前确认没有业务 C# 差异。
5. 单独提交，建议日志：`清理：删除历史构建产物并禁止重新纳入版本控制`。

静态验收：Git 中已跟踪的项目局部 `bin/obj` 文件数量为 0；目录、项目引用和资源路径检查仍通过；`git diff --check` 无新增问题。删除内容可由 Git 恢复。

## 第二批：建立 Mono.TextEditor 恢复基准与审计

### 固定基准

当前库的固定身份为：

- 文件：`dlls/Mono.TextEditor.dll`
- 程序集：`Mono.TextEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null`
- SHA-256：`47DAA700220831CD86F0135E8CC57B920D4D8BF76AB734D781DE6BC27D0800B6`
- Git Blob：`460ef7fe657cf8ef2ba22b59aa2d91619dc4a955`
- 历史来源：固定提交 `632862e2e3dc6485fadc3f30d4454f97a9187c2d`；该提交、当前 HEAD 和工作区文件的 Blob 一致。

将 Mono.TextEditor 加入 `tests/RestoredBaseline.ps1` 的统一清单。提取继续使用 Git 二进制流和 SHA-256 校验，写入被忽略的测试缓存；外部基准目录也必须包含并通过这第十个文件的校验。正式构建不得从基准缓存解析依赖。

只执行不触发还原或构建的静态负例检查；现有会调用 Restore/Build 目标的基准回归留待以后获得编译测试授权时执行。

### 依赖与兼容审计

重新生成目录迁移后的静态审计，替换旧报告中的历史项目路径。由于本阶段不重新编译：

- 当前源码中的直接引用作为新目录证据；现有 8 个直接项目必须全部列出。
- 现有 9 个二进制消费者只能标为“目录迁移前构建输出快照”，不能作为新目录构建证明。
- 审计程序集身份、公开/内部 API、嵌入资源及逻辑名称、程序集属性、友元程序集、序列化类型、原生导入和动态加载字符串。
- 特别记录 GTK# 2.12、NRefactory 5.0 强名称、Xwt 0.1、Mono.Cairo 2.0/4.0 和 Mono.Posix 2.0 的身份约束；不得用绑定重定向掩盖差异。
- 检查 SourceEditor2 的定制入口，包括 `InternalsVisibleTo("CocoStudio.SourceEditor")`、`ProcessSaveText`、`ProcessLoadText` 和 `PrepareToSetCaret`，避免用上游实现覆盖本地行为。

反编译结果和分析中间文件统一放入被忽略的 `obj/MonoTextEditorAudit/`，不把候选源码加入正式项目。使用与既有恢复工作一致的 ILSpy/C# 版本设置，记录工具版本、原文件哈希、文件数、类型数、资源数及无法还原的信息。

### Stetic 旧路径专项

当前五份 `gui.stetic` 含旧 Mono.TextEditor 构建路径：Debugger、SourceEditor2、DesignerSupport、Refactoring 各一份，ProjectSetting 一份。

先静态追踪 Stetic 生成、设计时和运行时读取链路，结论写入恢复设计：

- 若只是未参与构建和运行时解析的设计器元数据，保留原字节并明确说明；
- 若当前流程会解析该路径，在后续源码接入批次设计统一、与仓库根无关的引用方式；
- 本阶段不直接改写资源，因为现有对照要求资源逐字节匹配旧 DLL。

### 恢复设计交付

新增 `docs/MONO_TEXT_EDITOR_RECOVERY.md`，必须明确：

1. 正式目标目录为 `third-party/MonoDevelop/Mono.TextEditor/`，程序集名称、版本、公钥状态和资源逻辑名称保持原值。
2. 依赖接入顺序及禁止形成的循环引用；现阶段 Core、Ide 继续使用原 DLL。
3. 原库与反编译候选的 API/资源差异及每项处理结论，不把“可以编译”当作行为等价。
4. 后续测试矩阵：文本载入/保存、BOM 与换行、撤销重做、选区和光标、搜索替换、语法高亮、折叠、标记、补全、滚动和布局，以及 SourceEditor2、Debugger、Refactoring 集成。
5. 后续切换规则：获得编译测试授权后，在独立工作副本完成 Debug/Release 冷构建和回归；全部通过时才接入源码并在同批删除原 DLL。失败时回退完整提交，不维持双轨正式构建。

第二批单独提交，建议日志：`文档：建立 Mono.TextEditor 恢复基准与兼容方案`。

## 最终验收与未验证项

本阶段结束时应满足：

- Git 不再跟踪项目局部 `bin/obj` 产物；当前用户输出未被递归清理。
- 固定基准包含十份 DLL，Mono.TextEditor 的历史、HEAD 和工作区身份一致。
- 依赖审计使用新目录路径，并区分源码静态证据与旧二进制快照。
- Mono.TextEditor 恢复文档对依赖、定制点、Stetic 路径、失败条件和后续测试作出明确决定。
- `dlls/Mono.TextEditor.dll` 仍存在且正式构建引用不变。
- 两笔提交均不含现有业务修改和三个 `.sln` 删除，不推送。

本阶段不提供任何编译、运行时或交互行为证明。目录迁移后的冷构建、完整编辑器操作、实际调试和 macOS 行为继续列为未验证项。
