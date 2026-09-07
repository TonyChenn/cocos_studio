param()

$ErrorActionPreference = 'Stop'
$root = (Resolve-Path "$PSScriptRoot/..").Path
$source = Join-Path $root 'third-party\MonoDevelop\Mono.TextEditor'
$project = Join-Path $source 'Mono.TextEditor.Restored.csproj'
$manifest = Join-Path $source 'SOURCE_MANIFEST.tsv'

if (!(Test-Path -LiteralPath $project -PathType Leaf)) { throw 'Mono.TextEditor restored project is missing.' }
if (!(Test-Path -LiteralPath $manifest -PathType Leaf)) { throw 'Mono.TextEditor source manifest is missing.' }

$rows = @(Import-Csv -LiteralPath $manifest -Delimiter "`t")
if ($rows.Count -ne 201) { throw "Unexpected Mono.TextEditor source manifest size: $($rows.Count)" }
$seen = @{}
foreach ($row in $rows) {
    if ($seen.ContainsKey($row.path)) { throw "Duplicate source manifest path: $($row.path)" }
    $seen[$row.path] = $true
    $path = Join-Path $source $row.path
    if (!(Test-Path -LiteralPath $path -PathType Leaf)) { throw "Missing restored source file: $($row.path)" }
    $actual = (Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash
    if ($actual -ne $row.sha256) { throw "Restored source hash changed without updating provenance: $($row.path)" }
}

$payload = @(Get-ChildItem $source -Recurse -File | Where-Object {
    $_.Name -notin 'Mono.TextEditor.Restored.csproj','SOURCE_PROVENANCE.md','SOURCE_MANIFEST.tsv','LICENSE.txt'
})
$extras = @($payload | Where-Object {
    $relative = $_.FullName.Substring($source.Length + 1).Replace('\','/')
    !$seen.ContainsKey($relative)
})
if ($extras.Count) { throw "Unclassified restored source files: $($extras.FullName -join ', ')" }
if (@($payload | Where-Object Extension -eq '.cs').Count -ne 161) { throw 'Expected 161 upstream C# source files.' }
if (@($payload | Where-Object Extension -in '.xml','.json','.stetic').Count -ne 39) { throw 'Expected 39 embedded resource files.' }

[xml]$xml = Get-Content -Raw -LiteralPath $project
$properties = @($xml.Project.PropertyGroup)
function Get-Property([string]$name) {
    @($properties | ForEach-Object { $_.$name } | Where-Object { $_ } | Select-Object -Last 1)[0]
}
if ((Get-Property 'AssemblyName') -ne 'Mono.TextEditor' -or
    (Get-Property 'TargetFrameworkVersion') -ne 'v4.8' -or
    (Get-Property 'LangVersion') -ne '7.3' -or
    (Get-Property 'PlatformTarget') -ne 'AnyCPU' -or
    (Get-Property 'AllowUnsafeBlocks') -ne 'true') {
    throw 'Mono.TextEditor restored project identity or compiler settings changed.'
}
$projectText = Get-Content -Raw -LiteralPath $project
if ($projectText -match 'MonoDevelop\.(Core|Ide)') { throw 'Mono.TextEditor must not depend on MonoDevelop.Core or MonoDevelop.Ide.' }
foreach ($excluded in 'Mono.TextEditor\ITextPasteHandler.cs','Mono.TextEditor\Gui\SolidFoldMarkerMargin.cs') {
    if ($projectText -notmatch [regex]::Escape($excluded)) { throw "Upstream non-compiled source is not excluded: $excluded" }
}
$assemblyInfo = Get-Content -Raw -LiteralPath (Join-Path $source 'AssemblyInfo.cs')
if ($assemblyInfo -notmatch 'AssemblyVersion\("1\.0\.0\.0"\)' -or
    $assemblyInfo -notmatch 'InternalsVisibleTo\("MonoDevelop\.TextEditor\.Tests"\)') {
    throw 'Mono.TextEditor assembly version or friend assembly changed.'
}

$consumers = @(
    'src\Editor\CocoStudio.LuaBinding\CocoStudio.LuaBinding.Restored.csproj',
    'src\Editor\CocoStudio.SourceEditor\CocoStudio.SourceEditor.Restored.csproj',
    'src\Framework\CocoStudio.Gtk.Extend\CocoStudio.Gtk.Extend.csproj',
    'src\Modules\Communal\Modules.Communal.Render\Modules.Communal.Render.csproj',
    'third-party\MonoDevelop\MonoDevelop.Debugger\MonoDevelop.Debugger.Restored.csproj',
    'third-party\MonoDevelop\MonoDevelop.DesignerSupport\MonoDevelop.DesignerSupport.Restored.csproj',
    'third-party\MonoDevelop\MonoDevelop.Refactoring\MonoDevelop.Refactoring.Restored.csproj',
    'third-party\MonoDevelop\MonoDevelop.SourceEditor2\MonoDevelop.SourceEditor2.Restored.csproj'
)
foreach ($relative in $consumers) {
    $path = Join-Path $root $relative
    [xml]$consumer = Get-Content -Raw -LiteralPath $path
    $legacy = @($consumer.Project.ItemGroup.Reference | Where-Object { $_.Include -match '^Mono\.TextEditor(,|$)' })
    $references = @($consumer.Project.ItemGroup.ProjectReference | Where-Object {
        $_.Include -and [IO.Path]::GetFullPath((Join-Path (Split-Path $path -Parent) $_.Include)) -eq $project
    })
    if ($legacy.Count -or $references.Count -ne 1) { throw "Mono.TextEditor consumer was not switched exactly once: $relative" }
}

$copyRules = Get-Content -Raw -LiteralPath (Join-Path $root 'Directory.Build.targets')
$referenceRules = Get-Content -Raw -LiteralPath (Join-Path $root 'build\RestoredMonoDevelop.targets')
if ($copyRules -notmatch 'DllFiles Remove="[^\"]*dlls\\Mono\.TextEditor\.dll"' -or
    $referenceRules -notmatch "WithMetadataValue\('Filename', 'Mono\.TextEditor'\)") {
    throw 'Mono.TextEditor legacy copy exclusion is missing.'
}

$configHash = (Get-FileHash -LiteralPath (Join-Path $source 'Mono.TextEditor.dll.config') -Algorithm SHA256).Hash
$runtimeConfigHash = (Get-FileHash -LiteralPath (Join-Path $root 'runtime-assets\Mono.TextEditor.dll.config') -Algorithm SHA256).Hash
if ($configHash -ne $runtimeConfigHash) { throw 'Mono.TextEditor runtime dllmap differs from restored upstream source.' }

$sourceEditor2 = Join-Path $root 'third-party\MonoDevelop\MonoDevelop.SourceEditor2'
foreach ($hook in 'ProcessSaveText','ProcessLoadText','PrepareToSetCaret') {
    if (!@(Get-ChildItem $sourceEditor2 -Recurse -Filter '*.cs' | Select-String -SimpleMatch $hook).Count) {
        throw "SourceEditor2 Cocos hook is missing: $hook"
    }
}
$sourceEditorAssemblyInfo = Get-Content -Raw -LiteralPath (Join-Path $sourceEditor2 'Properties\AssemblyInfo.cs')
if ($sourceEditorAssemblyInfo -notmatch 'InternalsVisibleTo\("CocoStudio\.SourceEditor"\)') {
    throw 'SourceEditor2 friend access for CocoStudio.SourceEditor is missing.'
}

"PASS Mono.TextEditor source gate: files=$($rows.Count), C#=161, resources=39, consumers=$($consumers.Count)"
'No restore, compilation, copying or editor launch was performed.'
