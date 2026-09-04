# DLL / NuGet 迁移记录

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
`CocosStudio.sln` 和 `CocosStudio.slnx` 都不再构建旧 NRefactory 源码项目，
避免同名输出覆盖 NuGet DLL；源码目录仍保留。

## 第八批清理

删除 `dlls/Modules.Communal.MutualEditor.dll` 的旧预编译副本。
四处项目依赖均为 ProjectReference；全局复制规则本就排除此同名源码项目的 DLL。
模块功能、源码和项目引用不变，输出继续来自源码编译。

## 暂不能直接替换的候选库

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
