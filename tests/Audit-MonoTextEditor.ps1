param(
    [string]$BaselineDirectory,
    [string]$OutputDirectory = "$PSScriptRoot/../bin/Debug"
)
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path -LiteralPath "$PSScriptRoot/..").Path
. "$PSScriptRoot/RestoredBaseline.ps1"
$baseline = Get-RestoredBaseline -BaselineDirectory $BaselineDirectory -RepositoryRoot $root
$original = Join-Path $baseline 'Mono.TextEditor.dll'
$output = (Resolve-Path -LiteralPath $OutputDirectory).Path
Add-Type -Path (Join-Path $output 'Mono.Cecil.dll')

function Get-AllTypes($type) {
    $type
    foreach ($nested in $type.NestedTypes) { Get-AllTypes $nested }
}

$assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($original)
$types = @($assembly.MainModule.Types | ForEach-Object { Get-AllTypes $_ } | Where-Object FullName -ne '<Module>')
$visibleTypes = @($types | Where-Object { $_.IsPublic -or $_.IsNestedPublic -or $_.IsNestedFamily -or $_.IsNestedFamilyOrAssembly })
$visibleMembers = @($types | ForEach-Object {
    @($_.Methods | Where-Object { !$_.IsSpecialName -and ($_.IsPublic -or $_.IsFamily -or $_.IsFamilyOrAssembly) })
    @($_.Fields | Where-Object { $_.IsPublic -or $_.IsFamily -or $_.IsFamilyOrAssembly })
    @($_.Properties | Where-Object {
        ($_.GetMethod -and ($_.GetMethod.IsPublic -or $_.GetMethod.IsFamily -or $_.GetMethod.IsFamilyOrAssembly)) -or
        ($_.SetMethod -and ($_.SetMethod.IsPublic -or $_.SetMethod.IsFamily -or $_.SetMethod.IsFamilyOrAssembly))
    })
    @($_.Events | Where-Object {
        ($_.AddMethod -and ($_.AddMethod.IsPublic -or $_.AddMethod.IsFamily -or $_.AddMethod.IsFamilyOrAssembly)) -or
        ($_.RemoveMethod -and ($_.RemoveMethod.IsPublic -or $_.RemoveMethod.IsFamily -or $_.RemoveMethod.IsFamilyOrAssembly))
    })
})
$directProjects = @(Get-ChildItem $root -Directory | Where-Object Name -notin 'bin','obj','.git','.vs','.codegraph' |
    Get-ChildItem -Recurse -File -Filter *.csproj | Where-Object FullName -notmatch '[\\/](bin|obj)[\\/]' | Where-Object {
        [xml]$xml = Get-Content -Raw -Encoding UTF8 -LiteralPath $_.FullName
        @($xml.Project.ItemGroup.Reference | Where-Object { ($_.Include -split ',')[0] -eq 'Mono.TextEditor' }).Count -gt 0
    } | ForEach-Object { $_.FullName.Substring($root.Length + 1) } | Sort-Object)
$assemblyConsumers = @(Get-ChildItem $output -File | Where-Object Extension -in '.dll','.exe' | ForEach-Object {
    try { $consumer = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($_.FullName) } catch [BadImageFormatException] { return }
    if (@($consumer.MainModule.AssemblyReferences | Where-Object Name -eq 'Mono.TextEditor').Count -gt 0) { $_.Name }
} | Sort-Object -Unique)
$steticPaths = @(Get-ChildItem $root -Directory | Where-Object Name -notin 'bin','obj','.git','.vs','.codegraph' |
    Get-ChildItem -Recurse -File -Filter gui.stetic | Select-String -SimpleMatch 'build/bin/Mono.TextEditor.dll' | ForEach-Object {
        "$($_.Path.Substring($root.Length + 1)):$($_.LineNumber):$($_.Line.Trim())"
    } | Sort-Object)

$record = [ordered]@{
    Tool = 'Mono.Cecil static audit'
    Source = "Git baseline 632862e2e3dc6485fadc3f30d4454f97a9187c2d"
    Identity = $assembly.Name.FullName
    SHA256 = (Get-FileHash -LiteralPath $original -Algorithm SHA256).Hash
    Types = $types.Count
    PublicOrProtectedTypes = $visibleTypes.Count
    PublicOrProtectedMembers = $visibleMembers.Count
    Resources = @($assembly.MainModule.Resources | ForEach-Object Name | Sort-Object)
    References = @($assembly.MainModule.AssemblyReferences | ForEach-Object FullName)
    NativeModules = @($assembly.MainModule.ModuleReferences | ForEach-Object Name | Sort-Object -Unique)
    FriendAssemblies = @($assembly.CustomAttributes | Where-Object AttributeType -match 'InternalsVisibleToAttribute$' | ForEach-Object { [string]$_.ConstructorArguments[0].Value })
    SerializableTypes = @($types | Where-Object IsSerializable | ForEach-Object FullName | Sort-Object)
    DirectProjects = $directProjects
    DirectProjectsEvidence = 'current source tree after repository layout migration'
    AssemblyConsumers = $assemblyConsumers
    AssemblyConsumersEvidence = 'existing bin/Debug snapshot; not rebuilt after repository layout migration'
    SteticLegacyPaths = $steticPaths
}
$directory = New-Item -ItemType Directory -Force -Path "$root/obj/MonoTextEditorAudit"
$record | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $directory.FullName 'audit.json') -Encoding UTF8
"Mono.TextEditor: projects=$($directProjects.Count) consumers=$($assemblyConsumers.Count) types=$($types.Count) resources=$($record.Resources.Count) stetic=$($steticPaths.Count)"
"REPORT=$($directory.FullName)/audit.json"
