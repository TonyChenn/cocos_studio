param([string]$MSBuild)
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path "$PSScriptRoot/..").Path
if (!$MSBuild) {
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    $MSBuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
}
if (!$MSBuild -or !(Test-Path -LiteralPath $MSBuild)) { throw 'Visual Studio MSBuild is required.' }
$projects = @(Get-ChildItem $root -Directory | Where-Object Name -notin 'bin','obj','.git','.vs','.codegraph' |
    Get-ChildItem -Filter '*.csproj' -File -Recurse | Where-Object FullName -notmatch '[\\/](bin|obj)[\\/]')
$gtkCount = 0
foreach ($project in $projects) {
    [xml]$xml = Get-Content -Raw -LiteralPath $project.FullName
    $packages = @($xml.Project.ItemGroup.PackageReference | Where-Object { $_ })
    $debugging = @($packages | Where-Object Include -eq 'Mono.Debugging')
    if ($debugging.Count) {
        throw "Mono.Debugging must use the fixed DLL rather than a NuGet declaration: $($project.FullName)"
    }
    $gtkReferences = @($xml.Project.ItemGroup.Reference | Where-Object {
        $_.Include -match '^(atk-sharp|gdk-sharp|glib-sharp|gtk-sharp|Mono.Cairo|pango-sharp|glade-sharp|gtk-dotnet)(,|$)'
    })
    if (!$gtkReferences.Count) { continue }
    $gtkCount++
    $gtk = @($packages | Where-Object Include -eq 'Mono.GtkSharp')
    $posix = @($packages | Where-Object Include -eq 'Mono.Posix-4.5')
    if ($gtk.Count -ne 1 -or $gtk[0].Version -ne '2.12.0.1' -or $gtk[0].GeneratePathProperty -ne 'true' -or
        $gtk[0].ExcludeAssets -ne 'all' -or $gtk[0].PrivateAssets -ne 'all' -or
        $posix.Count -ne 1 -or $posix[0].Version -ne '4.5.0') {
        throw "Missing/incorrect direct Gtk#/Posix declarations: $($project.FullName)"
    }
}
"PASS package declarations: projects=$($projects.Count), Gtk#=$gtkCount, Mono.Debugging NuGet=0"
$projectFile = Join-Path $root 'src\Modules\Communal\Modules.Communal.StartAutoRecover\Modules.Communal.StartAutoRecover.csproj'
# Exercise the transitive copy path without rebuilding project references, as in a Visual Studio build.
$json = & $MSBuild $projectFile /t:GetCopyToOutputDirectoryItems /p:Configuration=Debug /p:Platform=x86 /p:BuildingInsideVisualStudio=true /p:BuildProjectReferences=false -getTargetResult:GetCopyToOutputDirectoryItems /nologo
if ($LASTEXITCODE -ne 0) { throw 'Transitive copy collection failed; restore NuGet packages first.' }
$items = (($json -join "`n") | ConvertFrom-Json).TargetResults.GetCopyToOutputDirectoryItems.Items
foreach ($name in 'atk-sharp','gdk-sharp','glade-sharp','glib-sharp','gtk-dotnet','gtk-sharp','Mono.Cairo','pango-sharp') {
    $matches = @($items | Where-Object TargetPath -eq "$name.dll.config")
    if ($matches.Count -ne 1 -or !(Test-Path -LiteralPath $matches[0].Identity) -or
        $matches[0].Identity -notmatch '[\\/]mono.gtksharp[\\/]2\.12\.0\.1[\\/]build[\\/]') {
        throw "Incorrect transitive config source: $name"
    }
}
'PASS StartAutoRecover transitive copy: 8 configs resolve to the restored package'
$gtkProject = Join-Path $root 'src\Framework\CocoStudio.Core\CocoStudio.Core.csproj'
$missing = & $MSBuild $gtkProject /t:GetCopyToOutputDirectoryItems /p:Configuration=Debug /p:PkgMono_GtkSharp= /p:BuildProjectReferences=false /v:quiet /nologo 2>&1
if ($LASTEXITCODE -eq 0 -or ($missing -join "`n") -notmatch 'Mono.GtkSharp package path is missing') {
    throw 'Missing package path did not fail with the expected restore diagnostic.'
}
'PASS missing package path fails before file copying'
