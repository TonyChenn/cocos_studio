param([string]$MSBuild)
$ErrorActionPreference = 'Stop'
. "$PSScriptRoot/RestoredBaseline.ps1"
$root = (Resolve-Path "$PSScriptRoot/..").Path
$baseline = Get-RestoredBaseline
$test = New-Item -ItemType Directory -Path (Join-Path $root ('obj\BaselineChecks-' + [Guid]::NewGuid().ToString('N')))
$empty = New-Item -ItemType Directory -Path (Join-Path $test.FullName 'empty-history')
& git init --quiet $empty.FullName
if ($LASTEXITCODE -ne 0) { throw 'Cannot initialize empty history fixture' }
$rejected = $false
try { $null = Get-RestoredBaseline -RepositoryRoot $empty.FullName } catch { $rejected = $_.Exception.Message -like '*Missing pinned baseline history*' }
if (!$rejected) { throw 'Missing history was not rejected' }
$null = Get-RestoredBaseline -RepositoryRoot $empty.FullName -BaselineDirectory $baseline
'PASS missing history rejected; explicit verified baseline accepted without history'
$bad = New-Item -ItemType Directory -Path (Join-Path $test.FullName 'bad-hash')
Get-ChildItem $baseline -File -Filter *.dll | Copy-Item -Destination $bad.FullName
$file = Join-Path $bad.FullName 'Mono.Debugging.dll'
$stream = [IO.File]::Open($file, [IO.FileMode]::Append)
try { $stream.WriteByte(0) } finally { $stream.Dispose() }
$rejected = $false
try { $null = Get-RestoredBaseline -BaselineDirectory $bad.FullName } catch { $rejected = $_.Exception.Message -like '*hash mismatch*' }
if (!$rejected) { throw 'Corrupted baseline was not rejected' }
'PASS corrupted baseline rejected'
if (!$MSBuild) {
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    $MSBuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
}
foreach ($target in 'Restore','PrepareForBuild','CopyDllsToOutputDir') {
    $result = & $MSBuild "$root/CocosStudio/CocosStudio.csproj" "/t:$target" /p:UseRestoredMonoDevelop=false /p:Configuration=Debug /v:quiet /nologo 2>&1
    if ($LASTEXITCODE -eq 0 -or "$result" -notlike '*Legacy MonoDevelop fallback has been removed*') { throw "Legacy fallback not rejected at $target : $result" }
}
'PASS legacy fallback rejected during restore, build preparation and copying'
