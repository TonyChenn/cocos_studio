param(
    [Parameter(Mandatory = $true)][string]$CandidateDirectory,
    [string]$BaselineDirectory,
    [string]$GtkNativeDirectory = 'C:\Program Files (x86)\GtkSharp\2.12\bin'
)
$ErrorActionPreference = 'Stop'
. "$PSScriptRoot/RestoredBaseline.ps1"
$root = (Resolve-Path "$PSScriptRoot/..").Path
$originals = Get-RestoredBaseline -BaselineDirectory $BaselineDirectory -RepositoryRoot $root
$candidate = (Resolve-Path -LiteralPath $CandidateDirectory).Path
$native = (Resolve-Path -LiteralPath $GtkNativeDirectory).Path
$test = New-Item -ItemType Directory -Path (Join-Path $root ('obj\MonoTextEditorSmoke-' + [Guid]::NewGuid().ToString('N')))
$old = New-Item -ItemType Directory -Path (Join-Path $test.FullName 'old')
$new = New-Item -ItemType Directory -Path (Join-Path $test.FullName 'new')
foreach ($directory in $old,$new) { Get-ChildItem $candidate -File | Copy-Item -Destination $directory.FullName }
Copy-Item -LiteralPath (Join-Path $originals 'Mono.TextEditor.dll') -Destination $old.FullName -Force
$oldHash = (Get-FileHash -LiteralPath (Join-Path $old.FullName 'Mono.TextEditor.dll') -Algorithm SHA256).Hash
if ($oldHash -ne '47DAA700220831CD86F0135E8CC57B920D4D8BF76AB734D781DE6BC27D0800B6') { throw 'Old Mono.TextEditor baseline changed.' }
$newAssembly = Join-Path $new.FullName 'Mono.TextEditor.dll'
if (!(Test-Path -LiteralPath $newAssembly -PathType Leaf)) { throw "Candidate Mono.TextEditor.dll not found: $newAssembly" }

$source = Join-Path $PSScriptRoot 'MonoTextEditorSmoke.cs'
$executable = Join-Path $old.FullName 'MonoTextEditorSmoke.exe'
$references = @('Mono.TextEditor','ICSharpCode.NRefactory','gtk-sharp','gdk-sharp','glib-sharp','pango-sharp','atk-sharp','Mono.Cairo','Xwt','Mono.Posix') | ForEach-Object {
    $path = Join-Path $old.FullName ($_.ToString() + '.dll')
    if (!(Test-Path -LiteralPath $path -PathType Leaf)) { throw "Test dependency missing: $path" }
    '/r:' + $path
}
& "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\csc.exe" /nologo /target:exe /platform:x86 "/out:$executable" @references $source
if ($LASTEXITCODE -ne 0) { throw 'Mono.TextEditor smoke compilation failed.' }
Copy-Item -LiteralPath $executable -Destination $new.FullName
if (Test-Path -LiteralPath (Join-Path $candidate 'CocosStudio.exe.config')) {
    Copy-Item -LiteralPath (Join-Path $candidate 'CocosStudio.exe.config') -Destination ($executable + '.config')
    Copy-Item -LiteralPath (Join-Path $candidate 'CocosStudio.exe.config') -Destination (Join-Path $new.FullName 'MonoTextEditorSmoke.exe.config')
}

function Invoke-Smoke($directory, $label) {
    $start = New-Object Diagnostics.ProcessStartInfo
    $start.FileName = Join-Path $directory 'MonoTextEditorSmoke.exe'
    $start.WorkingDirectory = $directory
    $start.UseShellExecute = $false
    $start.CreateNoWindow = $true
    $start.RedirectStandardOutput = $true
    $start.RedirectStandardError = $true
    $start.StandardOutputEncoding = New-Object Text.UTF8Encoding($false)
    $start.StandardErrorEncoding = New-Object Text.UTF8Encoding($false)
    $start.EnvironmentVariables['PATH'] = $native + ';' + $env:PATH
    $start.EnvironmentVariables.Remove('DEVPATH')
    $process = [Diagnostics.Process]::Start($start)
    $stdout = $process.StandardOutput.ReadToEndAsync()
    $stderr = $process.StandardError.ReadToEndAsync()
    $timedOut = !$process.WaitForExit(45000)
    if ($timedOut) { $process.Kill(); $process.WaitForExit() }
    $stdout.Result | Set-Content -LiteralPath (Join-Path $test.FullName "$label.stdout.log") -Encoding UTF8
    $stderr.Result | Set-Content -LiteralPath (Join-Path $test.FullName "$label.stderr.log") -Encoding UTF8
    if ($timedOut) { throw "$label Mono.TextEditor smoke timed out." }
    if ($process.ExitCode -ne 0) { throw "$label Mono.TextEditor smoke failed: $($stderr.Result)" }
    @($stdout.Result -split '\r?\n' | Where-Object { $_ -like 'RESULT *' })
}

$oldResults = @(Invoke-Smoke $old.FullName 'old')
$newResults = @(Invoke-Smoke $new.FullName 'new')
$differences = @(Compare-Object $oldResults $newResults)
if ($differences.Count) { throw "Mono.TextEditor behavior differs: $($differences | Out-String)" }
"PASS old/new Mono.TextEditor behavior: $($oldResults.Count) result records"
"TEST_DIRECTORY=$($test.FullName)"
