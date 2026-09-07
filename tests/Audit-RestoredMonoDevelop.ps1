param([string]$BaselineDirectory,

    [string]$CandidateDirectory = "$PSScriptRoot/../bin/RestoredMonoDevelopTest"
)
$ErrorActionPreference = 'Stop'
. "$PSScriptRoot/RestoredBaseline.ps1"
$originals = Get-RestoredBaseline -BaselineDirectory $BaselineDirectory
$root = (Resolve-Path "$PSScriptRoot/..").Path
$baseline = (Resolve-Path $CandidateDirectory).Path
$candidate = (Resolve-Path $CandidateDirectory).Path
if ((Get-FileHash "$candidate/Mono.Debugging.dll").Hash -ne '54E778012E4AD8118EB38BA9788B1B937E36B1A0C6A5880275DAE6EE40AAC3F2') {
    throw 'Output does not contain Mono.Debugging 1.0.20170212.42 from NuGet; do not audit a mixed/legacy output as a candidate.'
}
Add-Type -Path "$baseline/Mono.Cecil.dll"
$resolver = New-Object Mono.Cecil.DefaultAssemblyResolver
$resolver.AddSearchDirectory($candidate)
$resolver.AddSearchDirectory($baseline)
$parameters = New-Object Mono.Cecil.ReaderParameters
$parameters.AssemblyResolver = $resolver
$failures = [Collections.Generic.List[string]]::new()
$sha = [Security.Cryptography.SHA256]::Create()
function Get-Types($type) {
    if ($type.Name -match '[<>]' -or @($type.CustomAttributes | Where-Object { $_.AttributeType.Name -eq 'CompilerGeneratedAttribute' }).Count) { return }
    $type
    foreach ($nested in $type.NestedTypes) { Get-Types $nested }
}
function Get-Surface($assembly) {
    foreach ($top in $assembly.MainModule.Types) {
        foreach ($type in (Get-Types $top)) {
            # Roslyn adds BeforeFieldInit on these fieldless interfaces; they have no initializer to run.
            $attributes = $type.Attributes
            if ($type.IsInterface -and !$type.HasFields -and !@($type.Methods | Where-Object Name -eq '.cctor').Count) {
                $attributes = $attributes -band (-bnot [Mono.Cecil.TypeAttributes]::BeforeFieldInit)
            }
            "TYPE $($type.FullName) $attributes BASE=$($type.BaseType)"
            foreach ($method in $type.Methods | Where-Object { !$_.IsPrivate -and $_.Name -notmatch '[<>]' }) {
                "METHOD $($method.FullName) $($method.Attributes)"
            }
            foreach ($field in $type.Fields | Where-Object { !$_.IsPrivate }) {
                "FIELD $($field.FullName) $($field.Attributes)"
            }
            foreach ($property in $type.Properties) { "PROPERTY $($property.FullName)" }
            foreach ($event in $type.Events) { "EVENT $($event.FullName)" }
            foreach ($interface in $type.Interfaces) { "INTERFACE $($type.FullName) $interface" }
        }
    }
}
function Get-AttributeKey($attribute) {
    $parts = @($attribute.ConstructorArguments | ForEach-Object { "$($_.Type.FullName)=$($_.Value)" })
    $parts += @($attribute.Properties | ForEach-Object { "P:$($_.Name)=$($_.Argument.Value)" } | Sort-Object)
    $parts += @($attribute.Fields | ForEach-Object { "F:$($_.Name)=$($_.Argument.Value)" } | Sort-Object)
    "$($attribute.AttributeType.FullName):$($parts -join '|')"
}
foreach ($name in 'MonoDevelop.Debugger','MonoDevelop.SourceEditor2','MonoDevelop.Refactoring','MonoDevelop.DesignerSupport','CocoStudio.LuaBinding','CocoStudio.WindowsPlatform','CocoStudio.SourceEditor','MonoDevelop.Projects.Formats.MSBuild') {
    $old = [Mono.Cecil.AssemblyDefinition]::ReadAssembly("$originals/$name.dll", $parameters)
    $new = [Mono.Cecil.AssemblyDefinition]::ReadAssembly("$candidate/$name.dll", $parameters)
    $oldSurface = @(Get-Surface $old | Sort-Object -Unique)
    $newSurface = @(Get-Surface $new | Sort-Object -Unique)
    $differences = @(Compare-Object $oldSurface $newSurface)
    if ($name -eq 'CocoStudio.WindowsPlatform') {
        # Only the four required Xwt properties and their accessors are intentional API additions.
        $expectedAdditions = @($differences | Where-Object {
            $_.SideIndicator -eq '=>' -and $_.InputObject -match '^PROPERTY System\.(Boolean|String) CocoStudio\.WindowsPlatform\.CSWindowWebViewBackend::(ContextMenuEnabled|ScrollBarsEnabled|DrawsBackground|CustomCss)\(\)$|^METHOD System\.(Void|Boolean|String) CocoStudio\.WindowsPlatform\.CSWindowWebViewBackend::(get_|set_)(ContextMenuEnabled|ScrollBarsEnabled|DrawsBackground|CustomCss)\('
        })
        if ($expectedAdditions.Count -ne 12) { $failures.Add("WINDOWS ADAPTER expected 12 property/accessor additions, got $($expectedAdditions.Count)") }
        $differences = @($differences | Where-Object { $_ -notin $expectedAdditions })
    }
    foreach ($difference in $differences) { $failures.Add("SURFACE $name $($difference.SideIndicator) $($difference.InputObject)") }
    $resources = 0
    foreach ($resource in $old.MainModule.Resources) {
        $other = $new.MainModule.Resources | Where-Object Name -eq $resource.Name
        if (!$other) { $failures.Add("MISSING RESOURCE $name $($resource.Name)"); continue }
        $oldHash = [Convert]::ToBase64String($sha.ComputeHash($resource.GetResourceData()))
        $newHash = [Convert]::ToBase64String($sha.ComputeHash($other.GetResourceData()))
        if ($oldHash -ne $newHash -or $resource.Attributes -ne $other.Attributes) { $failures.Add("RESOURCE CHANGED $name $($resource.Name)") }
        $resources++
    }
    if ($old.MainModule.Resources.Count -ne $new.MainModule.Resources.Count) { $failures.Add("RESOURCE COUNT $name") }
    foreach ($attribute in $old.CustomAttributes | Where-Object { $_.AttributeType.FullName -match '^(Mono.Addins.|System.Runtime.CompilerServices.InternalsVisibleTo)' }) {
        $key = Get-AttributeKey $attribute
        $match = @($new.CustomAttributes | Where-Object { (Get-AttributeKey $_) -eq $key })
        if (!$match.Count) { $failures.Add("ATTRIBUTE MISSING $name $key") }
    }
    foreach ($type in $old.MainModule.Types) {
        $newType = $new.MainModule.Types | Where-Object FullName -eq $type.FullName
        foreach ($attribute in $type.CustomAttributes | Where-Object { $_.AttributeType.FullName -like 'Mono.Addins.*' }) {
            $key = Get-AttributeKey $attribute
            if (!$newType -or !@($newType.CustomAttributes | Where-Object { (Get-AttributeKey $_) -eq $key }).Count) {
                $failures.Add("TYPE ATTRIBUTE MISSING $name $($type.FullName) $key")
            }
        }
    }
    "ASSEMBLY=$name SURFACE=$($oldSurface.Count) DIFFERENCES=$($differences.Count) RESOURCES=$resources"
    $new.MainModule.AssemblyReferences | Where-Object Name -eq 'Mono.Debugging' | ForEach-Object { "REFERENCE=$($_.FullName)" }
}
$targets = @('Mono.TextEditor','MonoDevelop.Debugger','MonoDevelop.SourceEditor2','MonoDevelop.Refactoring','MonoDevelop.DesignerSupport','CocoStudio.LuaBinding','Mono.Debugging','MonoDevelop.Projects.Formats.MSBuild')
$counts = @{}
$paths = @{}
foreach ($file in Get-ChildItem $baseline -File | Where-Object Extension -in '.dll','.exe') {
    $path = $file.FullName
    if (Test-Path "$candidate/$($file.Name)") { $path = "$candidate/$($file.Name)" }
    try { $consumer = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($path, $parameters) }
    catch [System.BadImageFormatException] { continue }
    foreach ($member in $consumer.MainModule.GetMemberReferences() | Where-Object { $_.DeclaringType.Scope.Name -in $targets }) {
        $scope = $member.DeclaringType.Scope.Name
        $counts[$scope]++
        try {
            $resolved = $member.Resolve()
            if (!$resolved) { $failures.Add("UNRESOLVED $($file.Name) $member"); continue }
            $resolvedPath = [IO.Path]::GetFullPath($resolved.Module.FullyQualifiedName)
            $paths[$resolvedPath] = $true
            if (![string]::Equals($resolvedPath, [IO.Path]::GetFullPath("$candidate/$scope.dll"), [StringComparison]::OrdinalIgnoreCase)) {
                $failures.Add("WRONG RESOLUTION $member $resolvedPath")
            }
        } catch { $failures.Add("UNRESOLVED $($file.Name) $member $($_.Exception.Message)") }
    }
}
foreach ($key in $counts.Keys | Sort-Object) { "MEMBER_REFERENCES=$key COUNT=$($counts[$key])" }
$paths.Keys | Sort-Object | ForEach-Object { "RESOLVED_FROM=$_" }
$failures
"FAILURES=$($failures.Count)"
if ($failures.Count) { exit 1 }
