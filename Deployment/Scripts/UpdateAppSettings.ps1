######################### Parameters #########################
param(
[string]$url,
[string]$username,
[string]$password,
[string]$clientId,
[string]$clientSecret,
[string]$authType,
[string]$appsettingsPath
)

######################### Script #########################

Set-Location $PSScriptRoot
Write-Host "Working Directory: $PSScriptRoot"
Write-Host "App Settings path: $appsettingsPath"

$fullPath = Resolve-Path -Path $appsettingsPath
Write-Host "Full Path: $fullPath"

[xml]$content = Get-Content $fullPath

$urlNode = ($content.appSettings.add | where { $_.key -eq "Url" })
$clientIdNode = ($content.appSettings.add | where { $_.key -eq "ClientId" })
$clientSecretNode = ($content.appSettings.add | where { $_.key -eq "ClientSecret" })
$authTypeNode = ($content.appSettings.add | where { $_.key -eq "AuthType" })


$urlNode.value = $url
$clientIdNode.value = $clientId
$clientSecretNode.value = $clientSecret
$authTypeNode.value = $authType

$content.Save($fullPath)
