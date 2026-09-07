# Mono.TextEditor 审计与保留决定

## 结论

`Mono.TextEditor` 是 Studio 使用的底层编辑器库，不包含 Cocos 业务定制。此前恢复的 MonoDevelop 5.9.5.10 上游源码能够重建兼容程序集，但源码化不会给本项目带来需要维护的功能价值，因此已撤销正式源码接入，继续保留原 DLL。

- 文件：`dlls/Mono.TextEditor.dll`
- 程序集身份：`Mono.TextEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null`
- SHA-256：`47DAA700220831CD86F0135E8CC57B920D4D8BF76AB734D781DE6BC27D0800B6`
- 固定来源提交：`632862e2e3dc6485fadc3f30d4454f97a9187c2d`

## 保留的验证

专项审计仍比较程序集身份、公开 API、序列化布局、39 项资源、程序集引用和 12 个原生入口。行为测试覆盖文本编辑、撤销重做、选择、搜索、折叠和语法资源。候选输出必须与固定 DLL 完全一致，不再允许“换目标框架或依赖版本”作为正常差异。

当前直接源码消费者通过普通程序集引用使用该 DLL。确有 Cocos 定制的 `MonoDevelop.SourceEditor2` 继续以源码维护，并保留 `ProcessSaveText`、`ProcessLoadText`、`PrepareToSetCaret`、`OwnerDocument` 等入口。

除非以后出现必须修复且无法在上层隔离的问题，否则不再恢复或反编译 `Mono.TextEditor` 源码。
