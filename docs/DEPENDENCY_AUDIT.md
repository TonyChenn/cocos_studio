# 剩余二进制依赖审计

核对日期：2026-09-07。已替换的十份旧 DLL 已删除，当前 `dlls` 剩 14 个。
项目声明已按目录迁移后的当前源码重新扫描；二进制消费者仍来自迁移前的 `bin/Debug` 完整构建快照，不构成新目录已编译的证明。
源码项目统计含主方案外项目，不等于全部项目均已构建。
审计同时读取程序集引用、模块注册属性、原生导入以及 IL 字符串；字符串命中只提供调查线索，不证明分支在运行时执行。
独立新旧注册扫描仍为 47 项，当前没有足够依据直接再删除下表任意一项。

## 逐库结论

| 库 | 当前保留证据 | 后续处理与障碍 |
| --- | --- | --- |
| Addins.LuaExtend | 有 Addin 注册；与现有模块发现流程相关 | 应用专用模块，先核对 Lua 扩展契约和原始生成来源，不能按无直接引用删除 |
| Addins.LuaExtendWrap | 被 Addins.LuaExtend 引用 | 与扩展模块成组恢复，避免只替换包装层 |
| Addins.ModelExtend | CocoStudio.Model.Lua 直接引用且输出仍有程序集引用 | 模型/Lua 桥接专用库，需要接口和模型行为回归 |
| Cocos.Launcher.Resource | CocoStudio.Gtk.Extend 直接及二进制引用 | 属于资源程序集；恢复资源标识、内容及所有消费路径后才能去掉 DLL |
| CocoStudio.DefaultResource | 4 个项目直接引用，4 个输出引用 | 同上，不能用通用 NuGet 包替代应用资源 |
| Modules.Animation | 有 Addin 注册及 32 处名称字符串候选 | 动画内置模块；需时间轴/序列化/模块加载回归，不属于在线插件安装功能 |
| Mono.Addins.CecilReflector | Mono.Addins 的 GetReflectorForFile、OnResolveAddinAssembly 共 4 处加载字符串 | 是内置模块扫描器；Cecil 包升级必须连同扫描器及其他调用方验证 |
| Mono.Addins.Gui | MonoDevelop.Ide 有程序集引用 | 不用安装/更新界面不代表引用已消失；先在 Ide 源码中解除依赖或恢复对应库 |
| MonoDevelop.Core | 42 个项目直接引用，46 个输出引用 | 基础服务覆盖广，且直接引用 IKVM.Reflection；按子系统恢复和审计，不跨组强换依赖 |
| MonoDevelop.Ide | 37 个项目直接引用，34 个输出引用 | 工作台/项目系统依赖广，并引用 NRefactory.IKVM、Mono.Addins.Gui；放在基础层之后 |
| WindowsPlatform | 源码版 CocoStudio.WindowsPlatform 直接及二进制引用 | 仍使用旧 Code Pack 内部接口；先恢复适配代码并替换内部调用，再决定换包 |
| ICSharpCode.NRefactory.IKVM | MonoDevelop.Ide 引用；NRefactory.AssemblyLoader.Create 有动态名称 | 必须与 IKVM.Reflection、Core/Ide 的调用方一起验证，保留强名称身份约束 |
| IKVM.Reflection | NRefactory.IKVM 和 MonoDevelop.Core 均引用 | 当前公钥标记 ed091f233d5d52a4；既往候选身份不兼容，不能仅做版本重定向 |
| Xamarin.Mac | 4 个项目直接引用，4 个输出引用；Xwt 有 AppKit 类型加载字符串 | 保留现有 macOS 支持；Windows 测试没有覆盖 macOS 行为，不能据此删除 |

## 动态依赖证据

- `Mono.Addins.Database.AddinFileSystemExtension.GetReflectorForFile` 含 `Mono.Addins.CecilReflector.dll` 与反射器类型名。
- `Mono.Addins.Database.AddinDatabase.OnResolveAddinAssembly` 含 `Mono.Addins.CecilReflector`。
- `ICSharpCode.NRefactory.TypeSystem.AssemblyLoader.Create` 含 `ICSharpCode.NRefactory.IKVM`。
- `Xwt.GtkBackend.EmbeddedWidgetBackend.SetContent` 含 `AppKit.NSView, Xamarin.Mac`。
- LuaExtend、Animation 的 Addin 属性及组件注册扫描属于内置模块发现；不因用户不使用在线插件安装/更新而取消。

## 分组后续任务

1. **编辑器基础层**：Mono.TextEditor 已完成恢复、验证和旧 DLL 删除；下一步按依赖推进 Core、Ide。保持现有框架和平台，不在同一批升级 GTK 或重写工作台。
2. **Windows 文件对话框层**：针对旧 WindowsPlatform 的 `Attach`、`IFileDialogCustomize`、`customize/nativeDialog` 等内部访问设计公开 API 适配；以真实文件/目录选择验证，未通过前不强换 Code Pack。
3. **程序集扫描/导入层**：CecilReflector/Cecil 与 NRefactory.IKVM/IKVM 分别成组处理。重新核对届时候选包身份和 API；第八批历史包调查不是永久结论，也不表示最新包已测试。
4. **应用专用模块与资源**：独立核对来源及生成过程；不把反编译视为恢复了原始注释、授权信息或所有运行时行为。

每批继续执行“替换 → 对照/冷构建验证 → 删除旧副本 → 更新文档 → 本地提交”，不推送。
以上为后续独立任务边界，本轮没有实施这些高风险组，也没有移除平台或内置模块功能。

## 重跑审计

通用审计运行 `tests/Audit-VendoredDependencies.ps1 -OutputDirectory <构建输出>`；Mono.TextEditor 专项静态审计运行
`tests/Audit-MonoTextEditor.ps1 -OutputDirectory <含 Mono.Cecil 的输出目录>`。
完整程序集身份、SHA-256、直接项目清单、二进制消费方和字符串位置写入忽略的 `obj/VendoredDependencyAudit/dependencies.json`。
专项结果写入忽略的 `obj/MonoTextEditorAudit/audit.json`，并明确区分当前源码路径与既有输出快照。
审计不是完整 IDE、实际调试、动画或 macOS 的运行时验证；相关交互仍需用户验收。
