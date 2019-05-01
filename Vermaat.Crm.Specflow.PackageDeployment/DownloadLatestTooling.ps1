$sourceNugetExe = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$targetNugetExe = ".\nuget.exe"
$toolsFolder = ".\Tools"

function Install-NuGetPackage {

    param(
       [string]$packageName,
       $folderName
    )

    ./nuget install $packageName -O $toolsFolder -Source "https://api.nuget.org/v3/index.json"
    md $toolsFolder\$folderName
    $prtFolder = Get-ChildItem $toolsFolder | Where-Object {$_.Name -match "$packageName."}
    move $toolsFolder\$prtFolder\tools\*.* $toolsFolder\$folderName
    Remove-Item $toolsFolder\$prtFolder -Force -Recurse
}

Set-Location $PSScriptRoot
Remove-Item $toolsFolder -Force -Recurse -ErrorAction Ignore
Invoke-WebRequest $sourceNugetExe -OutFile $targetNugetExe
Set-Alias nuget $targetNugetExe -Scope Global -Verbose


##
##Download Plugin Registration Tool
##
#Install-NuGetPackage -packageName "Microsoft.CrmSdk.XrmTooling.PluginRegistrationTool" -folderName "PluginRegistration"
#Install-NuGetPackage -packageName "Microsoft.CrmSdk.XrmTooling.PackageDeployment.WPF" -folderName "PackageDeployment"
Install-NuGetPackage -packageName "Microsoft.CrmSdk.XrmTooling.ConfigurationMigration.Wpf" -folderName "ConfigurationMigration"

##
##Remove NuGet.exe
##
Remove-Item nuget.exe    