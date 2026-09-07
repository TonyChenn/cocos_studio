# Test-only originals. Production builds must never resolve assemblies from this cache.
function Get-RestoredBaseline {
    param([string]$BaselineDirectory, [string]$RepositoryRoot = "$PSScriptRoot/..")
    $revision = '632862e2e3dc6485fadc3f30d4454f97a9187c2d'
    $hashes = [ordered]@{
        'Mono.TextEditor.dll' = '47DAA700220831CD86F0135E8CC57B920D4D8BF76AB734D781DE6BC27D0800B6'
        'MonoDevelop.Refactoring.dll' = 'D61254DA8795752ECEA8C6216AFD90CD7B4FFEA9E0B2571A2F833D9E4CF847FD'
        'Mono.Debugging.dll' = 'ADBC06E24AF54E2D5BAE778BC55756AE14AA2DDAAF56BD2CC982401FA606BACE'
        'MonoDevelop.Debugger.dll' = 'F18016A71BB89405759658319E12AA9A4B35E2894E9A78088190285A599CCF02'
        'MonoDevelop.SourceEditor2.dll' = 'B4718C10AA8A14FDDA59F65E9D48FD192211112A51D662E6F3ECB71890745836'
        'MonoDevelop.DesignerSupport.dll' = '37058746AD28C9A6D0617DCBDF01BD317BF1E55548CA8B19549F2F0D6974D498'
        'MonoDevelop.Projects.Formats.MSBuild.dll' = '910A447AB3D71F9B6B9EFF7C715BD758F4ADF797E587B77989EF229FEE21A72D'
        'CocoStudio.SourceEditor.dll' = 'BBAD404B771F239C7117E85176433596CF109C38C9CF09062E6EE419937E112B'
        'CocoStudio.LuaBinding.dll' = 'DE22BC4A54EBDC0A4671F8D20ADC41245283F53E54538E5B3886E593B67278AD'
        'CocoStudio.WindowsPlatform.dll' = '1C2B4E83C1B997EF584D01D9077384A5535F97CE21D424628321BB65E8DA8AA8'
    }
    $root = (Resolve-Path -LiteralPath $RepositoryRoot).Path
    $external = ![string]::IsNullOrWhiteSpace($BaselineDirectory)
    if ($external) { $directory = (Resolve-Path -LiteralPath $BaselineDirectory).Path }
    else {
        # Require the pinned history even when cached: never silently substitute a different baseline.
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
        if ($process.ExitCode -ne 0) { throw "Missing pinned baseline history $revision. Supply -BaselineDirectory with verified originals. $errorText" }
        $directory = Join-Path $root "obj\RestoredBaseline\$revision"
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
