# Cocos Studio 外部 CSD 导入接口（独立插件方案）

## 1. 目标

给外部生成工具提供一个稳定入口。外部工具生成完成后，只需要传入：

- 一个 `.csd` 文件；
- 一个配套资源文件夹；
- Studio 工程内的目标目录。

Studio 负责复制文件、登记资源、刷新资源树，并打开导入后的 CSD。

全部新增代码位于 `src/ExternalImport/`，使用独立解决方案 `CocosStudio.ExternalImport.sln`。原 `CocosStudio.sln`、`Program.cs`、`CocoStudio.Core` 和现有模块源码均不需要修改。

## 2. 外部调用方式

提供独立命令行工具 `CocosStudio.Import.exe`，由外部生成工具调用：

```text
CocosStudio.Import.exe import ^
  --csd "D:\Generate\Login.csd" ^
  --resources "D:\Generate\LoginRes" ^
  --target "ui/login" ^
  --project "D:\Game\Game.ccs" ^
  --overwrite
```

参数说明：

| 参数 | 必填 | 说明 |
| --- | --- | --- |
| `--csd` | 是 | 外部生成的 CSD 文件绝对路径。 |
| `--resources` | 是 | 配套资源文件夹绝对路径。文件夹内部结构保持不变。 |
| `--target` | 是 | 相对于当前工程资源根目录的目标目录，例如 `ui/login`。 |
| `--project` | 否 | 目标 `.ccs` 工程。只运行一个 Studio 实例时可以省略；多实例时必须指定。 |
| `--overwrite` | 否 | 允许覆盖同名 CSD 和资源；不传时遇到冲突直接失败。 |
| `--no-open` | 否 | 导入完成后不自动打开 CSD；默认自动打开。 |

CLI 使用退出码表示最终状态，并向标准输出返回一份 JSON 结果，方便 C#、C++、Python、批处理等外部工具调用。

`--overwrite` 只替换同名文件，不会删除目标资源文件夹中未出现在本次源文件夹里的旧文件。

## 3. 导入后的目录结构

假设请求为：

```text
CSD:       D:\Generate\Login.csd
Resources: D:\Generate\LoginRes\
Target:    ui/login
```

导入结果为：

```text
<工程资源根目录>\ui\login\Login.csd
<工程资源根目录>\ui\login\LoginRes\...
```

资源文件夹名称 `LoginRes` 会保留，不会把其内容直接平铺到目标目录。

CSD 内的资源路径不会由 Studio 自动改写。外部生成器必须按导入后的工程相对路径生成引用，例如：

```text
ui/login/LoginRes/background.png
```

## 4. 请求协议

CLI 与已运行的 Studio 使用当前 Windows 用户专属的命名管道通信。消息采用 UTF-8 JSON，不复用现有 UDP 和二进制序列化协议。

内部请求格式：

```json
{
  "protocolVersion": 1,
  "requestId": "login-ui-001",
  "projectFile": "D:\\Game\\Game.ccs",
  "csdFile": "D:\\Generate\\Login.csd",
  "resourceFolder": "D:\\Generate\\LoginRes",
  "targetDirectory": "ui/login",
  "overwrite": true,
  "openAfterImport": true
}
```

返回格式：

```json
{
  "success": true,
  "requestId": "login-ui-001",
  "code": "OK",
  "message": "Import completed.",
  "importedCsd": "ui/login/Login.csd",
  "importedResourceFolder": "ui/login/LoginRes",
  "openedCsd": "ui/login/Login.csd",
  "warnings": []
}
```

## 5. Studio 插件加载

服务端是 Mono.Addins 插件 `CocosStudio.ExternalImport.StudioPlugin.dll`。插件通过现有 `ICommandHandle` 扩展点注册；Studio 启动执行 `GlobalCommandHandle.InitService()` 时会自动创建扩展对象并启动命名管道服务，不需要改 Studio 启动入口。

插件只引用 Studio 已编译的程序集。默认从仓库的 `bin/<Configuration>/` 读取 `CocoStudio.Core.dll`、`CocoStudio.Projects.dll` 等宿主依赖；如宿主程序集位于其他目录，可在构建插件时传入 MSBuild 属性 `CocosStudioBinDir`。

## 6. Studio 处理流程

1. 确认 Studio 已打开目标工程；指定 `projectFile` 时必须与当前工程一致，不自动切换工程。
2. 校验 CSD 文件、资源文件夹和目标目录；拒绝绝对目标路径、`..`、符号链接或越过工程资源根目录的路径。
3. CSD 必须以 `.csd` 结尾，资源文件夹必须存在，且 CSD 不能位于该资源文件夹内部。
4. 导入前一次性检查所有目标冲突；默认不覆盖，只有 `overwrite=true` 才允许替换。
5. 如果目标 CSD 已在 Studio 中打开且存在未保存修改，拒绝整次覆盖，不弹出自动保存确认框。
6. 将 CSD 复制到目标目录，将整个资源文件夹复制到目标目录并保留文件夹名和内部结构。
7. 新文件通过现有 `ProjectsService.ReadResourceItem` 建立资源模型；已有文件调用 `ResourceItem.Refresh()` 更新内容和缓存。
8. 保存一次当前 Solution，并发布 `AddResourcesEvent` 刷新资源树。
9. 默认打开导入后的 CSD；若该 CSD 已打开且没有未保存修改，则重新加载。

文件复制采用临时文件提交。任一文件复制失败时回滚本次已经创建或覆盖的文件，不向资源树登记半成品。

## 7. 错误码

| 错误码 | 含义 |
| --- | --- |
| `STUDIO_NOT_FOUND` | 没有找到正在运行的 Studio。 |
| `MULTIPLE_STUDIOS` | 存在多个 Studio 实例，但没有指定工程。 |
| `PROJECT_NOT_OPEN` | Studio 尚未打开工程。 |
| `PROJECT_MISMATCH` | 请求工程与该 Studio 当前工程不一致。 |
| `INVALID_CSD` | CSD 路径、扩展名或文件状态无效。 |
| `INVALID_RESOURCE_FOLDER` | 资源文件夹不存在或路径无效。 |
| `INVALID_TARGET` | 目标目录越过工程根目录或包含非法片段。 |
| `TARGET_CONFLICT` | 目标文件已存在，但请求未允许覆盖。 |
| `DIRTY_DOCUMENT` | 请求覆盖 Studio 中尚未保存的 CSD。 |
| `COPY_FAILED` | 文件复制失败，已尝试回滚。 |
| `REGISTER_FAILED` | 文件已复制，但资源模型登记失败，已尝试回滚。 |

## 8. 独立解决方案

- `Protocol/`：命名管道 JSON 协议、请求/响应和 Studio 实例描述。
- `Client/`：外部调用的 `CocosStudio.Import.exe`。
- `StudioPlugin/`：Studio 内的导入服务、命名管道服务和自动启动扩展。
- `Install-Plugin.ps1`：把已构建的插件和协议程序集复制到用户 Addins 目录。

构建入口：

```text
src\ExternalImport\CocosStudio.ExternalImport.sln
```

默认产物目录：

```text
src\ExternalImport\artifacts\Debug\Client\CocosStudio.Import.exe
src\ExternalImport\artifacts\Debug\StudioPlugin\CocosStudio.ExternalImport.StudioPlugin.dll
```

构建完成后安装 Debug 插件：

```powershell
.\src\ExternalImport\Install-Plugin.ps1 -Configuration Debug
```

也可以通过 `-AddinsDirectory` 指定 Studio 实际使用的 Addins 目录。安装或更新插件后需要重启 Studio。

## 9. 验收场景

- 首次导入后，CSD 与资源文件夹出现在指定目录，资源树立即更新并打开 CSD。
- 未传 `--overwrite` 时，任一同名目标都会让整次导入失败且不留下部分文件。
- 允许覆盖时，干净的已打开 CSD 会重新加载；有未保存修改时拒绝覆盖。
- 非法目标路径、源文件缺失、工程不匹配、多实例目标不明确时返回确定错误码。
- 多个 Studio 实例运行时，可通过 `.ccs` 路径准确导入到指定实例。
- 插件未安装或 Studio 未重启时，CLI 返回 `STUDIO_NOT_FOUND`，不会静默复制文件。
- 导入过程中不修改 CSD 内容，不删除现有代码注释。

当前交付只进行源码、工程引用、协议和差异的静态检查，不执行编译或运行验证。
