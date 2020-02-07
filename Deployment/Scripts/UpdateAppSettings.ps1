######################### Parameters #########################
param(
[string]$url,
[string]$username,
[string]$password,
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

$urlNode.value = $url
$usernameNode.value = $username
$passwordNode.value = $password

$content.Save($fullPath)
