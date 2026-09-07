param([string]$MSBuild)
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path "$PSScriptRoot/..").Path
if (!$MSBuild) {
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    $MSBuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
}
if (!$MSBuild -or !(Test-Path -LiteralPath $MSBuild)) { throw 'Visual Studio MSBuild is required for evaluation only.' }
$projects = @(Get-ChildItem "$root/src","$root/third-party" -Recurse -File -Filter '*.csproj' |
    Where-Object { $_.FullName.Substring($root.Length) -notmatch '[\\/](bin|obj|artifacts)[\\/]' })
if (!$projects.Count) { throw 'No source projects found.' }
$rootProjects = @(Get-ChildItem $root -Directory | Where-Object Name -notin 'src','third-party','obj','bin' |
    Get-ChildItem -File -Filter '*.csproj')
if ($rootProjects.Count) { throw "Unclassified root project: $($rootProjects.FullName -join ', ')" }
$legacySolutions = @(Get-ChildItem $root -Recurse -File -Filter '*.sln' |
    Where-Object { $_.FullName.Substring($root.Length) -notmatch '[\\/](bin|obj|artifacts|\.git|\.vs|\.codegraph)[\\/]' })
if ($legacySolutions.Count) { throw "Legacy .sln files are no longer maintained: $($legacySolutions.FullName -join ', ')" }
[xml]$slnx = Get-Content -Raw "$root/CocosStudio.slnx"
foreach ($entry in $slnx.SelectNodes('//Project[@Path]')) {
    if (!(Test-Path -LiteralPath (Join-Path $root $entry.Path) -PathType Leaf)) { throw "Broken slnx project: $($entry.Path)" }
}
"PASS solution paths: no legacy .sln files; CocosStudio.slnx"
foreach ($configuration in 'Debug','Release') {
    foreach ($project in $projects) {
        # No targets are requested: evaluation cannot build, restore or run copy rules.
        $json = & $MSBuild $project.FullName /nologo "/p:Configuration=$configuration" /p:Platform=x86 `
            -getProperty:RepositoryRoot,AssemblyName,OutDir,IntermediateOutputPath,CocosStudioBinDir,ShouldUnsetParentConfigurationAndPlatform `
            -getItem:ProjectReference,Compile,EmbeddedResource,Content,None,Reference
        if ($LASTEXITCODE -ne 0) { throw "Project evaluation failed: $($project.FullName)" }
        $data = ($json -join "`n") | ConvertFrom-Json
        if ($data.Properties.RepositoryRoot.TrimEnd('\','/') -ne $root -or
            $data.Properties.OutDir.TrimEnd('\','/') -ne "$root\bin\$configuration" -or
            $data.Properties.IntermediateOutputPath.TrimEnd('\','/') -ne "$root\obj\$($project.BaseName)\$configuration") {
            throw "Shared output paths changed: $($project.FullName) ($configuration)"
        }
        foreach ($kind in 'ProjectReference','Compile','EmbeddedResource','Content','None') {
            foreach ($item in $data.Items.$kind) {
                if (!(Test-Path -LiteralPath $item.FullPath)) { throw "Missing $kind in $($project.Name): $($item.FullPath)" }
            }
        }
        foreach ($reference in $data.Items.Reference) {
            if ($reference.HintPath -and $reference.HintPath -match '[\\/]dlls[\\/]') {
                $hint = if ([IO.Path]::IsPathRooted($reference.HintPath)) { [IO.Path]::GetFullPath($reference.HintPath) }
                    else { [IO.Path]::GetFullPath((Join-Path $project.DirectoryName $reference.HintPath)) }
                if (!(Test-Path -LiteralPath $hint -PathType Leaf) -or (Split-Path $hint -Parent) -ne "$root\dlls") {
                    throw "Invalid vendored dependency: $($project.Name): $hint"
                }
            }
        }
        if ($project.BaseName -eq 'CocosStudio.ExternalImport.StudioPlugin' -and
            $data.Properties.CocosStudioBinDir.TrimEnd('\','/') -ne "$root\bin\$configuration") { throw 'ExternalImport host output path changed.' }
        if ($project.BaseName -eq 'CocosStudio') {
            foreach ($path in 'src/Editor/CocoStudio.SourceEditor/CocoStudio.SourceEditor.Restored.csproj',
                'src/Editor/CocoStudio.LuaBinding/CocoStudio.LuaBinding.Restored.csproj',
                'src/Platforms/CocoStudio.WindowsPlatform/CocoStudio.WindowsPlatform.Restored.csproj') {
                $expected = [IO.Path]::GetFullPath((Join-Path $root $path))
                if (@($data.Items.ProjectReference | Where-Object FullPath -eq $expected).Count -ne 1) { throw "Missing application project: $path" }
            }
        }
    }
    "PASS $configuration/x86 evaluation: $($projects.Count) projects; references, source/resource items and output paths"
}
'No restore, compilation, copying or editor launch was performed.'
