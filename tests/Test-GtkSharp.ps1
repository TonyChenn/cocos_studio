param(
    [Parameter(Mandatory = $true)][string]$OutputDirectory,
    [string]$GtkNativeDirectory = 'C:\Program Files (x86)\GtkSharp\2.12\bin'
)

$ErrorActionPreference = 'Stop'
$outputRoot = (Resolve-Path $OutputDirectory).Path
$nativeRoot = (Resolve-Path $GtkNativeDirectory).Path
$testRoot = Join-Path $PSScriptRoot ('..\obj\GtkSharpSmoke-' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $testRoot | Out-Null
$testRoot = (Resolve-Path $testRoot).Path
$names = @('atk-sharp', 'gdk-sharp', 'glade-sharp', 'glib-sharp', 'gtk-dotnet', 'gtk-sharp', 'Mono.Cairo', 'pango-sharp')
foreach ($name in $names + 'Mono.Posix') {
    Copy-Item -LiteralPath (Join-Path $outputRoot "$name.dll") -Destination $testRoot
}
if ([Reflection.AssemblyName]::GetAssemblyName((Join-Path $testRoot 'Mono.Posix.dll')).Version.ToString() -ne '4.0.0.0') {
    throw 'Expected Mono.Posix 4.0, not the old DLL bundled inside Mono.GtkSharp.'
}
$references = $names | ForEach-Object { '/r:' + (Join-Path $testRoot "$_.dll") }
$executable = Join-Path $testRoot 'GtkSharpSmoke.exe'
& "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\csc.exe" /nologo /target:exe /platform:x86 "/out:$executable" @references (Join-Path $PSScriptRoot 'GtkSharpSmoke.cs')
if ($LASTEXITCODE -ne 0) { throw 'Smoke test compilation failed.' }
$savedPath = $env:PATH
$savedDevPath = $env:DEVPATH
try {
    $env:PATH = $nativeRoot + ';' + $savedPath
    $env:DEVPATH = $null
    & $executable | Tee-Object (Join-Path $testRoot 'default.log')
    if ($LASTEXITCODE -ne 0) { throw 'Default-loading smoke test failed.' }

    # This configuration belongs only to the temporary test harness, never the editor or machine.config.
    [xml]$configuration = '<configuration><runtime><developmentMode developerInstallation="true" /></runtime></configuration>'
    $configuration.Save($executable + '.config')
    $env:DEVPATH = $testRoot
    & $executable local | Tee-Object (Join-Path $testRoot 'candidate.log')
    if ($LASTEXITCODE -ne 0) { throw 'Output-DLL smoke test failed.' }
} finally {
    $env:PATH = $savedPath
    $env:DEVPATH = $savedDevPath
}
Write-Output "Passed. Isolated test binaries and logs: $testRoot"
