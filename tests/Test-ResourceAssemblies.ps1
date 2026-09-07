param(
    [Parameter(Mandatory = $true)][string]$CandidateDirectory,
    [string]$BaselineDirectory
)
$ErrorActionPreference = 'Stop'
. "$PSScriptRoot/ResourceAssemblyBaseline.ps1"
$root = (Resolve-Path "$PSScriptRoot/..").Path
$baseline = Get-ResourceAssemblyBaseline -BaselineDirectory $BaselineDirectory -RepositoryRoot $root
$candidate = (Resolve-Path -LiteralPath $CandidateDirectory).Path
$test = New-Item -ItemType Directory -Path (Join-Path $root ('obj\ResourceAssemblySmoke-' + [Guid]::NewGuid().ToString('N')))
$executable = Join-Path $test.FullName 'ResourceAssemblySmoke.exe'
$source = Join-Path $PSScriptRoot 'ResourceAssemblySmoke.cs'
& "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\csc.exe" /nologo /target:exe "/out:$executable" $source
if ($LASTEXITCODE -ne 0) { throw 'Resource assembly smoke compilation failed.' }

foreach ($item in @(
    @{ Name = 'Cocos.Launcher.Resource'; Type = 'Cocos.Launcher.Resources'; Count = 83 },
    @{ Name = 'CocoStudio.DefaultResource'; Type = 'CocoStudio.DefaultResource.Resources'; Count = 302 }
)) {
    $oldOutput = & $executable (Join-Path $baseline "$($item.Name).dll") $item.Type
    if ($LASTEXITCODE -ne 0) { throw "Original resource assembly failed: $($item.Name)" }
    $newOutput = & $executable (Join-Path $candidate "$($item.Name).dll") $item.Type
    if ($LASTEXITCODE -ne 0) { throw "Candidate resource assembly failed: $($item.Name)" }
    $differences = @(Compare-Object @($oldOutput) @($newOutput))
    if ($differences.Count) { throw "Resource assembly changed: $($item.Name)`n$($differences | Out-String)" }
    $resourceCount = @($newOutput | Where-Object { $_ -like 'RESOURCE=*' }).Count
    if ($resourceCount -ne $item.Count) { throw "Unexpected candidate resource count: $($item.Name) = $resourceCount" }
    "PASS $($item.Name): identity, accessor behavior and $resourceCount resource hashes"
}
"TEST_DIRECTORY=$($test.FullName)"
