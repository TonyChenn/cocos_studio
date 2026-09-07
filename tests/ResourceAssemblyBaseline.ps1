# Test-only originals. Production builds must never resolve assemblies from this cache.
function Get-ResourceAssemblyBaseline {
    param([string]$BaselineDirectory, [string]$RepositoryRoot = "$PSScriptRoot/..")
    $revision = '200fed451ace6f21802f8a4542057a21de8f0c2c'
    $hashes = [ordered]@{
        'Cocos.Launcher.Resource.dll' = '0A2E93C972248979A051F85AB5460051F15641183675755AB595E2A8269FB61C'
        'CocoStudio.DefaultResource.dll' = '36CFF5F99CFF7BDCE358059658189D6B143B21451559E2043245BCC4CCA1366D'
    }
    $root = (Resolve-Path -LiteralPath $RepositoryRoot).Path
    $external = ![string]::IsNullOrWhiteSpace($BaselineDirectory)
    if ($external) {
        $directory = (Resolve-Path -LiteralPath $BaselineDirectory).Path
    } else {
        $start = New-Object Diagnostics.ProcessStartInfo
        $start.FileName = 'git'
        $start.WorkingDirectory = $root
        $start.Arguments = "cat-file -e $revision`^{commit}"
        $start.UseShellExecute = $false
        $start.CreateNoWindow = $true
        $start.RedirectStandardError = $true
        $start.EnvironmentVariables['GIT_NO_LAZY_FETCH'] = '1'
        $start.EnvironmentVariables['GIT_TERMINAL_PROMPT'] = '0'
        $process = [Diagnostics.Process]::Start($start)
        $errorText = $process.StandardError.ReadToEnd()
        $process.WaitForExit()
        if ($process.ExitCode -ne 0) { throw "Missing resource baseline history $revision. Supply -BaselineDirectory with verified originals. $errorText" }
        $directory = Join-Path $root "obj\ResourceAssemblyBaseline\$revision"
        $null = New-Item -ItemType Directory -Force -Path $directory
    }
    foreach ($name in $hashes.Keys) {
        $destination = Join-Path $directory $name
        if (!(Test-Path -LiteralPath $destination) -and !$external) {
            $temporary = $destination + '.' + [Guid]::NewGuid().ToString('N') + '.tmp'
            $start = New-Object Diagnostics.ProcessStartInfo
            $start.FileName = 'git'
            $start.WorkingDirectory = $root
            $start.Arguments = "show ${revision}:dlls/$name"
            $start.UseShellExecute = $false
            $start.CreateNoWindow = $true
            $start.RedirectStandardOutput = $true
            $start.RedirectStandardError = $true
            $start.EnvironmentVariables['GIT_NO_LAZY_FETCH'] = '1'
            $start.EnvironmentVariables['GIT_TERMINAL_PROMPT'] = '0'
            $process = [Diagnostics.Process]::Start($start)
            $stderr = $process.StandardError.ReadToEndAsync()
            $stream = [IO.File]::Open($temporary, [IO.FileMode]::CreateNew)
            try { $process.StandardOutput.BaseStream.CopyTo($stream) } finally { $stream.Dispose() }
            $process.WaitForExit()
            if ($process.ExitCode -ne 0) { throw "Cannot extract baseline $name : $($stderr.Result)" }
            if ((Get-FileHash -LiteralPath $temporary -Algorithm SHA256).Hash -ne $hashes[$name]) { throw "Baseline hash mismatch: $name" }
            Move-Item -LiteralPath $temporary -Destination $destination
        }
        if (!(Test-Path -LiteralPath $destination) -or (Get-FileHash -LiteralPath $destination -Algorithm SHA256).Hash -ne $hashes[$name]) {
            throw "Baseline missing or hash mismatch: $name in $directory"
        }
    }
    return $directory
}
