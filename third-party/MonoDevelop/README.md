# MonoDevelop 相关依赖

当前采用“有定制才维护源码，通用底层库保留固定 DLL”的模式。

## 维护源码的组件

- `MonoDevelop.SourceEditor2`：保留 Cocos 定制入口，包括 `ProcessSaveText`、`ProcessLoadText`、`PrepareToSetCaret`、`OwnerDocument` 以及对 `CocoStudio.SourceEditor` 的友元授权。
- `MonoDevelop.Projects.Formats.MSBuild`：保留本仓库针对项目元数据读取的兼容修复。

Cocos 自身的 `CocoStudio.SourceEditor`、`CocoStudio.LuaBinding` 和 `CocoStudio.WindowsPlatform` 位于 `src/`，不属于本目录。

## 固定 DLL

下列库没有 Cocos 业务定制，不再维护反编译或上游源码副本：

| 文件 | SHA-256 |
| --- | --- |
| `dlls/Mono.Debugging.dll` | `ADBC06E24AF54E2D5BAE778BC55756AE14AA2DDAAF56BD2CC982401FA606BACE` |
| `dlls/Mono.TextEditor.dll` | `47DAA700220831CD86F0135E8CC57B920D4D8BF76AB734D781DE6BC27D0800B6` |
| `dlls/MonoDevelop.Debugger.dll` | `F18016A71BB89405759658319E12AA9A4B35E2894E9A78088190285A599CCF02` |
| `dlls/MonoDevelop.DesignerSupport.dll` | `37058746AD28C9A6D0617DCBDF01BD317BF1E55548CA8B19549F2F0D6974D498` |
| `dlls/MonoDevelop.Refactoring.dll` | `D61254DA8795752ECEA8C6216AFD90CD7B4FFEA9E0B2571A2F833D9E4CF847FD` |

`MonoDevelop.Debugger.dll` 与 `Mono.Debugging.dll` 是匹配的一组旧接口。不要只把后者换成 NuGet 版本；NuGet 版改变了 `BreakpointStore` 和 `TargetExited` 等接口，会迫使 Debugger 源码重编译，但没有给 Studio 带来实际收益。

五个 DLL 均来自固定提交 `632862e2e3dc6485fadc3f30d4454f97a9187c2d`。测试基准工具使用二进制流提取并校验哈希，正式构建不会从测试缓存解析依赖。

## 构建规则

- `UseRestoredMonoDevelop` 默认启用；关闭时明确失败，防止生成缺少本地源码组件的不完整输出。
- 源码组件通过项目引用构建，固定组件从 `dlls` 复制。
- 不使用 DEVPATH，也不为 Mono.Debugging 增加绑定重定向。
- 回滚依靠 Git 恢复完整版本，不维护第二套正式输出。

## 验证入口

```powershell
.\tests\Test-RepositoryLayout.ps1
.\tests\Build-RestoredMonoDevelop.ps1 -Configuration Debug
.\tests\Build-RestoredMonoDevelop.ps1 -Configuration Release
.\tests\Audit-RestoredMonoDevelop.ps1
.\tests\Test-RestoredMonoDevelop.ps1
.\tests\Test-RestoredMonoDevelopEditor.ps1 -Mode new
.\tests\Test-MonoTextEditor.ps1 -CandidateDirectory .\bin\RestoredMonoDevelopTest
.\tests\Test-NuGetProjectDeclarations.ps1
```

自动测试不能替代编辑器打开/保存、补全、断点、属性面板、滚动和 Stetic 设计器的交互验收。
