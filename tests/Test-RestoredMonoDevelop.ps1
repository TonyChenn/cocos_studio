param([string]$BaselineDirectory,
[string]$OutputDirectory = "$PSScriptRoot/../bin/RestoredMonoDevelopTest")
$ErrorActionPreference = 'Stop'
. "$PSScriptRoot/RestoredBaseline.ps1"
$originals = Get-RestoredBaseline -BaselineDirectory $BaselineDirectory
$baseline = (Resolve-Path $OutputDirectory).Path
$repository = (Resolve-Path "$PSScriptRoot/..").Path
$testRoot = Join-Path "$repository/obj" ('RestoredMonoDevelopSmoke-' + [Guid]::NewGuid().ToString('N'))
$oldRoot = New-Item -ItemType Directory -Path "$testRoot/old"
$newRoot = New-Item -ItemType Directory -Path "$testRoot/new"
$fixtures = New-Item -ItemType Directory -Path "$testRoot/fixtures"
foreach ($destination in $oldRoot,$newRoot) {
    # Include application assemblies too: their AddinRoot attributes are part of the registry baseline.
    Get-ChildItem $baseline -File | Copy-Item -Destination $destination.FullName
}
foreach ($name in 'Mono.Debugging','MonoDevelop.Debugger','MonoDevelop.SourceEditor2','MonoDevelop.Refactoring','MonoDevelop.DesignerSupport','CocoStudio.LuaBinding','CocoStudio.SourceEditor','MonoDevelop.Projects.Formats.MSBuild') {
    Copy-Item -LiteralPath "$originals/$name.dll" -Destination $oldRoot.FullName
}
$executable = Join-Path $oldRoot.FullName 'DebuggingSmoke.exe'
$source = Join-Path $PSScriptRoot 'RestoredMonoDevelopSmoke.cs'
& "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\csc.exe" /nologo /target:exe /platform:x86 "/out:$executable" $source
if ($LASTEXITCODE -ne 0) { throw 'Smoke compilation failed' }
Copy-Item $executable $newRoot.FullName
$sessionExecutable = Join-Path $newRoot.FullName 'SessionSmoke.exe'
$sessionSource = Join-Path $PSScriptRoot 'RestoredMonoDevelopSessionSmoke.cs'
$debuggingReference = Join-Path $newRoot.FullName 'Mono.Debugging.dll'
& "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\csc.exe" /nologo /target:exe /platform:x86 "/out:$sessionExecutable" "/r:$debuggingReference" $sessionSource
if ($LASTEXITCODE -ne 0) { throw 'Session smoke compilation failed' }
$registryExecutable = Join-Path $newRoot.FullName 'RegistrySmoke.exe'
$registrySource = Join-Path $PSScriptRoot 'RestoredMonoDevelopRegistrySmoke.cs'
$addinsReference = Join-Path $newRoot.FullName 'Mono.Addins.dll'
& "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\csc.exe" /nologo /target:exe /platform:x86 "/out:$registryExecutable" "/r:$addinsReference" $registrySource
if ($LASTEXITCODE -ne 0) { throw 'Registry smoke compilation failed' }
Copy-Item $registryExecutable $oldRoot.FullName
foreach ($destination in $oldRoot,$newRoot) {
    [xml]$config = Get-Content -Raw "$baseline/CocosStudio.exe.config"
    # Both Mono.Debugging assemblies use 0.0.0.0; do not add a redirect or DEVPATH workaround.
    $config.Save("$destination/DebuggingSmoke.exe.config")
}
Copy-Item "$newRoot/DebuggingSmoke.exe.config" ($sessionExecutable + '.config')
Copy-Item "$newRoot/DebuggingSmoke.exe.config" ($registryExecutable + '.config')
Copy-Item "$oldRoot/DebuggingSmoke.exe.config" "$oldRoot/RegistrySmoke.exe.config"
$savedDevPath = $env:DEVPATH
try {
    $env:DEVPATH = $null
    & "$oldRoot/DebuggingSmoke.exe" old $fixtures.FullName | Tee-Object "$testRoot/old.log"
    if ($LASTEXITCODE -ne 0) { throw 'Old baseline failed' }
    & "$newRoot/DebuggingSmoke.exe" new $fixtures.FullName | Tee-Object "$testRoot/new.log"
    if ($LASTEXITCODE -ne 0) { throw 'NuGet candidate failed' }
    & "$oldRoot/DebuggingSmoke.exe" verify-old $fixtures.FullName | Tee-Object "$testRoot/rollback.log"
    if ($LASTEXITCODE -ne 0) { throw 'Rollback compatibility failed' }
    & $sessionExecutable | Tee-Object "$testRoot/session.log"
    if ($LASTEXITCODE -ne 0) { throw 'Session event compatibility failed' }
    $oldRegistry = & "$oldRoot/RegistrySmoke.exe" "$testRoot/old-registry" | Tee-Object "$testRoot/old-registry.log"
    if ($LASTEXITCODE -ne 0) { throw 'Old module registry scan failed' }
    $newRegistry = & $registryExecutable "$testRoot/new-registry" | Tee-Object "$testRoot/new-registry.log"
    if ($LASTEXITCODE -ne 0) { throw 'New module registry scan failed' }
    $oldIds = @($oldRegistry | Where-Object { $_ -match '^(ADDIN|ROOT) ' })
    $newIds = @($newRegistry | Where-Object { $_ -match '^(ADDIN|ROOT) ' })
    if (Compare-Object $oldIds $newIds) { throw 'Module registry changed' }
    "PASS old/new module registry: $($newIds.Count) entries"
} finally { $env:DEVPATH = $savedDevPath }
"PASSED: $testRoot"
