$sourceNugetExe = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$targetNugetExe = ".\nuget.exe"
$toolsFolder = "..\Tools"

function Install-NuGetPackage {

    param(
       [string]$source,
       [string]$packageName,
       [string]$rootFolder,
       [string]$folderName
    )

    ./nuget install $packageName -O $toolsFolder -Source $source
    md $toolsFolder\$folderName
    $prtFolder = Get-ChildItem $toolsFolder | Where-Object {$_.Name -match "$packageName."}
    move $toolsFolder\$prtFolder\$rootFolder\*.* $toolsFolder\$folderName
    Remove-Item $toolsFolder\$prtFolder -Force -Recurse
}

Set-Location $PSScriptRoot
Remove-Item $toolsFolder -Force -Recurse -ErrorAction Ignore
Invoke-WebRequest $sourceNugetExe -OutFile $targetNugetExe
Set-Alias nuget $targetNugetExe -Scope Global -Verbose


$nugetSource = "https://api.nuget.org/v3/index.json"
$psGallerySource = "https://www.powershellgallery.com/api/v2"

#Install-NuGetPackage -source $nugetSource -packageName "Microsoft.CrmSdk.XrmTooling.PluginRegistrationTool" -folderName "PluginRegistration"
#Install-NuGetPackage -source $nugetSource -packageName "Microsoft.CrmSdk.XrmTooling.PackageDeployment.WPF" -folderName "PackageDeployment"
Install-NuGetPackage -source $nugetSource -packageName "Microsoft.CrmSdk.XrmTooling.ConfigurationMigration.Wpf" -rootFolder "Tools" -folderName "ConfigurationMigration\UI"
#Install-NuGetPackage -source $nugetSource -packageName "Microsoft.CrmSdk.XrmTooling.CrmConnector.PowerShell" -rootFolder "tools\Microsoft.Xrm.Tooling.CrmConnector.PowerShell" -folderName "CrmConnector"
#Install-NuGetPackage -source $nugetSource -packageName "Microsoft.CrmSdk.XrmTooling.PackageDeployment.PowerShell" -rootFolder "Tools\Microsoft.Xrm.Tooling.PackageDeployment.Powershell" -folderName "PackageDeploymentPowershell"
Install-NuGetPackage -source $nugetSource -packageName "Microsoft.CrmSdk.CoreTools" -rootFolder "content\bin\coretools" -folderName "CoreTools"
Install-NuGetPackage -source $nugetSource -packageName "XrmCIFramework" -rootFolder "Tools" -folderName "XrmCIFramework"
#Install-NuGetPackage -source $psGallerySource -packageName "microsoft.xrm.tooling.configurationmigration" -rootFolder "" -folderName "ConfigurationMigration\PS"

##
##Remove NuGet.exe
##
Remove-Item nuget.exe    