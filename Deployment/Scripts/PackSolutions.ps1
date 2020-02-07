######################### Parameters #########################
######################### Script #########################


. ".\Common.ps1"
$settings = Get-SettingsJson

Foreach($solution in $settings.Solutions)
{
    Write-Host "Processing $solution"
    & $settings.PackSolutionScriptPath -UnpackedFilesFolder "$($settings.ExportSolutionOutputPath)\$solution" -PackageType "Both" -CoreToolsPath $settings.CoreToolsDirectory -OutputPath ".\"
}


