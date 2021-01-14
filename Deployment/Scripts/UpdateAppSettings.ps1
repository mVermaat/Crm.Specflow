######################### Parameters #########################
param(
[string]$url,
[string]$username,
[string]$password,
[string]$clientId,
[string]$clientSecret,
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
$usernameNode = ($content.appSettings.add | where { $_.key -eq "Username" })
$passwordNode = ($content.appSettings.add | where { $_.key -eq "Password" })
$clientIdNode = ($content.appSettings.add | where { $_.key -eq "ClientId" })
$clientSecretNode = ($content.appSettings.add | where { $_.key -eq "ClientSecret" })

$urlNode.value = $url
$usernameNode.value = $username
$passwordNode.value = $password
$clientIdNode.value = $clientId
$clientSecretNode.value = $clientSecret

$content.Save($fullPath)
