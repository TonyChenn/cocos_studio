param(
    [string]$MSBuild,
    [ValidateSet('Debug','Release')][string]$Configuration = 'Debug',
    [switch]$DefaultOutput
)
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path "$PSScriptRoot/..").Path
if (!$MSBuild) {
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    $MSBuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
}
if (!$MSBuild -or !(Test-Path -LiteralPath $MSBuild)) { throw 'Pass a valid Visual Studio MSBuild path using -MSBuild.' }
$logs = New-Item -ItemType Directory -Force -Path "$root/obj/RestoredMonoDevelopBuild"
$props = Join-Path $root 'build\RestoredMonoDevelop.Test.props'
$solution = Join-Path $root 'CocosStudio.slnx'
$output = Join-Path $root 'bin\RestoredMonoDevelopTest'
$intermediate = Join-Path $root 'obj\RestoredMonoDevelopTest'
$buildProperties = @("/p:DirectoryBuildPropsPath=$props")
$logName = 'slnx-build.log'
if ($DefaultOutput) {
    $output = Join-Path $root "bin\$Configuration"
    $intermediate = Join-Path $root 'obj'
    $buildProperties = @()
    $logName = 'default-slnx-build.log'
}
$runningEditor = @(Get-Process CocosStudio -ErrorAction SilentlyContinue | Where-Object {
    !$_.Path -or [string]::Equals([IO.Path]::GetDirectoryName($_.Path), $output, [StringComparison]::OrdinalIgnoreCase)
})
if ($runningEditor.Count) { throw "Close the editor using $output before rebuilding. No process was stopped." }
$log = Join-Path $logs.FullName $logName
if ($Configuration -ne 'Debug') { $log = Join-Path $logs.FullName "$Configuration-$logName" }
& $MSBuild $solution /restore /t:Rebuild /m:1 "/p:Configuration=$Configuration" /p:Platform=x86 @buildProperties /v:quiet /nologo /fl "/flp:logfile=$log;encoding=UTF-8" /clp:ErrorsOnly
if ($LASTEXITCODE -ne 0) { throw "Build failed; see $log" }
foreach ($name in 'Mono.TextEditor','MonoDevelop.Debugger','MonoDevelop.SourceEditor2','MonoDevelop.Refactoring','MonoDevelop.DesignerSupport','CocoStudio.LuaBinding','CocoStudio.WindowsPlatform','CocoStudio.SourceEditor','MonoDevelop.Projects.Formats.MSBuild') {
    $sourceBuild = Join-Path $intermediate "$name.Restored\$Configuration\$name.dll"
    $outputDll = Join-Path $output "$name.dll"
    if ((Get-FileHash $sourceBuild).Hash -ne (Get-FileHash $outputDll).Hash) { throw "Source build overwritten in test output: $name" }
}
# NuGet writes UTF-8 without a BOM, including localized warning messages; Windows PowerShell otherwise uses ANSI.
$assets = Get-Content -Raw -Encoding UTF8 "$intermediate/CocosStudio/project.assets.json" | ConvertFrom-Json
$package = @($assets.packageFolders.PSObject.Properties.Name | ForEach-Object {
    Join-Path $_ 'mono.debugging\1.0.20170212.42\lib\net40\Mono.Debugging.dll'
} | Where-Object { Test-Path -LiteralPath $_ })
if ($package.Count -ne 1 -or (Get-FileHash $package[0]).Hash -ne (Get-FileHash "$output/Mono.Debugging.dll").Hash) {
    throw 'Test output does not contain the restored Mono.Debugging NuGet asset.'
}
"Built and verified $output. The editor was not launched. Log: $log"
