param([string]$OutputDirectory = "$PSScriptRoot/../bin/RestoredMonoDevelopTest")
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path "$PSScriptRoot/..").Path
$output = (Resolve-Path $OutputDirectory).Path
$test = New-Item -ItemType Directory -Path (Join-Path $root ('obj\RestoredMSBuildSmoke-' + [Guid]::NewGuid().ToString('N')))
$library = 'MonoDevelop.Projects.Formats.MSBuild.dll'
$testSource = Join-Path $PSScriptRoot 'RestoredMSBuildSmoke.cs'
foreach ($mode in 'old','new') {
    $bin = New-Item -ItemType Directory -Path (Join-Path $test.FullName $mode)
    $work = New-Item -ItemType Directory -Path (Join-Path $bin.FullName 'work')
    $source = Join-Path $output $library
    if ($mode -eq 'old') { $source = Join-Path "$root/dlls" $library }
    Copy-Item -LiteralPath $source -Destination $bin.FullName
    if ((Get-FileHash $source).Hash -ne (Get-FileHash "$($bin.FullName)/$library").Hash) { throw 'Copied bridge hash differs' }
    $exe = Join-Path $bin.FullName 'MSBuildSmoke.exe'
    $reference = Join-Path $bin.FullName $library
    & "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\csc.exe" /nologo /target:exe /platform:x86 "/out:$exe" "/r:$reference" $testSource
    if ($LASTEXITCODE -ne 0) { throw 'Bridge test compilation failed' }
    $start = New-Object Diagnostics.ProcessStartInfo
    $start.FileName = $exe
    $start.Arguments = '"' + $work.FullName + '" ' + $mode
    $start.WorkingDirectory = $bin.FullName
    $start.UseShellExecute = $false
    $start.CreateNoWindow = $true
    $start.RedirectStandardOutput = $true
    $start.RedirectStandardError = $true
    $start.StandardOutputEncoding = New-Object Text.UTF8Encoding($false)
    $start.StandardErrorEncoding = New-Object Text.UTF8Encoding($false)
    $start.EnvironmentVariables.Remove('DEVPATH')
    $process = [Diagnostics.Process]::Start($start)
    $stdout = $process.StandardOutput.ReadToEndAsync()
    $stderr = $process.StandardError.ReadToEndAsync()
    $timedOut = !$process.WaitForExit(30000)
    if ($timedOut) { $process.Kill(); $process.WaitForExit() }
    $stdout.Result | Tee-Object (Join-Path $bin.FullName 'stdout.log')
    $stderr.Result | Tee-Object (Join-Path $bin.FullName 'stderr.log')
    "MODE=$mode EXIT=$($process.ExitCode) DIRECTORY=$($bin.FullName)"
    if ($timedOut -or $process.ExitCode -ne 0) { throw 'Bridge behavior test failed; see logs above.' }
}
