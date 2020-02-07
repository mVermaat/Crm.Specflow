. ".\Common.ps1"
$settings = Get-SettingsJson
$connectionString = Build-CrmConnectionString $settings

Foreach($solution in $settings.Solutions)
{
    Write-Host "Processing $solution"
    & $settings.ExtractSolutionScriptPath -connectionString $connectionString -solutionName $solution -UnpackedFilesFolder "$($settings.ExportSolutionOutputPath)\$solution" -CoreToolsPath $settings.CoreToolsDirectory -PackageType "Both" -solutionFile $null -Verbose
}




