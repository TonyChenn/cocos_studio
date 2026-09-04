param([string]$OutputDirectory = "$PSScriptRoot/../bin/Debug")
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path "$PSScriptRoot/..").Path
$output = (Resolve-Path $OutputDirectory).Path
Add-Type -Path (Join-Path $output 'Mono.Cecil.dll')
$records = [ordered]@{}
foreach ($file in Get-ChildItem "$root/dlls" -File -Filter *.dll) {
    $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($file.FullName)
    $records[$file.BaseName] = [ordered]@{
        Name = $file.BaseName
        Identity = $assembly.Name.FullName
        SHA256 = (Get-FileHash -LiteralPath $file.FullName).Hash
        DirectProjects = @()
        AssemblyConsumers = @()
        StringCandidates = @()
        NativeConsumers = @()
        Registration = @($assembly.CustomAttributes | Where-Object { $_.AttributeType.FullName -in 'Mono.Addins.AddinAttribute','Mono.Addins.AddinRootAttribute' } | ForEach-Object {
            "$($_.AttributeType.FullName):$($_.ConstructorArguments.Value -join ',')"
        })
        References = @($assembly.MainModule.AssemblyReferences | ForEach-Object FullName)
    }
}
foreach ($project in Get-ChildItem $root -Directory | Where-Object Name -notin 'bin','obj','.git','.vs','.codegraph' | Get-ChildItem -Recurse -File -Filter *.csproj | Where-Object FullName -notmatch '[\\/](bin|obj)[\\/]') {
    [xml]$xml = Get-Content -Raw -Encoding UTF8 -LiteralPath $project.FullName
    foreach ($reference in $xml.Project.ItemGroup.Reference | Where-Object { $_ -and $_.Include }) {
        $name = ($reference.Include -split ',')[0]
        if ($records.Contains($name)) { $records[$name].DirectProjects += $project.FullName.Substring($root.Length + 1) }
    }
}
function Get-AllTypes($type) {
    $type
    foreach ($nested in $type.NestedTypes) { Get-AllTypes $nested }
}
$namePattern = ($records.Keys | ForEach-Object { [regex]::Escape($_) }) -join '|'
foreach ($file in Get-ChildItem $output -File | Where-Object Extension -in '.dll','.exe') {
    try { $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($file.FullName) } catch [BadImageFormatException] { continue }
    foreach ($reference in $assembly.MainModule.AssemblyReferences) {
        if ($records.Contains($reference.Name)) { $records[$reference.Name].AssemblyConsumers += $file.Name }
    }
    foreach ($module in $assembly.MainModule.ModuleReferences) {
        $name = [IO.Path]::GetFileNameWithoutExtension($module.Name)
        if ($records.Contains($name)) { $records[$name].NativeConsumers += $file.Name }
    }
    foreach ($top in $assembly.MainModule.Types) {
        foreach ($type in (Get-AllTypes $top)) {
            foreach ($method in $type.Methods | Where-Object HasBody) {
                foreach ($instruction in $method.Body.Instructions | Where-Object { $_.OpCode.Code -eq 'Ldstr' }) {
                    $literal = [string]$instruction.Operand
                    foreach ($match in [regex]::Matches($literal, $namePattern)) {
                        # A string match is evidence to investigate, not proof the branch executes at runtime.
                        $records[$match.Value].StringCandidates += "$($file.Name):$($method.FullName) -> $literal"
                    }
                }
            }
        }
    }
}
$rows = @($records.Values | ForEach-Object {
    foreach ($field in 'DirectProjects','AssemblyConsumers','StringCandidates','NativeConsumers') { $_[$field] = @($_[$field] | Sort-Object -Unique) }
    [pscustomobject]$_
})
$directory = New-Item -ItemType Directory -Force -Path "$root/obj/VendoredDependencyAudit"
$rows | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $directory.FullName 'dependencies.json') -Encoding UTF8
$rows | ForEach-Object { "$($_.Name): projects=$($_.DirectProjects.Count) consumers=$($_.AssemblyConsumers.Count) strings=$($_.StringCandidates.Count) registrations=$($_.Registration.Count)" }
"REPORT=$($directory.FullName)/dependencies.json"
