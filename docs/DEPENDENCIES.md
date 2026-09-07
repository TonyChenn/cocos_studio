# DLL / NuGet 迁移记录

## 当前清理状态

已将前期源码恢复按模块提交，并删除八份已替换的旧 DLL：Mono.Debugging、MonoDevelop.Debugger、SourceEditor2、
DesignerSupport、Projects.Formats.MSBuild，以及 CocoStudio.SourceEditor、LuaBinding、WindowsPlatform。
`dlls` 从 24 个减至 16 个；未删除仍被使用的库。以下旧批次的“保留/回退”描述仅记录历史状态。
随后完成 Refactoring 同批恢复和删除，当前进一步减至 15 个，累计删除九份已替换旧库。
当前取消 `UseRestoredMonoDevelop=false` 构建；恢复完整历史版本应使用 Git。
旧库对照测试统一从固定提交 `632862e2e3dc6485fadc3f30d4454f97a9187c2d` 提取并校验 SHA-256，
测试缓存不受版本控制，也不能参与正式构建；历史缺失时可显式提供同样受校验的基准目录。

仓库现只维护 `CocosStudio.slnx`，所有 `.sln` 已删除。下文出现的 `.sln` 构建结果仅是删除前的历史验证记录，不再是当前构建入口。

本批全新工作副本验证暴露并修复两个问题：Git 自动换行改变嵌入资源字节，现通过 `.gitattributes` 对资源禁用文本转换；
方案外源码项目在 Release 时被 MSBuild 清除父配置，现保留父配置，构建脚本按实际 Debug/Release 校验各份产物。
Debug 两种方案、Release/x86 重建与接口/资源审计通过；397 处引用、104 项资源、编辑器/Lua/DesignerSupport、
断点/47 项模块扫描及 MSBuild 桥接回归通过。缺少基准历史、损坏哈希和旧回退开关均明确失败。
完整 IDE 交互和实际调试仍由用户验收，不以自动测试替代。

核对日期：2026-09-04。目标是兼容现有 .NET Framework 4.8 / Gtk# 编辑器，
不是把所有依赖强制更新到最新版本。源码目录和代码注释保留。

## 已迁移

| 库 | 固定 NuGet 版本 |
| --- | --- |
| log4net | 3.4.0 |
| SharpZipLib | 1.3.3 |
| Microsoft-WindowsAPICodePack-Core / Shell | 1.1.5 |
| Newtonsoft.Json | 13.0.4 |
| Mono.Addins / Mono.Addins.Setup | 1.3.7 |
| PostSharp | 4.0.43（仅编译和运行时资产，不重新织入） |
| Mono.Cecil | 0.9.6.4 |
| Xwt / Xwt.Gtk / Xwt.WPF | 0.2.251 |
| Mono.Posix-4.5 | 4.5.0 |
| ICSharpCode.NRefactory | 5.5.1 |
| Mono.GtkSharp（第三方 Gtk# 2.12 封装，筛选托管资产） | 2.12.0.1 |

具体引用以项目文件为准；程序集版本不一定等于 NuGet 包版本。
`CocosStudio.slnx` 不再构建旧 NRefactory 源码项目，
避免同名输出覆盖 NuGet DLL；源码目录仍保留。

## 第八批清理

删除 `dlls/Modules.Communal.MutualEditor.dll` 的旧预编译副本。
四处项目依赖均为 ProjectReference；全局复制规则本就排除此同名源码项目的 DLL。
模块功能、源码和项目引用不变，输出继续来自源码编译。

## 第八批候选库调查（历史结论，后续进展见各批次）

| 库 | 实际障碍 | 本轮决定 |
| --- | --- | --- |
| Mono.Debugging | 对 NuGet 当前列出的全部 6 个版本检查了旧输出中的 269 处成员引用。4 个 2016 版本各缺少 2 处引用；2 个 2017 版本各缺少 4 处；现有 DLL 为 0 处缺失。 | 保留现有 DLL。 |
| Mono.Addins.CecilReflector | NuGet 1.3.6 / 1.3.7 依赖 Mono.Cecil >= 0.10.0-beta5；已下载的 1.3.7 引用 Cecil 的公钥标记为 `50cebf1cceb9d05e`，旧 MonoDevelop.DesignerSupport 引用为 `0738eb9f132ed756`。 | 不强制升级 Cecil；版本重定向不能解决公钥差异。 |
| IKVM.Reflection | 已下载的 NuGet 7.2.4630.5 公钥标记为 `13235d27fcbfff58`，现有 DLL 为 `ed091f233d5d52a4`，旧 NRefactory.IKVM 依赖后者。 | 保留成套 DLL。 |
| ICSharpCode.NRefactory.IKVM / Mono.TextEditor / Mono.Addins.Gui / MonoDevelop.Core | 核对 NuGet 同名包版本索引返回 404；不代表不存在其他名字的移植版或可行的源码升级方案。 | 本轮不使用第三方移植包强行替换。 |

Mono.Debugging 的共同缺口是 `DebuggerSession.TargetExited` 从 `EventHandler`
改为 `EventHandler<TargetEventArgs>`，旧 `MonoDevelop.Debugger.dll` 和
`MonoDevelop.SourceEditor2.dll` 都仍引用旧签名。2017 候选还缺少旧的
`BreakpointStore.Save()` / `Load(XmlElement)`。审计已确认解析路径指向候选包，
不是意外回退到本地旧 DLL；这是静态兼容性检查，不是新调试器运行测试。

后续若继续这些库，需要单独处理旧 MonoDevelop 调用方的源码/API 适配，
或设计并验证依赖隔离，不能作为普通换包处理。

## 保留范围

`dlls` 剩余 24 个 DLL，包含 Cocos 专用二进制、旧 MonoDevelop 编辑器/调试器、
Mono.Addins.Gui / CecilReflector、Mono.TextEditor、Mono.Debugging、IKVM 配套库及
Xamarin.Mac。没有因为缺少直接 csproj 引用就删除它们：运行时发现和二进制间接依赖
同样需要考虑。此清单不代表这些库仍被维护或已经通过全部平台测试。

## 第八批验证

- 隔离输出目录：`bin/NuGetPhase8Debug`；未覆盖正在使用的 `bin/Debug`。
- `CocosStudio.sln` Debug/x86 重建：0 错误，108 警告。
- `CocosStudio.slnx` Debug/x86 重建：0 错误，108 警告；4 个 NRefactory 输出 DLL 的 SHA-256 均与 NuGet 包一致。
- ExternalImport Debug/Any CPU 重建：0 错误，1 个架构警告。
- 独立 x86 注册扫描：47 项组件标识与上一批一致。
- `.slnx` 输出的 C# 解析/语义分析、错误语法检测、旧 IKVM 导入和文本编辑器文档读写冒烟测试通过。
- MutualEditor 输出 DLL 与本次源码编译中间产物 SHA-256 相同，与旧副本不同。
- GUI 操作、真实调试会话及 macOS 行为未验证。

本机审计日志、下载包和删除文件的备份位于被忽略的 `obj/NuGetPhase8Audit/`，
不属于提交内容；旧 DLL 也可以从 Git 历史恢复。

参考：[CecilReflector 1.3.6](https://www.nuget.org/packages/Mono.Addins.CecilReflector/1.3.6)、
[CecilReflector 1.3.7](https://www.nuget.org/packages/Mono.Addins.CecilReflector/1.3.7)、
[IKVM.Reflection 7.2.4630.5](https://www.nuget.org/packages/IKVM.Reflection/7.2.4630.5)、
[Mono.Debugging 版本索引](https://api.nuget.org/v3-flatcontainer/mono.debugging/index.json)。

## 第九批：Gtk# 托管引用迁移

Gtk# 2.12 原先由本机安装环境提供，不在 `dlls` 的 24 个保留 DLL 中。
本批改用第三方 NuGet 包 [Mono.GtkSharp 2.12.0.1](https://www.nuget.org/packages/Mono.GtkSharp/2.12.0.1)
提供编译和分发所需的托管 DLL；保留既有原生 GTK 安装，不安装 GTK 3，不修改 GAC。

- 对第八批输出中引用 gtk-sharp、glib-sharp、gdk-sharp、atk-sharp、pango-sharp 的
  8,999 处成员引用检查：0 处缺失；确认解析路径实际指向下载包。
- 这五个程序集的版本/公钥与当前安装版一致，但文件 SHA-256 不同。
  静态 API 检查通过不能证明原生交互或界面行为一致。
- 包内直接携带 Mono.Posix 2.0.0.0，而本工程已经通过 Mono.Posix-4.5 使用
  4.0.0.0 程序集；该内嵌文件不是 NuGet 依赖项，不能仅靠依赖版本解析将其升级。
  `build/GtkSharp.targets` 使用 `ExcludeAssets="all"` / `PrivateAssets="all"` 禁止自动引入，
  再通过 `GeneratePathProperty` 定位包，只显式引用并复制 8 个选定 DLL 和对应 dllmap 配置。
  不导入包自身的构建目标，也不复制其 Mono.Posix 文件。
- 包的 `lib/net45` 含 9 个托管 DLL，没有 GTK 原生运行库；现有绑定仍调用
  `libgtk-win32-2.0-0.dll`、`gtksharpglue-2` 等原生模块。
  使用此包不等于摆脱 Gtk# / GTK 安装环境。
- 原有 Gtk# 项目通过共享 targets 统一使用这套引用，并明确使用 Mono.Posix-4.5 4.5.0。
  UndoManager 也补充了该包引用，防止其间接依赖从系统目录复制旧 Mono.Posix。
- 最终主方案 Debug/x86 重建 0 错误、108 警告；编译命令和复制日志中，
  从系统 Gtk# 安装目录取托管库的记录均为 0。8 个输出 DLL 的哈希与 NuGet 包一致，
  Mono.Posix 输出仍为 4.0.0.0。
- `.slnx` Debug/x86 重建 0 错误、108 警告；ExternalImport Debug/Any CPU 重建
  0 错误、1 个架构警告。
- x86 冒烟测试分别验证了默认加载方式和强制使用候选输出的方式：隐藏控件、UTF-8 文本、
  IconView 选择/滚动、Gdk 图片及 Cairo 绘图通过。
- 候选输出还通过 Xwt/GTK 图片转换、编辑器图标按钮、进度对话框、Mono.Posix 翻译回退测试。
  默认和候选加载下的组件注册清单均为 47 项，与第八批相同。

### 运行时边界

本机默认启动仍优先加载 GAC 中同身份的 Gtk#，不能将此路径的成功当作新 DLL 的验证。
候选测试仅在临时测试程序配置启用 developmentMode，并设置进程级 DEVPATH，
逐个断言 8 个程序集来自测试目录；原生 GTK 路径仍指向现有安装。
正式编辑器配置、machine.config、GAC 和系统环境变量均未改动。
DEVPATH 会放宽版本匹配，只用于测试，不能复制此设置到发布环境。

因此本批完成的是托管依赖的 NuGet 管理和兼容性冒烟验证，不是无安装依赖的 GTK 分发。
仍未验证完整人工界面流程、没有 GAC 的干净机器以及 macOS；运行环境需要原生 GTK 2。

### 重复运行基础测试

在仓库根目录的 PowerShell 中运行：

```powershell
.\tests\Test-GtkSharp.ps1 -OutputDirectory .\bin\NuGetPhase9Debug
```

可以通过 `-GtkNativeDirectory` 指定现有 GTK 2 原生库目录。
测试源码在 `tests/GtkSharpSmoke.cs`；脚本编译 x86 测试程序，拷贝待测输出到独立
`obj/GtkSharpSmoke-*` 目录，运行两种加载模式并保存日志，不覆盖编辑器输出或系统文件。
此测试验证指定输出的行为，输出是否来自预期 NuGet 包仍应结合文件哈希核对。

本机下载包、构建日志和额外审计脚本位于被忽略的 `obj/NuGetPhase9Audit/`。
本批没有删除额外 DLL，其余 DLL 保持第八批结论。

构建机制参考：[NuGet 资产筛选与包路径属性](https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files)。
测试机制参考：[DEVPATH 仅用于开发测试](https://learn.microsoft.com/en-us/dotnet/framework/configure-apps/how-to-locate-assemblies-by-using-devpath)。

## 第十批：剩余 DLL 保留依据复核

在第九批构建输出上检查程序集引用、原生模块引用、精确程序集名字符串和模块注册属性，
并对照源码项目引用及第九批的 47 项注册扫描记录。剩余 24 个 DLL 的分类如下，互不重复：

| 保留依据 | 数量 | 说明 |
| --- | --- | --- |
| 项目直接引用 | 7 | Addins.ModelExtend、Cocos.Launcher.Resource、CocoStudio.DefaultResource、Mono.TextEditor、MonoDevelop.Core、MonoDevelop.Ide、Xamarin.Mac。 |
| 没有项目直接引用，但被其他程序集引用 | 10 | Addins.LuaExtendWrap、NRefactory.IKVM、IKVM.Reflection、Mono.Addins.Gui、Mono.Debugging、MonoDevelop.Debugger、MonoDevelop.DesignerSupport、MonoDevelop.Projects.Formats.MSBuild、MonoDevelop.SourceEditor2、WindowsPlatform。 |
| 自动发现的内置模块 | 6 | Addins.LuaExtend、CocoStudio.LuaBinding、CocoStudio.SourceEditor、CocoStudio.WindowsPlatform、Modules.Animation、MonoDevelop.Refactoring；均带 Addin/AddinRoot 属性，对应标识存在于注册扫描记录中。 |
| 动态加载的扫描器 | 1 | Mono.Addins.CecilReflector；Mono.Addins 的 GetReflectorForFile / OnResolveAddinAssembly 中存在加载它的程序集名字符串。 |

源码 `CocoStudio.Core/Service/Runtime.cs` 的 InitializeAddins 调用 AddinManager.Initialize、
Registry.Update，并检查显示构建器扩展。这是编辑器内置模块发现机制，不能因为不使用在线
插件安装/更新功能就删除上述 DLL。

本轮没有删除 DLL、替换包或改动业务代码，也没有重新编译；完整依赖关系和审计脚本
保存在被忽略的 `obj/NuGetPhase10Audit/`。该检查证明存在依赖或模块发现依据，
不是全部功能的动态覆盖测试，也不排除未来经源码重构后移除部分依赖的可能。

目前未发现下一批可直接安全替换或删除的库。继续推进剩余依赖需单独进行旧 MonoDevelop
调用方的源码/API 适配或隔离；GTK 无安装依赖分发则是另一项原生运行库部署工作。

## 第十一批：MonoDevelop 上游源码重建验证（未替换）

已找到并下载官方 `mono/monodevelop` 的 `monodevelop-5.4.0.240` 标签，
固定提交 `01786bc67c7024ec33d327ed27e4416d7a846f4e`。
这是本轮选定的对照基线，不代表已经确定现有定制 DLL 的精确原始提交。
上游 AssemblyInfo 同样使用程序集版本 2.6；本地模块依赖版本为 5.4.0，
两种版本号并不矛盾，不能按 DLL 的 2.6.0.0 去找 MonoDevelop 2.6 源码。

隔离项目直接引用上游 csproj 列出的源码/资源，以及第九批输出中的依赖，
使用现有旧 Mono.Debugging 先验证基线可构建性：

- SourceEditor2：70 个源码项、31 个资源项，重建 0 错误、36 警告。
- Debugger：48 个源码项、54 个资源项，首次出现 1 个编译错误：上游使用
  `StatusBarIcon.EventBox.ButtonPressEvent`，本地 MonoDevelop.Ide 只有 `Clicked` 事件。
  对照本地 Debugger IL，确认其本来就订阅 `Clicked`；仅在隔离上游副本恢复这项适配后，
  重建 0 错误、20 警告。
- 现有输出引用这两个程序集的 82 处成员引用能解析到重建候选；这只是静态引用验证，
  不代表完整行为、内部扩展权限和资源注册都等价。

已确认不能直接以纯上游二进制替代本地定制版本：

- 本地 SourceEditor2 有 `InternalsVisibleTo("CocoStudio.SourceEditor")`，上游只有 UnitTests。
- 本地 SourceEditorView 增加了受保护虚方法 `ProcessSaveText`、`ProcessLoadText`、
  `PrepareToSetCaret`，以及 OwnerDocument、页面跳转、ShowMap/ShowFull 等成员，
  当前对照上游没有这些成员。
- `CocoStudio.SourceEditor.TextEditorView` 实际继承该 SourceEditorView。
- 本地 Core / Ide 同样包含授予 Cocos 程序集内部访问权限的定制属性。

本轮没有修改现用 DLL、主方案引用或升级 Mono.Debugging；没有运行重建候选的界面、
调试会话或主程序。上游源码、隔离 csproj、候选 DLL 和日志均在被忽略的
`obj/NuGetPhase11Audit/`，仅用于比对，不是可发布的替换产物。
下一步需逐项恢复并验证定制接口/行为，再处理 Mono.Debugging 的事件与断点持久化接口，
不能只以“编译通过”作为替换条件。

上游证据：[Debugger 源码目录](https://github.com/mono/monodevelop/tree/monodevelop-5.4.0.240/main/src/addins/MonoDevelop.Debugger)、
[SourceEditor2 程序集属性](https://github.com/mono/monodevelop/blob/01786bc67c7024ec33d327ed27e4416d7a846f4e/main/src/addins/MonoDevelop.SourceEditor2/AssemblyInfo.cs)。

## 第十二批：定制库反编译源码重建与 NuGet 候选（未替换）

从当前 `dlls` 中反编译导出的 Debugger、SourceEditor2 建立隔离 .NET Framework 4.8 项目，
178 个 C# 文件与原始导出一致，无需手工删改定制源码即可重建。
先验证旧依赖基线，再通过 PackageReference 接入 `Mono.Debugging 1.0.20170212.42`：
Debugger 0 错误、18 警告；SourceEditor2 0 错误、34 警告，后者引用本批重建的 Debugger。

事件委托差异可由方法组重新编译适配；新版 BreakpointStore.Save / Load 增加可选 baseDir 参数，
现有源码调用重新编译后传 null，无须改用其他存储格式。新旧 Mono.Debugging 程序集版本均为
0.0.0.0，本批不增加绑定重定向，不使用 DEVPATH，不修改系统配置。

验证结果：

- 两库 1125 + 1406 项接口表面检查无差异；明确忽略了五个无字段且无静态构造函数的接口的
  BeforeFieldInit 编译器标志差异。定制钩子、OwnerDocument、Addin 属性和内部访问授权保留。
- 85 项嵌入资源名称、属性与字节哈希一致。
- 现有输出及候选的 351 处相关成员引用均解析到候选，未发现缺失成员。
- 独立 x86 进程验证新旧断点数据双向读取、保存/读取往返；样本覆盖中文路径、条件表达式、
  行列、禁用状态和异常断点。
- 候选库及上层 CocoStudio.SourceEditor 类型加载、继承关系和定制接口检查通过。
- 模拟会话的 TargetExited 订阅、触发、退出状态和取消订阅通过；不涉及真实调试进程。

源码副本、项目、重复构建/审计/测试脚本及日志在被忽略的 `obj/NuGetPhase12Audit/`，
详见其 README。原始反编译导出保持不变，现用四个 MonoDevelop DLL 哈希未改变。
本轮未替换正式 DLL、修改主方案引用、删除 DLL 或提交。Core / Ide 仍为原定制二进制。
尚需正式源码接入、主方案独立输出验证、完整编辑器交互和真实调试回归，不能据本轮有限测试发布候选。

## 第十三批：恢复源码纳入维护结构并接入主方案测试构建

已将 Debugger / SourceEditor2 的 178 个反编译 C# 文件和 85 项资源放入 `third-party/MonoDevelop/`，
内容未改写；新增来源哈希、上游对照版本及其 111 个文件的版权/许可头注释记录。
项目不再依赖本机 `obj` 实验目录或第九批输出，详细边界见该目录 README。

采用默认关闭的 `UseRestoredMonoDevelop` 开关，主程序条件引用 SourceEditor2，后者引用 Debugger。
统一测试属性将输出固定在 `bin/RestoredMonoDevelopTest/`，中间文件另行隔离；恢复项目直接构建、
未启用测试模式或试图写入普通输出都会被拦截。普通构建继续复制原三份 DLL，已用哈希验证。

接入过程中发现根 ReferencePath 的旧 DLL 查找顺序早于 HintPath，即使写了包路径，也可能编译成旧 API。
已将包目录优先级显式提前，并防止旧 Debugger / SourceEditor2 被间接引用复制覆盖。
构建脚本同时验证两份库与本次编译产物相同、Mono.Debugging 与 NuGet 包相同；
审计脚本也先验证 NuGet DLL 哈希，防止把全旧或混合输出误判成新版本兼容。

验证结果：

- `.sln` 和 `.slnx` Debug/x86 重建均为 0 错误、160 警告。
- 351 处成员引用、85 项资源、接口及授权属性检查通过。
- 新旧断点样本双向读写、上层编辑器类型加载、模拟 TargetExited 事件测试通过。
- 包含应用程序集的旧/新模块注册扫描各为 47 项，清单一致，均使用测试专属缓存。
- Gtk# 默认/GAC 和强制本地候选两种加载模式的基础控件、图片、Cairo 测试通过。
- 未启用开关的构建复制检查仍得到原三库；恢复项目未授权测试模式时会失败退出。

可重复入口为 `tests/Build-RestoredMonoDevelop.ps1`、`tests/Audit-RestoredMonoDevelop.ps1`、
`tests/Test-RestoredMonoDevelop.ps1`，以及已有 Gtk# 测试。构建与测试不会主动启动编辑器。
测试程序路径为 `bin/RestoredMonoDevelopTest/CocosStudio.exe`；完整编辑器可能仍使用用户配置，
人工运行前应保存当前工作、使用工程副本并备份配置，输出隔离不等于用户数据隔离。

本轮未删除原 DLL、切换默认构建、修改正式配置或提交；完整界面及真实调试仍待人工回归，
不应将本批标记为已完成正式替换。当前主方案测试输出也包含工作区已有的其他未提交修改。

## 第十四批：Windows 适配层恢复与真实编辑器控件测试

实际执行平台初始化后发现原 `CocoStudio.WindowsPlatform.dll` 的浏览器适配器缺少新版 Xwt 接口实现，
导致平台和字体初始化失败。换回旧 Debugger / SourceEditor2 仍可复现，属于现有 Xwt 升级的兼容性缺口。

将该库的 4 个 C# 文件和 1 项资源恢复至 `src/Platforms/CocoStudio.WindowsPlatform/`，
仅修改浏览器适配器，补齐菜单、滚动条、背景和自定义 CSS 四个属性；保留原引擎、已有接口及模块资源。
源码项目加入同一默认关闭的候选构建，普通构建仍复制原 DLL。来源哈希、具体实现及测试边界见该目录上级 README。

验证结果：

- `.sln` / `.slnx` Debug/x86 重建均为 0 错误、163 警告，三份源码产物与候选输出哈希一致。
- 原两库接口、351 处成员引用和 85 项资源审计通过；Windows 适配层另验证原 72 项接口及 1 项资源，
  只允许预期的 4 个属性和 8 个访问器新增。
- 新增 `tests/Test-RestoredMonoDevelopEditor.ps1`：在独立进程中完成实际平台初始化、SourceEditorView 创建，
  验证加载/保存/光标定制钩子、插入/删除/撤销/重做、中文路径、UTF-8 BOM 和换行保留、控件释放。
- 同一修复后平台下，旧/新 MonoDevelop 编辑器两种模式均通过；WebBrowser 仅加载本地 HTML，设置与 CSS 更新/清除通过。
- 原断点双向持久化、模拟退出事件、47 项模块扫描和 Gtk# 基础测试继续通过。
- 默认复制检查得到四份原 DLL；原 DLL 未删除，普通输出、现用编辑器和真实用户配置未修改。

测试用临时文件、独立模块缓存和用户配置；不启动完整主程序。控件测试不替代完整界面、GTK 浏览器嵌入、
真实调试和干净机器回归。未切换默认版本、未提交，当前候选输出仍包含工作区已有的其他修改。

## 第十五批：按用户要求切换默认源码构建

`Directory.Build.props` 现默认启用 `UseRestoredMonoDevelop`。普通 Debug/Release 构建会从源码生成
Debugger / SourceEditor2 / CocoStudio.WindowsPlatform，并使用 NuGet Mono.Debugging；Core / Ide 等未恢复模块仍为原 DLL。
保留独立候选构建入口，其输出路径检查只在隔离测试模式下生效。

`tests/Build-RestoredMonoDevelop.ps1 -DefaultOutput` 可重建并校验 `bin/Debug`，不传该选项仍写入隔离目录。
脚本会检查目标目录是否有正在运行的 CocosStudio，不主动关闭进程。

本轮普通 `.sln` Debug/x86 重建为 0 错误、163 警告。已核对 `bin/Debug` 三份源码 DLL 与本次编译产物哈希一致，
Mono.Debugging 与指定 NuGet 包一致；针对该输出执行的接口/资源审计、断点双向互读、模拟退出、47 项模块扫描、
编辑器加载/保存/光标/撤销重做/编码换行及本地 HTML 设置测试全部通过。
Release 仅检查默认属性，未编译。没有启动完整编辑器，界面和真实调试仍由用户试用确认。

原 DLL 暂不删除。显式传 `/p:UseRestoredMonoDevelop=false` 可让主方案回到原四库（需还原包、完整重建，
不可只覆盖单个 DLL）；这不回退前批 Xwt 等升级，因此仍有原 Windows 适配层的已知兼容性缺口。
未提交、未推送；其他工作区修改保持原样，正常重建产物包含这些已有修改。

## 第十六批：修复 Visual Studio 还原后 Gtk# 配置路径为空

用户在 VS 重建时报告 `Modules.Communal.StartAutoRecover` 无法复制 `D:\build\atk-sharp.dll.config` 等文件。
检查当时的 `obj/CocoStudio.Core/project.assets.json` 与生成的 NuGet props：
仅有项目直接声明的包，没有集中 targets 注入的 Mono.GtkSharp / Mono.Debugging，两个 Pkg 路径属性均为空。
Gtk# 配置的 `\build\...` 因而被解析到当前驱动器根目录；StartAutoRecover 是收集依赖项目复制项时暴露问题。
之前的命令行还原/构建通过，不能证明 VS 重新还原后仍有效。

将原先集中注入的包声明移入每个旧格式 csproj：55 个项目显式声明条件启用的 Mono.Debugging，
46 个已有 Gtk# 引用的项目显式声明 Mono.GtkSharp 及 Mono.Posix-4.5（已有 Posix 声明不重复添加）。
版本、ExcludeAssets、PrivateAssets 和 GeneratePathProperty 保持不变；集中 targets 继续负责程序集和配置复制规则。
PkgMono_GtkSharp 为空时不生成根目录复制项，并在解析引用或收集间接复制项时给出明确的 NuGet 还原错误。
同时检查包内配置文件确实存在，不能通过跳过复制来掩盖缺失依赖。

本轮普通 Debug/x86 还原重建通过（0 错误、163 警告），源码 DLL 和 NuGet 产物哈希校验通过，编辑器行为测试通过。
新增 `tests/Test-NuGetProjectDeclarations.ps1` 检查直接包声明、VS 风格构建属性下 StartAutoRecover 的 8 项间接复制路径，
以及人为置空包路径时的提前失败；三项均通过。未操作实际 VS 界面，不等同于 IDE 端重新还原验证。
用户需重新加载方案、还原 NuGet 包并重建，以刷新旧项目系统的已加载项目状态。未提交、未推送。

## 第十七批：恢复上层 CocoStudio.SourceEditor

本批不强换另一套 NuGet 库，而是补齐已恢复的底层 SourceEditor2 之上的应用封装源码。
`CocoStudio.SourceEditor.dll` 原 SHA-256 为 `BBAD404B771F239C7117E85176433596CF109C38C9CF09062E6EE419937E112B`。
使用相同 ILSpy 工具导出 3 个业务类及 AssemblyInfo，纳入 `src/Editor/CocoStudio.SourceEditor/`；
仅规范文本换行，未改写逻辑、程序集标识或原注释/属性。原始导出保存在被忽略的 `obj/NuGetPhase17Audit/`。

主程序现在依次通过 ProjectReference 构建 CocoStudio.SourceEditor → MonoDevelop.SourceEditor2 → MonoDevelop.Debugger。
新项目保留显式 NuGet 声明，并引用现有 Core / Projects / Basic 源码项目；补充基类所需 DesignerSupport 编译引用。
默认和独立测试输出都排除旧上层 DLL 的统一/间接复制，并校验它确实来自本次源码编译。

验证：

- `.sln` 隔离 Debug/x86 与 `.slnx` 普通 Debug/x86 重建均为 0 错误、164 警告；四份恢复库产物哈希一致。
- 上层 36 项接口与原 DLL 一致，AddinRoot、依赖和类型 Extension 注册属性保留；原 351 处依赖引用审计继续通过。
- 上层工厂创建的真实 TextEditorView：大小写 Lua 文件识别、非 Lua 拒绝、加载/保存/重新加载、光标、撤销/重做、
  文件视图复用规则测试通过。新旧上层/底层组合在各自独立进程下均通过；普通 bin/Debug 输出复测通过。
- 断点双向互读、模拟退出事件、47 项模块扫描继续通过；56 个项目直接包声明检查通过，其中 47 个引用 Gtk#。
- 显式关闭源码开关时，复制得到原五库，已核对哈希。原 DLL 暂不删除。

测试最初在底层编辑操作之后才初始化上层，初始化会禁用并等待自动保存线程，导致无主事件循环的测试进程超时。
调整为先初始化上层绑定再打开编辑器后通过，没有据此改动产品代码。测试使用临时文件和独立配置，未启动完整编辑器。
本批默认 `bin/Debug` 已更新，未提交、未推送；真实调试与完整工作台交互仍待用户回归。

## 第十八批：MSBuild 桥接源码恢复及 Windows 元数据读取修复

恢复 `MonoDevelop.Projects.Formats.MSBuild` 的 17 个 C# 文件至 `third-party/MonoDevelop/`，
原 DLL SHA-256 为 `910A447AB3D71F9B6B9EFF7C715BD758F4ADF797E587B77989EF229FEE21A72D`。
默认源码构建与测试输出均接入，原库及显式回退仍保留；这不是把旧 MSBuild 引擎升级为现代版本。

实际测试发现原 ProjectBuilder 依赖 Mono 私有 `BuildItem.evaluatedMetadata` 字段，Windows 实现不存在它，
读取项目条目时出现空引用异常。仅修改该类，改为公开 CustomMetadataNames / GetEvaluatedMetadata，
同时避免重复反转义；其他 16 个源码文件规范换行后与原导出一致。公开接口、原属性和注释保留。
API 说明见 [Microsoft BuildItem](https://learn.microsoft.com/en-us/dotnet/api/microsoft.build.buildengine.builditem)。

验证结果：

- `.sln` 独立输出与 `.slnx` 默认 Debug/x86 重建均为 0 错误、174 警告；五份源码产物与输出哈希一致。
- 该库 166 项接口无差异、29 处调用引用解析通过；连同此前库共检查 380 处成员引用。
- 新增 `tests/Test-RestoredMSBuild.ps1`，通过跨 AppDomain 的 IBuildEngine / IProjectBuilder 验证项目求值、
  自定义元数据与转义、目标执行、日志回调、警告/错误、未保存/磁盘刷新、非法 XML、卸载和结果序列化。
  旧库的条目读取失败作为基线缺陷明确复现，新库通过回归，不能将旧模式退出码 0 解释为旧库无缺陷。
- 57 个项目显式包声明检查、默认输出接口审计及现有编辑器/断点/模块注册回归通过。
- 关闭源码构建后，六份原始 DLL 的回退复制哈希检查通过。

本批另检查了 `WindowsPlatform.dll`：除旧 Code Pack 程序集身份外，还使用 IFileDialogCustomize、
内部 Attach/SyncUnmanagedProperties 以及 customize/nativeDialog 等内部成员，不能直接重建后强行使用新版包。
其原始反编译结果仅留在忽略的 `obj/NuGetPhase18Audit/WindowsPlatform`，本轮未替换或修改它。

默认 `bin/Debug` 已更新；完整 IDE、独立构建进程启动协议、真实工程编译及其他平台尚未验证。
未提交、未推送；既有资源定位修改及三个 .sln 删除记录保持不动。

## 第十九批：恢复 CocoStudio.LuaBinding

恢复 `CocoStudio.LuaBinding.dll` 的 23 个 C# 文件及 8 个资源到
`src/Editor/CocoStudio.LuaBinding/`，原 DLL SHA-256 为
`DE22BC4A54EBDC0A4671F8D20ADC41245283F53E54538E5B3886E593B67278AD`。
原始导出保存在忽略的 `obj/NuGetPhase19Audit/LuaBinding`；所有源码仅规范换行，未改写业务逻辑。
资源按字节保留，程序集版本 `0.0.0.0`、Addin 属性和 XML 注册不变。

该库通过主程序条件 ProjectReference 构建，依赖使用已有 NuGet 版本，包含旧项目 VS 还原所需显式包声明。
默认/隔离输出排除旧 DLL 的统一及间接复制；原 DLL 保留，不是替换为通用 Lua 包或升级 Lua 语言版本。

验证：

- `.sln` 隔离输出与 `.slnx` 默认 Debug/x86 均为 0 错误、174 警告；六份源码 DLL 输出哈希通过。
- 本库 139 项接口、8 项资源与原库一致；现有 380 处调用引用解析审计通过。
- 新旧独立进程验证 Lua 文件识别、注释标记、可选参数、提示签名、中文路径错误解析、两份语法资源及内置补全数据。
- 58 个项目显式包声明检查通过，其中 48 个使用 Gtk#；StartAutoRecover 的 8 项配置复制检查通过。
- 原编辑器、断点持久化、模块注册与 MSBuild 桥接回归通过；七份原 DLL 的回退复制哈希通过。

对照测试发现原 `LuaParameterDataProvider.Unpack` 对嵌套可选参数丢失逗号，
`list [, i [, j]]` 实际展开为 `list|list i|list i, j`；新旧均如此，本批作为已知缺陷保留并单独标记。
`LuaParser.GetLocals` 原本就是空实现，不据此推断 LuaTextEditorCompletion 中的其他局部变量处理无效。
没有实际运行 Lua/luac、完整工作台补全窗口或真实 Lua 工程编译。

默认 `bin/Debug` 已更新。未提交、未推送；未改动其他功能修改及三个已有 .sln 删除记录。

## 第二十批：恢复 MonoDevelop.DesignerSupport

恢复 85 个 C# 文件与 10 项资源至 `third-party/MonoDevelop/MonoDevelop.DesignerSupport/`，
原 DLL SHA-256 为 `37058746AD28C9A6D0617DCBDF01BD317BF1E55548CA8B19549F2F0D6974D498`。
原始导出沿用 `obj/NuGetPhase19Audit/DesignerSupport`；源码仅规范换行，未改写业务逻辑、属性或原注释。
版本 `2.6.0.0`、模块注册及所有资源逻辑名称保持不变。

SourceEditor2 和 CocoStudio.SourceEditor 直接引用该源码项目；依赖使用已有 NuGet 版本，补齐 System.Drawing 等框架引用。
默认/隔离构建排除旧 DLL 的统一及间接复制，显式关闭源码模式仍复制原库。

验证：

- 隔离 `.sln` 编译 0 错误、183 警告；默认 `.slnx` 编译及完整输出验证 0 错误、179 警告。
- 883 项接口、10 项资源无差异，17 处调用引用解析到候选；全部恢复库共检查 397 处引用、104 项资源。
- 新旧独立进程验证属性只读标记、取值/重置、事件订阅解绑、文本工具箱筛选/预览/相等性、类型加载与转换。
- 59 个项目直接包声明检查通过，其中 49 个使用 Gtk#；StartAutoRecover 的 8 项间接配置复制路径通过。
- 既有编辑器、Lua、断点、模块注册、MSBuild 桥接回归及八份原 DLL 的回退复制哈希检查通过。

隔离还原曾因 NuGet 服务不可达产生中文 NU1900；Windows PowerShell 默认 ANSI 读取 UTF-8 assets 导致编译后的验证失败。
修正脚本为显式 UTF-8，使用原警告文件验证解析及包哈希成功；后续默认构建完整通过，未再出现 NU1900，未禁用漏洞审计。
一次隔离编辑器测试退出有 GDK clipboard assertion 警告，测试断言和退出码通过；剪贴板真实交互未验证。
完整设计器 UI、外部组件加载及远程设计进程尚未验证，没有据此升级或重写原有序列化逻辑。

默认 `bin/Debug` 已更新。原 DLL 保留，未提交、未推送；其他已有修改与三个 .sln 删除记录保持原样。

## 第二十二批：恢复 Refactoring 并同批删除原 DLL

恢复 `MonoDevelop.Refactoring` 的 126 个 C# 文件和 8 项资源，原始导出在忽略的 `obj/NuGetPhase22Audit/Refactoring`。
所有源码仅规范换行，版本 `2.6.0.0`、模块属性和资源逻辑名称保持原样；未改写业务逻辑。
原 DLL SHA-256 为 `D61254DA8795752ECEA8C6216AFD90CD7B4FFEA9E0B2571A2F833D9E4CF847FD`。
主程序通过 ProjectReference 构建它，依赖已恢复的 SourceEditor2 / DesignerSupport / Debugger 及现有 NuGet 包。
旧 DLL 已删除；对照基准同样来自固定 Git 提交，不参与生产构建。

- 1,187 项接口与 8 个资源无差异；全量审计为 397 处成员引用、112 个资源。
- 新旧独立进程验证内存及真实已加载编辑器中的替换、光标、撤销/重做、保存、BOM/CRLF、参数拒绝、问题列表可见性。
- 测试最初直接执行未打开文件替换，因没有完整 Workbench 而触发旧 TextFileProvider 空引用；改为注入真实已加载编辑器，未修改产品代码。
- 没有验证完整工作台的未打开文件发现、跨工程重命名、批量修复或实际调试，不能将本轮回归当作完整重构功能验收。
- 删除后的全新 `.sln` Debug 与默认 `.slnx` Debug 构建为 0 错误、206 警告；全新 `.slnx` Release 为 0 错误、205 警告。

默认 `bin/Debug` 已更新；继续保留内置模块发现和现有平台支持，不改 Lua 的已知功能缺陷。
