# 下一阶段计划：剩余 DLL 专项审计

## 当前边界

依赖整理先收敛到以下规则：

- `Mono.TextEditor`、`MonoDevelop.DesignerSupport`、`MonoDevelop.Refactoring` 保留原 DLL。
- `MonoDevelop.Debugger` 与配套的 `Mono.Debugging` 保留原 DLL，不为接入 NuGet 单独恢复 Debugger 源码。
- `MonoDevelop.SourceEditor2` 因存在 Cocos 定制继续维护源码。
- `MonoDevelop.Projects.Formats.MSBuild` 因存在实际兼容修复继续维护源码。
- Cocos 自身的 SourceEditor、LuaBinding、WindowsPlatform 继续位于 `src/` 并维护源码。

## 下一步

1. 重新生成剩余 DLL 的直接项目引用、二进制引用、动态加载和模块注册清单。
2. 为每个库记录“通用第三方库 / Cocos 定制库 / 资源程序集 / 平台适配层”分类，以及保留或替换依据。
3. 通用第三方库默认保留 DLL；仅当有明确安全、构建或兼容问题时，优先寻找匹配版本的官方包，不以反编译源码作为常规替换方式。
4. Cocos 定制库只有在确认定制差异、来源和测试边界后才恢复源码；恢复、验证和删除旧 DLL 在同一批完成。
5. Core、Ide、Cecil/IKVM、Windows Code Pack 等高风险组合单独立项，不与普通清理混合。

内置模块发现、现有平台支持和 Xamarin.Mac 继续保留；不因未使用在线插件安装/更新功能而删除运行时扫描器或内置模块。

## 收尾标准

每批执行“确认必要性 → 选择官方包或保留 DLL → 验证 → 删除确已替换的旧副本 → 更新文档 → 分模块本地提交”。没有明确维护收益的库不再源码化。
