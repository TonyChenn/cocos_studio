param(
    [string]$OutputDirectory = "$PSScriptRoot/../bin/RestoredMonoDevelopTest",
    [string]$GtkNativeDirectory = 'C:\Program Files (x86)\GtkSharp\2.12\bin',
    [ValidateSet('old','new')][string]$Mode = 'new'
)
$ErrorActionPreference = 'Stop'
$output = (Resolve-Path $OutputDirectory).Path
$native = (Resolve-Path $GtkNativeDirectory).Path
$root = (Resolve-Path "$PSScriptRoot/..").Path
$test = New-Item -ItemType Directory -Path (Join-Path $root ('obj\RestoredEditorSmoke-' + [Guid]::NewGuid().ToString('N')))
$binaries = New-Item -ItemType Directory -Path (Join-Path $test.FullName 'bin')
$work = New-Item -ItemType Directory -Path (Join-Path $test.FullName 'work')
Get-ChildItem $output -File | Copy-Item -Destination $binaries.FullName
if ($Mode -eq 'old') {
    # Compare editor generations with the same repaired Windows adapter; this is not an all-old platform baseline.
    foreach ($name in 'Mono.Debugging','MonoDevelop.Debugger','MonoDevelop.SourceEditor2') {
        Copy-Item -LiteralPath "$root/dlls/$name.dll" -Destination $binaries.FullName
    }
}
foreach ($name in 'Mono.Debugging','MonoDevelop.Debugger','MonoDevelop.SourceEditor2','CocoStudio.WindowsPlatform') {
    $expectedDirectory = $output
    if ($Mode -eq 'old' -and $name -ne 'CocoStudio.WindowsPlatform') { $expectedDirectory = Join-Path $root 'dlls' }
    if ((Get-FileHash "$expectedDirectory/$name.dll").Hash -ne (Get-FileHash "$($binaries.FullName)/$name.dll").Hash) {
        throw "Unexpected test assembly: $name"
    }
}
$exe = Join-Path $binaries.FullName 'EditorSmoke.exe'
$source = Join-Path $PSScriptRoot 'RestoredMonoDevelopEditorSmoke.cs'
$references = @('MonoDevelop.Core','MonoDevelop.Ide','MonoDevelop.SourceEditor2','MonoDevelop.Debugger','MonoDevelop.DesignerSupport','Mono.TextEditor','Mono.Debugging','Mono.Addins','Xwt','ICSharpCode.NRefactory','gtk-sharp','gdk-sharp','glib-sharp','pango-sharp','atk-sharp','Mono.Cairo') | ForEach-Object { '/r:' + (Join-Path $binaries.FullName ($_.ToString() + '.dll')) }
& "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\csc.exe" /nologo /debug /target:exe /platform:x86 /r:System.Windows.Forms.dll "/out:$exe" @references $source
if ($LASTEXITCODE -ne 0) { throw 'Editor smoke compilation failed' }
Copy-Item "$output/CocosStudio.exe.config" ($exe + '.config')
$start = New-Object Diagnostics.ProcessStartInfo
$start.FileName = $exe
$start.Arguments = '"' + $work.FullName + '"'
$start.WorkingDirectory = $binaries.FullName
$start.UseShellExecute = $false
$start.CreateNoWindow = $true
$start.RedirectStandardOutput = $true
$start.RedirectStandardError = $true
$start.StandardOutputEncoding = New-Object Text.UTF8Encoding($false)
$start.StandardErrorEncoding = New-Object Text.UTF8Encoding($false)
$start.EnvironmentVariables['MONODEVELOP_PROFILE'] = Join-Path $work.FullName 'profile'
$start.EnvironmentVariables['PATH'] = $native + ';' + $env:PATH
$start.EnvironmentVariables.Remove('DEVPATH')
$process = [Diagnostics.Process]::Start($start)
$stdout = $process.StandardOutput.ReadToEndAsync()
$stderr = $process.StandardError.ReadToEndAsync()
$timedOut = !$process.WaitForExit(45000)
if ($timedOut) { $process.Kill(); $process.WaitForExit() }
$stdout.Result | Tee-Object (Join-Path $test.FullName 'stdout.log')
$stderr.Result | Tee-Object (Join-Path $test.FullName 'stderr.log')
"TEST_DIRECTORY=$($test.FullName) MODE=$Mode EXIT=$($process.ExitCode)"
if ($timedOut) { throw "Editor smoke timed out; test directory: $($test.FullName)" }
if ($process.ExitCode -ne 0) { throw 'Editor behavior test failed; see logs above.' }
