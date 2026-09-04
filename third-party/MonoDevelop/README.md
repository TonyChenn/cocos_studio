# MonoDevelop 定制库恢复源码（默认源码构建）

本目录包含 MonoDevelop 底层定制库与 Cocos Studio 上层适配库，不是直接使用 MonoDevelop 官方源码替换定制版本。
Debugger / SourceEditor2 两个目录的 178 个 C# 文件与 85 项资源，来自当前仓库 DLL 的反编译导出，内容未改写。
另恢复 CocoStudio.WindowsPlatform 的 4 个 C# 文件和 1 项资源，并为其浏览器适配器补齐 Xwt 接口，详见下文。
同时恢复 CocoStudio.SourceEditor 的 3 个业务类及 AssemblyInfo，共 4 个 C# 文件。
另恢复 MSBuild 构建桥接库的 17 个 C# 文件，并修复 Windows 上的项目条目元数据读取。
另恢复 CocoStudio.LuaBinding 的 23 个 C# 文件及 8 项资源；本批不改写 Lua 业务逻辑。
另恢复 MonoDevelop.DesignerSupport 的 85 个 C# 文件及 10 项资源，源码仅规范换行。

## 来源

| 原 DLL | SHA-256 |
| --- | --- |
| `dlls/MonoDevelop.Debugger.dll` | `F18016A71BB89405759658319E12AA9A4B35E2894E9A78088190285A599CCF02` |
| `dlls/MonoDevelop.SourceEditor2.dll` | `B4718C10AA8A14FDDA59F65E9D48FD192211112A51D662E6F3ECB71890745836` |
| `dlls/CocoStudio.WindowsPlatform.dll` | `1C2B4E83C1B997EF584D01D9077384A5535F97CE21D424628321BB65E8DA8AA8` |
| `dlls/CocoStudio.SourceEditor.dll` | `BBAD404B771F239C7117E85176433596CF109C38C9CF09062E6EE419937E112B` |
| `dlls/MonoDevelop.Projects.Formats.MSBuild.dll` | `910A447AB3D71F9B6B9EFF7C715BD758F4ADF797E587B77989EF229FEE21A72D` |
| `dlls/CocoStudio.LuaBinding.dll` | `DE22BC4A54EBDC0A4671F8D20ADC41245283F53E54538E5B3886E593B67278AD` |
| `dlls/MonoDevelop.DesignerSupport.dll` | `37058746AD28C9A6D0617DCBDF01BD317BF1E55548CA8B19549F2F0D6974D498` |

- 反编译工具：ILSpy `ilspycmd 11.0.0.9375`，项目模式，C# 7.3，分层命名空间目录。
- 原始导出在本机被忽略的 `obj/DecompiledMonoDevelop`；本目录本身不依赖该目录或第九批构建输出。
  WindowsPlatform 的原始导出另存于 `obj/NuGetPhase14Audit/CocoStudio.WindowsPlatform`。
  它是 Cocos Studio 自有适配层，不是 MonoDevelop 上游模块；保留 DLL 内原有版权及模块注册属性。
  CocoStudio.SourceEditor 原始导出位于 `obj/NuGetPhase17Audit/CocoStudio.SourceEditor`，恢复内容仅规范换行，未改写逻辑。
  AssemblyInfo 中原有 Microsoft 2015 等标记照录，不以此推定原始代码的实际权属。
  MSBuild 桥接库原始导出在 `obj/NuGetPhase18Audit/MSBuild`；除 ProjectBuilder 的元数据修复外，源码仅规范换行。
  LuaBinding 原始导出在 `obj/NuGetPhase19Audit/LuaBinding`；23 个源码文件仅规范换行，资源按原字节保留。
  DesignerSupport 原始导出沿用 `obj/NuGetPhase19Audit/DesignerSupport`；85 个源码文件仅规范换行，10 项资源按原字节保留。
- 上游对照基线：[mono/monodevelop](https://github.com/mono/monodevelop/tree/01786bc67c7024ec33d327ed27e4416d7a846f4e)，
  标签 `monodevelop-5.4.0.240`，提交 `01786bc67c7024ec33d327ed27e4416d7a846f4e`。
  尚未确定现有定制 DLL 的精确原始提交，不能将这套恢复代码标成未经修改的官方源码。
- `UPSTREAM-REFERENCE-NOTICES.txt` 保留该对照版本两个模块 111 个源文件的原始版权/许可头注释。
  原程序集中的版权、模块注册及内部访问授权属性原样保留。此处不重新声明第三方代码的权属或许可。
  原始注释和所有定制历史不能从 DLL 完整恢复，发布前仍需核对原始来源及所需声明。

## LuaBinding 恢复与验证范围

`CocoStudio.LuaBinding` 保留程序集版本 `0.0.0.0`、原 Addin 属性与 XML 注册信息，依赖继续使用现有
Mono.Addins / NRefactory / Gtk# / Mono.Posix 包。139 项接口和 8 个资源与原库一致；没有借此升级 Lua 语言版本。
`Test-RestoredMonoDevelopEditor.ps1` 在新旧独立进程中验证文件识别、注释标记、可选参数展开、提示签名、
中文 Windows 路径的编译错误解析、两份语法资源加载及 151 项内置补全数据；同时继续执行原编辑器回归。

基线存在但本批不修改的问题：`LuaParser.GetLocals` 是空实现（不表示其他补全路径都无局部变量处理）；
`LuaParameterDataProvider.Unpack("list [, i [, j]]")` 会得到 `list|list i|list i, j`，嵌套参数丢失逗号。
测试明确标记后者为已知缺陷，新旧行为一致不等于功能无缺陷。没有实际启动 Lua/luac、验证弹出的补全窗口、
完整工作台注册路径或真实 Lua 工程编译；这些仍需运行时回归。

## 接入方式与隔离

- 项目文件名使用 `.Restored.csproj`，程序集名称保留原名，现包含 Debugger / SourceEditor2 / DesignerSupport / CocoStudio.WindowsPlatform / CocoStudio.SourceEditor / MonoDevelop.Projects.Formats.MSBuild / CocoStudio.LuaBinding 七库。
  文件名刻意不同，避免根构建按项目文件名扫描时，自动停止复制回退构建所需的旧 DLL。
- 按用户要求，`UseRestoredMonoDevelop` 现默认启用，普通 Debug/Release 构建使用这七份恢复源码及新版 Mono.Debugging。
  `build/RestoredMonoDevelop.Test.props` 仍可用于隔离测试，将输出限定为
  `bin/RestoredMonoDevelopTest/`，中间文件限定为 `obj/RestoredMonoDevelopTest/`。
- 主程序通过条件 ProjectReference 引用 CocoStudio.SourceEditor，后者引用 SourceEditor2，再引用 Debugger；`.sln` 和 `.slnx` 都沿此依赖构建，
  无需在方案中重复添加恢复项目。
  WindowsPlatform 也由主程序条件引用；其应用层依赖通过 ProjectReference 接入。
  MSBuild 构建桥接库同样由主程序条件引用，保留 .NET Framework 自带的旧 MSBuild 4.0 API，不是升级构建引擎。
  LuaBinding 也由主程序条件引用，模块注册与嵌入资源保持原样。
  SourceEditor2 与 CocoStudio.SourceEditor 通过 ProjectReference 引用 DesignerSupport，不再直接引用旧 DLL。
- 源码模式下所有项目使用同一个 NuGet `Mono.Debugging 1.0.20170212.42`；NRefactory、Mono.Addins、Xwt、Gtk# 等
  继续采用现有 NuGet 版本。Core、Ide、Mono.TextEditor 暂时引用仓库原 DLL。
  包声明直接保存在各 csproj 中，避免旧格式项目在 VS 还原时漏掉集中 targets 注入的包。
- 根 ReferencePath 的旧 DLL 搜索顺序早于 HintPath，因此源码构建目标明确把 Mono.Debugging 包目录放在前面。
  同时排除旧八库的统一复制及旧七份源码库的间接复制，最后核对实际输出哈希。
- 普通构建使用 `bin/Debug` 或 `bin/Release`；启用隔离测试模式时，构建前和清理前仍验证测试输出路径。
- `dlls` 中八份原库继续保留。需要回退时，对主方案传 `/p:UseRestoredMonoDevelop=false` 并还原包、完整重建，
  不要只覆盖部分 DLL，以免新旧依赖混用。该回退保留原 Windows 适配层的已知 Xwt 不兼容问题，不是完整依赖回滚。
  未改变 GAC、用户环境或现用编辑器进程。

## 构建和自动测试

从仓库根目录运行（Visual Studio MSBuild，.NET Framework 4.8 开发组件，原生 GTK 2 环境）：

```powershell
# 默认输出：构建并校验 bin/Debug，运行前先关闭该目录下的编辑器。
.\tests\Build-RestoredMonoDevelop.ps1 -DefaultOutput
.\tests\Test-NuGetProjectDeclarations.ps1
.\tests\Audit-RestoredMonoDevelop.ps1 -CandidateDirectory .\bin\Debug
.\tests\Test-RestoredMonoDevelop.ps1 -OutputDirectory .\bin\Debug
.\tests\Test-RestoredMonoDevelopEditor.ps1 -OutputDirectory .\bin\Debug
.\tests\Test-RestoredMSBuild.ps1 -OutputDirectory .\bin\Debug
# 以下保留独立输出测试入口：
.\tests\Build-RestoredMonoDevelop.ps1
# 若要检查新版方案格式：
.\tests\Build-RestoredMonoDevelop.ps1 -SolutionFormat slnx
.\tests\Audit-RestoredMonoDevelop.ps1
.\tests\Test-RestoredMonoDevelop.ps1
.\tests\Test-GtkSharp.ps1 -OutputDirectory .\bin\RestoredMonoDevelopTest
.\tests\Test-RestoredMonoDevelopEditor.ps1
.\tests\Test-RestoredMonoDevelopEditor.ps1 -Mode old
```

构建脚本自动查找 Visual Studio，也可用 `-MSBuild` 指定路径；不启动编辑器、不提交代码。
更新这些项目文件后，VS 中应重新加载方案并还原 NuGet 包，再重新生成；仅命令行还原成功不能证明 IDE 缓存已刷新。
构建日志在 `obj/RestoredMonoDevelopBuild/`；测试在独立的 `obj/RestoredMonoDevelopSmoke-*` 下运行，
不会使用用户的模块注册缓存或工程文件。Gtk# 测试沿用现有测试脚本的进程级 DEVPATH 隔离；
MonoDevelop 测试不使用 DEVPATH，也不添加 Mono.Debugging 绑定重定向。

先完成构建，再运行测试；不要同时重建和复制同一候选输出。

当前验证：隔离 `.sln` 为 0 错误、183 警告，默认 `.slnx` 为 0 错误、179 警告；输出的七份恢复库与各自编译产物哈希一致，
Mono.Debugging 与指定 NuGet 包一致。397 处成员引用、104 项资源、定制接口、断点样本双向互读、
模拟退出事件、上层类型加载通过。包含应用程序集的旧/新模块扫描各为 47 项，清单相同。
Gtk# 默认加载与强制本地候选加载的基础控件/图像测试也通过。

## DesignerSupport 恢复与验证范围

本库保留程序集版本 `2.6.0.0`、Addin 注册和全部资源逻辑名称；源码未改写，项目显式引用
System.Drawing / System.Design 等框架程序集及现有版本的 Cecil / Xwt / NRefactory / Mono.Addins / Gtk# / Mono.Posix 包。
883 项接口无差异，17 处调用引用解析到实际候选。新旧独立进程覆盖 CustomDescriptor/CustomProperty 的只读标记、
取值、重置、事件订阅解绑，以及文本工具箱筛选、预览、相等性和 TypeReference 加载/转换。
完整设计器面板、实际拖入工具箱、外部组件加载及远程设计进程尚未验证；仍保留原有序列化与加载实现。

首次构建补齐 System.Drawing 显式引用。随后隔离编译成功，但 NuGet 服务暂时不可达所产生的中文 NU1900 警告
暴露了 Windows PowerShell 默认 ANSI 读取无 BOM UTF-8 JSON 的问题；构建脚本已显式用 UTF-8 读取 assets。
含中文警告的原 assets 解析和包哈希回归通过，后续默认构建完整通过且未再出现 NU1900；没有关闭漏洞审计。
一次隔离编辑器测试退出时出现 GDK clipboard assertion 警告，断言测试与退出码仍通过；不以此证明剪贴板交互无问题。

## Windows 平台适配与编辑器行为测试

沿实际 `CocoStudio.Core.PlatformAdapter.Initialize()` 初始化路径发现：原 `CocoStudio.WindowsPlatform.dll`
的 `CSWindowWebViewBackend` 未实现 Xwt 0.2.251 的 `ContextMenuEnabled` 等新增接口，平台初始化失败，
字体服务随之未初始化。换回原 Debugger/SourceEditor2 仍能复现，因此不是这两个恢复库引入的问题。

恢复适配层后补齐 `ContextMenuEnabled`、`ScrollBarsEnabled`、`DrawsBackground`、`CustomCss`。
菜单沿用已有 `IsMenuEnabled` 状态；设置可在原生控件创建前保留，菜单/滚动条在创建时应用，CSS 在文档完成加载时应用。
背景透明通过本地 HTML 样式实现；不代表原生 WinForms 控件或 GTK 嵌入窗口支持透明合成。
没有更换原 WebBrowser 引擎。其余三个 C# 文件和模块资源未改写；接口审计仅允许四个属性及八个访问器的新增，
原 72 项接口、资源字节和模块注册属性均保留。

新增编辑器测试运行在独立 x86 子进程，使用临时 Lua 文件、模块缓存、日志和 `MONODEVELOP_PROFILE`。
提前创建空偏好文件，禁止迁移真实用户配置；不运行完整主程序、不连接真实调试目标。
调用实际平台初始化和 SourceEditorView：加载/保存/光标三个定制钩子，插入/删除、撤销/重做、中文路径、
UTF-8 有/无 BOM、CRLF/LF 保留以及释放均通过。
隐藏控件显式完成尺寸分配后测试光标，不修改内部标志来绕过正常逻辑。
浏览器测试注入独立 WinForms WebBrowser，仅加载本地 HTML，验证四个设置及样式更新/清除；
没有覆盖完整 GTK 嵌入、页面导航和实际渲染效果。

`-Mode old` 切回旧 Mono.Debugging/Debugger/SourceEditor2/DesignerSupport/CocoStudio.SourceEditor/CocoStudio.LuaBinding，WindowsPlatform 仍使用修复后的同一候选，
用于公平比较编辑器行为，不代表原平台兼容。两种模式均通过；测试会检查复制哈希和实际加载路径。
日志在独立的 `obj/RestoredEditorSmoke-*` 下。编译测试程序仍有一条 Xwt 版本引用警告，运行使用现有应用绑定配置。

## Cocos Studio 上层编辑器

`CocoStudio.SourceEditor` 是应用层 Lua 编辑入口，包含文件类型识别、编辑器工厂及初始化封装，不是新的 NuGet 替换。
源码构建保持原程序集名/版本、AddinRoot/依赖、类型上的 Extension 标记及原有行为；36 项接口审计无差异。
其依赖采用显式 PackageReference 和 ProjectReference，输出哈希检查防止原 DLL 覆盖。
行为测试通过上层 SourceEditorDisplayBinding 创建真实 TextEditorView，覆盖大小写 Lua 后缀、非 Lua 拒绝、
加载/保存/重载、光标、撤销/重做及 CanReuseView，并在新旧程序集上分别运行。
测试应先初始化上层绑定，再创建编辑器；上层初始化会禁用 MonoDevelop 自动保存。将它放在底层测试编辑操作之后
曾使无主事件循环的测试进程超时，调整测试启动顺序后通过；未为此改动产品源码。

## MSBuild 项目构建桥接

保留 166 项接口以及调用方的 29 处成员引用。这里仍使用旧 `Microsoft.Build.BuildEngine`，没有换成现代 MSBuild/NuGet 引擎。
原 ProjectBuilder 反射访问 Mono 的 `BuildItem.evaluatedMetadata` 字段；Windows .NET Framework 实现没有此字段，
实际读取条目元数据会抛 NullReferenceException。现改用公开的 `CustomMetadataNames` 和 `GetEvaluatedMetadata`，
保留自定义元数据及属性展开，且后者已反转义，不再重复反转义字面的百分号序列。
参考：[Microsoft BuildItem API](https://learn.microsoft.com/en-us/dotnet/api/microsoft.build.buildengine.builditem)。

`Test-RestoredMSBuild.ps1` 在独立进程、独立 AppDomain 中执行临时 `.proj`，覆盖远程接口代理/结果序列化、日志回调、
全局属性、中文路径及元数据、转义、警告/错误、未保存内容刷新、磁盘刷新、非法 XML 和卸载。
旧库只通过公共基础用例，条目读取异常作为已知缺陷显式复现；新库通过全部用例，不能把两者描述为行为完全一致。
测试项目仅含内置 Message/Warning/Error 任务，不访问用户工程或启动完整 IDE；未验证独立远程构建进程的启动协议、
真实 C#/C++ 工程构建、Mono/macOS 或现代 SDK 风格项目。原始 DLL 保留但也保留该旧缺陷。

## 人工回归入口（尚未执行）

默认源码版程序为 `bin/Debug/CocosStudio.exe`，隔离构建为 `bin/RestoredMonoDevelopTest/CocosStudio.exe`。
输出隔离不代表完整编辑器的用户配置也隔离；自动测试不启动它。
人工运行前先保存现有工作，用测试工程副本并备份相关配置；避免与正式编辑器的单实例机制冲突。

重点验证：

1. 打开 Lua 文件，中文文本加载、修改、保存、重新打开；确认编码与换行不变。
2. 光标跳转、页内翻页、ShowMap/ShowFull、查找替换、自动完成。
3. 普通/条件/禁用断点与异常断点保存，关闭后重新打开，确认状态保留。
4. 真实调试连接、断点命中、单步、监视表达式、停止及目标进程退出后的编辑器状态。

没有真实调试目标的情况下，模拟退出事件不能替代第 4 项验证。
现已切换默认源码构建供用户试用，但尚未验证完整编辑器界面、真实调试、macOS 或干净机器；不能据有限测试直接发布。
