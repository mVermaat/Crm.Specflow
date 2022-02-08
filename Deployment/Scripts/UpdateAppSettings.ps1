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

######################### Functions #########################

function Set-AppSetting {
	param(
		[string]$key,
		[string]$value
	)
	$node = ($content.appSettings.add | where { $_.key -eq $key })

	if($node -ne $null -and $value -ne $null) {
		Write-Host "Updating $key"
		$node.value = $value
	}
}

######################### Script #########################

Set-Location $PSScriptRoot
Write-Host "Working Directory: $PSScriptRoot"
Write-Host "App Settings path: $appsettingsPath"

$fullPath = Resolve-Path -Path $appsettingsPath
Write-Host "Full Path: $fullPath"

[xml]$content = Get-Content $fullPath

Set-AppSetting -key "Url" -value $url
Set-AppSetting -key "Username" -value $username
Set-AppSetting -key "Password" -value $password
Set-AppSetting -key "ClientId" -value $clientId
Set-AppSetting -key "ClientSecret" -value $clientSecret
Set-AppSetting -key "AuthType" -value $authType 

$content.Save($fullPath)
