function Get-AppSettingValue
{
    param(
    [xml]$settings,
    [string]$key
    )

    return ($settings.appSettings.add | where { $_.key -eq $key }).Value
}

function Get-SettingsJson
{
    return Get-Content -Path "..\Configuration\Settings.json" | ConvertFrom-Json
}

function Build-CrmConnectionString
{
    param($jsonSettings)

    $appsettings = Get-Content -Path $jsonSettings.AppsettingsPath
    return "Url=$(Get-AppSettingValue $appsettings "Url");AuthType=$(Get-AppSettingValue $appsettings "AuthType");ClientId=$(Get-AppSettingValue $appsettings "ClientId");ClientSecret=$(Get-AppSettingValue $appsettings "ClientSecret")"
}
