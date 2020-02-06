function Get-SettingValue
{
    param(
    [xml]$settings,
    [string]$key
    )

    return ($settings.appSettings.add | where { $_.key -eq $key }).Value
}

#Setup
Set-Location $PSScriptRoot
$configJsonPath = "..\Configuration\Settings.json"
$VerbosePreference="Continue"

#Getting settings
$settings = Get-Content -Path $configJsonPath | ConvertFrom-Json
[xml]$appsettings = Get-Content -Path $settings.AppsettingsPath

$exportDirectory = $settings.ExportSolutionOutputPath
$connectionString = "Url=$(Get-SettingValue $appsettings "Url");AuthType=$(Get-SettingValue $appsettings "AuthType");Username=$(Get-SettingValue $appsettings "Username");Password=$(Get-SettingValue $appsettings "Password")"

Write-Verbose "ConnectionString: $connectionString"
Write-Verbose "Export Directory: $exportDirectory"

Foreach($solution in $settings.Solutions)
{
    & ..\Tools\XrmCIFramework\ExtractSolution.ps1 -connectionString $connectionString -solutionName $solution -UnpackedFilesFolder "$exportDirectory\$solution" -CoreToolsPath "..\Tools\CoreTools" -PackageType "Both" -solutionFile $null
}




