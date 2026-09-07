param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug",

    [string]$AddinsDirectory
)

$ErrorActionPreference = "Stop"
$externalImportRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$artifactDirectory = Join-Path $externalImportRoot ("artifacts\{0}\StudioPlugin" -f $Configuration)
$pluginAssembly = Join-Path $artifactDirectory "CocosStudio.ExternalImport.StudioPlugin.dll"
$protocolAssembly = Join-Path $artifactDirectory "CocosStudio.ExternalImport.Protocol.dll"

if (-not (Test-Path -LiteralPath $pluginAssembly -PathType Leaf)) {
    throw "Plugin artifact was not found: $pluginAssembly. Build StudioPlugin/CocosStudio.ExternalImport.StudioPlugin.csproj first."
}
if (-not (Test-Path -LiteralPath $protocolAssembly -PathType Leaf)) {
    throw "Protocol artifact was not found: $protocolAssembly. Build StudioPlugin/CocosStudio.ExternalImport.StudioPlugin.csproj first."
}

if ([string]::IsNullOrWhiteSpace($AddinsDirectory)) {
    $appSourceFolder = (Get-ItemProperty -Path "HKLM:\SOFTWARE\ChuKong\CocoStudio\AppSourceKey" -Name AppSourceFolder -ErrorAction SilentlyContinue).AppSourceFolder
    if ([string]::IsNullOrWhiteSpace($appSourceFolder) -or -not (Test-Path -LiteralPath $appSourceFolder -PathType Container)) {
        $appSourceFolder = [Environment]::GetFolderPath([Environment+SpecialFolder]::Personal)
    }
    $AddinsDirectory = Join-Path $appSourceFolder "Cocos\CocosStudio2\Addins"
}

$pluginDirectory = Join-Path ([System.IO.Path]::GetFullPath($AddinsDirectory)) "CocosStudio.ExternalImport"
New-Item -ItemType Directory -Force -Path $pluginDirectory | Out-Null
Copy-Item -LiteralPath $pluginAssembly -Destination $pluginDirectory -Force
Copy-Item -LiteralPath $protocolAssembly -Destination $pluginDirectory -Force

Get-ChildItem -LiteralPath $artifactDirectory -Filter "*.pdb" -File -ErrorAction SilentlyContinue | Copy-Item -Destination $pluginDirectory -Force

Write-Host "Plugin installed to: $pluginDirectory"
Write-Host "Restart Cocos Studio so Mono.Addins can discover the plugin."
