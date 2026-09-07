$ErrorActionPreference = 'Stop'
$root = (Resolve-Path "$PSScriptRoot/..").Path
$tracked = @(& git -C $root ls-files | Where-Object { $_ -match '(^|/)(bin|obj)/' })
if ($LASTEXITCODE -ne 0) { throw 'Unable to enumerate tracked files.' }
if ($tracked.Count) {
    throw "Generated bin/obj files must not be tracked:`n$($tracked -join "`n")"
}
'PASS no tracked project-local bin/obj artifacts'
