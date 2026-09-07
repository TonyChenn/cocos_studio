param(
    [string]$BaselineDirectory,
    [string]$CandidateDirectory,
    [string]$OutputDirectory = "$PSScriptRoot/../bin/Debug"
)
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path -LiteralPath "$PSScriptRoot/..").Path
. "$PSScriptRoot/RestoredBaseline.ps1"
$baseline = Get-RestoredBaseline -BaselineDirectory $BaselineDirectory -RepositoryRoot $root
$original = Join-Path $baseline 'Mono.TextEditor.dll'
$output = (Resolve-Path -LiteralPath $OutputDirectory).Path
$candidate = if ($CandidateDirectory) { (Resolve-Path -LiteralPath $CandidateDirectory).Path } else { $null }
$cecilDirectory = if ($candidate -and (Test-Path -LiteralPath (Join-Path $candidate 'Mono.Cecil.dll'))) { $candidate } else { $output }
Add-Type -Path (Join-Path $cecilDirectory 'Mono.Cecil.dll')

function Get-AllTypes($type) {
    $type
    foreach ($nested in $type.NestedTypes) { Get-AllTypes $nested }
}

function Test-CompilerGenerated($item) {
    if ($item.Name -match '[<>]' -or $item.FullName -like '<PrivateImplementationDetails>*') { return $true }
    $item -is [Mono.Cecil.TypeDefinition] -and
        @($item.CustomAttributes | Where-Object AttributeType -match 'CompilerGeneratedAttribute$').Count -gt 0
}

function Format-AttributeValue($value) {
    if ($value -is [Mono.Cecil.CustomAttributeArgument]) { return "$(Format-AttributeValue $value.Type):$(Format-AttributeValue $value.Value)" }
    if ($value -is [System.Collections.IEnumerable] -and $value -isnot [string]) { return '[' + (@($value | ForEach-Object { Format-AttributeValue $_ }) -join ',') + ']' }
    if ($null -eq $value) { return '<null>' }
    return [string]$value
}

function Get-AttributeKey($attribute) {
    $arguments = @($attribute.ConstructorArguments | ForEach-Object { Format-AttributeValue $_ })
    $properties = @($attribute.Properties | ForEach-Object { "P:$($_.Name)=$(Format-AttributeValue $_.Argument)" } | Sort-Object)
    $fields = @($attribute.Fields | ForEach-Object { "F:$($_.Name)=$(Format-AttributeValue $_.Argument)" } | Sort-Object)
    "$($attribute.AttributeType.FullName):$($arguments + $properties + $fields -join '|')"
}

function Get-GenericParameterKey($parameter) {
    $constraints = @($parameter.Constraints | ForEach-Object ConstraintType | ForEach-Object FullName | Sort-Object)
    "$($parameter.Name):$($parameter.Attributes):$($constraints -join ',')"
}

function Get-Surface($assembly) {
    foreach ($top in $assembly.MainModule.Types) {
        foreach ($type in (Get-AllTypes $top)) {
            if ($type.FullName -eq '<Module>' -or (Test-CompilerGenerated $type)) { continue }
            $typeAttributes = $type.Attributes
            # Roslyn adds BeforeFieldInit to fieldless interfaces; the legacy Mono compiler did not.
            if ($type.IsInterface -and !$type.HasFields -and !@($type.Methods | Where-Object Name -eq '.cctor').Count) {
                $typeAttributes = $typeAttributes -band (-bnot [Mono.Cecil.TypeAttributes]::BeforeFieldInit)
            }
            $generic = @($type.GenericParameters | ForEach-Object { Get-GenericParameterKey $_ }) -join ';'
            $interfaces = @($type.Interfaces | ForEach-Object InterfaceType | ForEach-Object FullName | Sort-Object) -join ';'
            "TYPE $($type.FullName) ATTR=$typeAttributes BASE=$($type.BaseType) GENERIC=$generic INTERFACES=$interfaces"
            foreach ($method in $type.Methods | Where-Object { !$_.IsPrivate -and !(Test-CompilerGenerated $_) }) {
                $methodGeneric = @($method.GenericParameters | ForEach-Object { Get-GenericParameterKey $_ }) -join ';'
                "METHOD $($method.FullName) ATTR=$($method.Attributes) GENERIC=$methodGeneric"
            }
            foreach ($field in $type.Fields | Where-Object { !$_.IsPrivate -and !(Test-CompilerGenerated $_) }) {
                $fieldName = $field.FullName -replace '<Device>e__FixedBuffer0?', '<fixed-buffer>'
                "FIELD $fieldName ATTR=$($field.Attributes)"
            }
            foreach ($property in $type.Properties | Where-Object {
                ($_.GetMethod -and !$_.GetMethod.IsPrivate) -or ($_.SetMethod -and !$_.SetMethod.IsPrivate)
            }) { "PROPERTY $($property.FullName)" }
            foreach ($event in $type.Events | Where-Object {
                ($_.AddMethod -and !$_.AddMethod.IsPrivate) -or ($_.RemoveMethod -and !$_.RemoveMethod.IsPrivate)
            }) { "EVENT $($event.FullName)" }
        }
    }
}

function Get-ResourceRecords($assembly, $sha) {
    @($assembly.MainModule.Resources | ForEach-Object {
        $hash = [BitConverter]::ToString($sha.ComputeHash($_.GetResourceData())).Replace('-', '')
        "$($_.Name)|$($_.Attributes)|$hash"
    } | Sort-Object)
}

function Get-SerializableLayout($types) {
    @($types | Where-Object { $_.IsSerializable -and !(Test-CompilerGenerated $_) } | ForEach-Object {
        "TYPE $($_.FullName) ATTR=$($_.Attributes)"
        $_.Fields | Where-Object { !$_.IsStatic } | ForEach-Object { "FIELD $($_.FullName) ATTR=$($_.Attributes)" }
    } | Sort-Object)
}

function Get-PInvokes($types) {
    @($types | ForEach-Object Methods | Where-Object { $_.PInvokeInfo } | ForEach-Object {
        "$($_.FullName)|$($_.PInvokeInfo.Module.Name)|$($_.PInvokeInfo.EntryPoint)|$($_.PInvokeInfo.Attributes)"
    } | Sort-Object)
}

function Get-AssemblyRecord($path, $label, $sha) {
    $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($path)
    $types = @($assembly.MainModule.Types | ForEach-Object { Get-AllTypes $_ } | Where-Object FullName -ne '<Module>')
    $ignoredAttributes = 'System.Diagnostics.DebuggableAttribute','System.Runtime.CompilerServices.CompilationRelaxationsAttribute','System.Runtime.CompilerServices.RuntimeCompatibilityAttribute','System.Runtime.Versioning.TargetFrameworkAttribute'
    $attributes = @($assembly.CustomAttributes | Where-Object { $_.AttributeType.FullName -notin $ignoredAttributes } | ForEach-Object { Get-AttributeKey $_ } | Sort-Object)
    $targetFramework = @($assembly.CustomAttributes | Where-Object AttributeType -match 'TargetFrameworkAttribute$' | ForEach-Object { [string]$_.ConstructorArguments[0].Value })
    [ordered]@{
        Label = $label
        Path = $path
        Identity = $assembly.Name.FullName
        SHA256 = (Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash
        TargetFramework = $targetFramework
        Attributes = $attributes
        Surface = @(Get-Surface $assembly | Sort-Object -Unique)
        SerializableLayout = @(Get-SerializableLayout $types)
        Resources = @(Get-ResourceRecords $assembly $sha)
        References = @($assembly.MainModule.AssemblyReferences | ForEach-Object FullName | Sort-Object)
        NativeModules = @($assembly.MainModule.ModuleReferences | ForEach-Object Name | Sort-Object -Unique)
        PInvokes = @(Get-PInvokes $types)
        TypeCount = $types.Count
    }
}

function Add-Differences($differences, $field, $oldValues, $newValues) {
    foreach ($difference in @(Compare-Object @($oldValues) @($newValues))) {
        $differences.Add("$field $($difference.SideIndicator) $($difference.InputObject)")
    }
}

$sha = [Security.Cryptography.SHA256]::Create()
try {
    $oldRecord = Get-AssemblyRecord $original 'fixed Git baseline' $sha
    $directProjects = @(Get-ChildItem $root -Directory | Where-Object Name -notin 'bin','obj','.git','.vs','.codegraph' |
        Get-ChildItem -Recurse -File -Filter *.csproj | Where-Object FullName -notmatch '[\\/](bin|obj)[\\/]' | Where-Object {
            [xml]$xml = Get-Content -Raw -Encoding UTF8 -LiteralPath $_.FullName
            @($xml.Project.ItemGroup.Reference | Where-Object { ($_.Include -split ',')[0] -eq 'Mono.TextEditor' }).Count -gt 0 -or
            @($xml.Project.ItemGroup.ProjectReference | Where-Object { $_.Include -match '(^|[\\/])Mono\.TextEditor\.Restored\.csproj$' }).Count -gt 0
        } | ForEach-Object { $_.FullName.Substring($root.Length + 1) } | Sort-Object)
    $assemblyConsumers = @(Get-ChildItem $output -File | Where-Object Extension -in '.dll','.exe' | ForEach-Object {
        try { $consumer = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($_.FullName) } catch [BadImageFormatException] { return }
        if (@($consumer.MainModule.AssemblyReferences | Where-Object Name -eq 'Mono.TextEditor').Count -gt 0) { $_.Name }
    } | Sort-Object -Unique)
    $steticPaths = @(Get-ChildItem $root -Directory | Where-Object Name -notin 'bin','obj','.git','.vs','.codegraph' |
        Get-ChildItem -Recurse -File -Filter gui.stetic | Select-String -SimpleMatch 'build/bin/Mono.TextEditor.dll' | ForEach-Object {
            "$($_.Path.Substring($root.Length + 1)):$($_.LineNumber):$($_.Line.Trim())"
        } | Sort-Object)
    $report = [ordered]@{
        Tool = 'Mono.Cecil compatibility audit'
        BaselineRevision = '632862e2e3dc6485fadc3f30d4454f97a9187c2d'
        Baseline = $oldRecord
        Candidate = $null
        Differences = @()
        AllowedDifferences = @()
        DirectProjects = $directProjects
        DirectProjectsEvidence = 'current source tree'
        AssemblyConsumers = $assemblyConsumers
        AssemblyConsumersEvidence = 'selected output directory'
        SteticLegacyPaths = $steticPaths
    }
    if ($candidate) {
        $candidateFile = Join-Path $candidate 'Mono.TextEditor.dll'
        if (!(Test-Path -LiteralPath $candidateFile -PathType Leaf)) { throw "Candidate Mono.TextEditor.dll not found: $candidateFile" }
        $newRecord = Get-AssemblyRecord $candidateFile 'candidate output' $sha
        $differences = [Collections.Generic.List[string]]::new()
        foreach ($field in 'Identity','Attributes','Surface','SerializableLayout','Resources','NativeModules','PInvokes') {
            Add-Differences $differences $field $oldRecord[$field] $newRecord[$field]
        }
        $allowed = [Collections.Generic.List[string]]::new()
        Add-Differences $differences 'TargetFramework' $oldRecord.TargetFramework $newRecord.TargetFramework
        Add-Differences $differences 'References' $oldRecord.References $newRecord.References
        $report.Candidate = $newRecord
        $report.Differences = @($differences)
        $report.AllowedDifferences = @($allowed)
    }
    $directory = New-Item -ItemType Directory -Force -Path "$root/obj/MonoTextEditorAudit"
    $reportFile = Join-Path $directory.FullName 'audit.json'
    $report | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $reportFile -Encoding UTF8
    "Mono.TextEditor: projects=$($directProjects.Count) consumers=$($assemblyConsumers.Count) types=$($oldRecord.TypeCount) resources=$($oldRecord.Resources.Count) native=$($oldRecord.NativeModules.Count) stetic=$($steticPaths.Count)"
    if ($candidate) {
        "Candidate differences: $($report.Differences.Count)"
        if ($report.Differences.Count) { throw "Mono.TextEditor compatibility audit failed; see $reportFile" }
    }
    "REPORT=$reportFile"
} finally { $sha.Dispose() }
