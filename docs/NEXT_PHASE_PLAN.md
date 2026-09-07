# 下一阶段计划：恢复并替换 Mono.TextEditor

更新日期：2026-09-07。执行基线为本地提交 `3b3e3ac`。

前一阶段已经完成目录整理、历史 `bin/obj` 清理、Mono.TextEditor 固定基准和静态审计。本阶段不再重复这些工作，目标是在一个完整模块批次内恢复 Mono.TextEditor 源码、验证、切换正式引用并删除原 DLL。

执行状态：第一批测试门禁已建立；第二批源码恢复待开始。测试脚本尚未针对候选程序集运行。

## 目标与边界

- 正式源码目录：`third-party/MonoDevelop/Mono.TextEditor/`。
- 程序集身份保持 `Mono.TextEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null`。
- 固定原库继续取自提交 `632862e2e3dc6485fadc3f30d4454f97a9187c2d`，SHA-256 为 `47DAA700220831CD86F0135E8CC57B920D4D8BF76AB734D781DE6BC27D0800B6`。
- 保持 39 项嵌入资源的逻辑名称、属性和字节，不改写五个消费项目中的 Stetic 旧路径，除非后续运行证明确实需要解析它们。
- 保持 SourceEditor2 的 Cocos 定制调用链，不覆盖 `ProcessSaveText`、`ProcessLoadText`、`PrepareToSetCaret` 和对 `CocoStudio.SourceEditor` 的友元授权。
- 不同时恢复 MonoDevelop.Core 或 MonoDevelop.Ide，不升级 GTK、NRefactory、Xwt、Mono.Cairo、Mono.Posix，也不改模块发现和工作台结构。
- 不混入当前资源定位修改；仓库只维护 `CocosStudio.slnx`，不重新生成 `.sln`。只做本地 Git 提交，不推送。

详细身份、依赖和行为范围见 `docs/MONO_TEXT_EDITOR_RECOVERY.md`。

## 第一批：补齐新旧对照和切换门禁

这一批只增强测试基础设施，不改变正式引用，也不加入候选源码。

1. 扩展 `tests/Audit-MonoTextEditor.ps1`，支持同时读取固定原库和候选程序集，并比较：
   - 程序集名称、版本、公钥、目标框架和程序集属性；
   - 全部类型和非私有成员签名、可见性、基类、接口及泛型约束；
   - 友元程序集、4 个序列化类型及其字段布局；
   - 39 项资源的名称、属性和逐项 SHA-256；
   - 程序集引用身份、12 个原生模块名和 P/Invoke 入口点。
2. 增加 Mono.TextEditor 独立的新旧行为测试。原库与候选库在不同进程中加载，禁止因程序集同名而复用错误版本。
3. 无 GTK 窗口的测试先覆盖文本、行列、锚点、选区、撤销重做、搜索替换、折叠、语法模式和颜色样式；需要 GTK 的标记、布局、滚动和绘制测试沿用现有隔离环境。
4. 把 Mono.TextEditor 加入现有消费者成员解析审计，确认 9 个既有消费者的成员引用都解析到候选输出，而不是 `dlls` 或测试基准缓存。
5. 保留并检查负例：缺失固定历史、外部基准哈希错误、`UseRestoredMonoDevelop=false`、正式构建引用测试缓存均明确失败。

建议单独提交：`测试：补充 Mono.TextEditor 新旧对照与切换门禁`。

该提交可以在不编译候选源码时先完成脚本结构，但测试结果必须等候选程序集产生后才算有效。不能把静态脚本通过写成源码恢复通过。

## 第二批：恢复源码并建立独立项目

源码输入分为两类：

- MonoDevelop 5.4 固定提交 `01786bc67c7024ec33d327ed27e4416d7a846f4e` 的 `main/src/core/Mono.Texteditor`，用于获取原始目录、注释和许可信息；
- 被忽略的 `obj/MonoTextEditorAudit/original` 反编译结果，用于识别当前 DLL 的真实 API、资源和定制行为。

实施规则：

1. 建立逐文件来源清单，将文件标为“上游可确认”“当前 DLL 有差异”“只能由反编译恢复”或“编译器生成，不应作为源码恢复”。
2. 优先采用能够与当前 DLL 行为对应的上游源码并保留原注释；差异文件逐项说明，禁止把整份上游目录或整份反编译输出直接覆盖进仓库。
3. 不删除现有代码注释；补充上游许可和来源说明，不能把反编译结果标成未经修改的官方源码。
4. 新建 `Mono.TextEditor.Restored.csproj`，使用仓库现有 .NET Framework 4.8、C# 7.3 和 AnyCPU 约定，保留 unsafe 设置、程序集版本和 `MonoDevelop.TextEditor.Tests` 友元授权。
5. 依赖先沿用当前仓库版本；实际编译后核对解析出的程序集身份是否仍符合原库约束。任何 GTK# 2.12、NRefactory 5.0、Xwt 0.1、Mono.Cairo 2.0/4.0 或 Mono.Posix 2.0 身份变化都必须显式处理，不能只加绑定重定向。
6. 逐项嵌入 30 份语法 XML、8 份样式 JSON 和 `gui.stetic`，逻辑名称及字节以固定原库为准。

源码和项目先保留在工作区中验证，不单独提交一个长期与旧 DLL 并存的正式版本。

## 第三批：接入真实项目引用

候选独立审计通过后，将以下 8 个直接 DLL 引用改为 `ProjectReference`：

- `src/Editor/CocoStudio.LuaBinding/CocoStudio.LuaBinding.Restored.csproj`
- `src/Editor/CocoStudio.SourceEditor/CocoStudio.SourceEditor.Restored.csproj`
- `src/Framework/CocoStudio.Gtk.Extend/CocoStudio.Gtk.Extend.csproj`
- `src/Modules/Communal/Modules.Communal.Render/Modules.Communal.Render.csproj`
- `third-party/MonoDevelop/MonoDevelop.Debugger/MonoDevelop.Debugger.Restored.csproj`
- `third-party/MonoDevelop/MonoDevelop.DesignerSupport/MonoDevelop.DesignerSupport.Restored.csproj`
- `third-party/MonoDevelop/MonoDevelop.Refactoring/MonoDevelop.Refactoring.Restored.csproj`
- `third-party/MonoDevelop/MonoDevelop.SourceEditor2/MonoDevelop.SourceEditor2.Restored.csproj`

同时修改公共构建和验证：

- `build/RestoredMonoDevelop.targets`：排除传递进来的旧 Mono.TextEditor 副本，并继续拒绝基准缓存进入正式解析路径。
- `Directory.Build.targets`：正式输出不再从 `dlls` 复制 Mono.TextEditor。
- `tests/Build-RestoredMonoDevelop.ps1`：把源码中间产物与最终输出的哈希校验扩展到 Mono.TextEditor。
- `tests/Audit-RestoredMonoDevelop.ps1` 及编辑器回归：把 Mono.TextEditor 加入 API、资源和消费者解析对照。
- `CocosStudio.slnx` 仅在真实项目引用不足以保证构建顺序时补项目项；不能靠重新放回旧 DLL 修复顺序。

Mono.TextEditor 不得反向引用上述消费者、MonoDevelop.Core 或 MonoDevelop.Ide。若出现循环，停止并重新划分引用，而不是复制接口或保留 DLL 回退。

## 第四批：编译、自动测试与交互验收

此批需要用户明确允许编译测试后执行。必须在不能复用旧输出的独立工作副本中验证：

1. `CocosStudio.slnx` 的 Debug/x86 完整还原和冷构建。
2. `CocosStudio.slnx` 的 Release/x86 完整还原和冷构建。
3. 核对 `obj/Mono.TextEditor.Restored/<配置>/Mono.TextEditor.dll` 与最终输出来自同一次源码构建，且正式解析路径不含 `dlls/Mono.TextEditor.dll` 或 `obj/RestoredBaseline`。
4. 运行完整 API/资源审计、消费者成员解析、Mono.TextEditor 独立行为测试、编辑器/Lua/DesignerSupport/断点/Refactoring 集成回归。
5. 验证缺失历史、基准哈希损坏、旧回退开关和测试缓存污染四类负例。

用户交互验收保留以下项目：打开和保存不同编码/换行文件、撤销重做、块选择、搜索替换、语法高亮和折叠、补全弹窗、断点及执行行标记、属性/工具箱面板、长文件滚动和 Stetic 设计器。自动测试不能代替这些操作。

## 第五批：删除旧 DLL 并提交模块

只有第四批全部通过，才删除 `dlls/Mono.TextEditor.dll`。删除后在同一个独立工作副本再次执行冷构建和关键回归，确认没有输出缓存或隐式复制掩盖缺失依赖。

最终模块提交应同时包含：

- Mono.TextEditor 源码、项目文件、资源和来源说明；
- 8 个项目引用切换及公共构建修改；
- 新旧对照、行为回归和负例测试；
- 原 DLL 删除；
- `README.md`、`docs/DEPENDENCIES.md`、`docs/DEPENDENCY_AUDIT.md`、`docs/MONO_TEXT_EDITOR_RECOVERY.md` 更新。

建议提交：`源码：恢复 Mono.TextEditor 并删除原 DLL`。

不得提交 `obj/MonoTextEditorAudit`、`obj/RestoredBaseline`、构建输出或用户当前业务修改。只本地提交，不推送。回滚依靠 Git 恢复完整提交，不维护源码/DLL 双轨正式构建。

## 停止条件

出现以下任一情况即停止切换，不删除原 DLL：

- 当前 DLL 的差异无法从上游或可说明的恢复代码中重建；
- 许可或来源无法满足仓库保留要求；
- API、序列化布局、资源字节、原生入口或程序集身份存在未解释差异；
- 任一消费者仍从旧 DLL 或测试基准解析成员；
- `CocosStudio.slnx` 的 Debug/Release 任一冷构建失败；
- 自动回归或用户交互验收发现编辑、补全、断点、布局等行为回退。

## 完成后的后续顺序

Mono.TextEditor 完整收尾后，先重新生成剩余 14 个 DLL 的直接引用、二进制引用和动态加载审计，再决定下一库。默认顺序仍为 MonoDevelop.Core 后 MonoDevelop.Ide；WindowsPlatform、Cecil/IKVM 组合和应用专用模块继续单独立项，不与编辑器核心恢复混合。
