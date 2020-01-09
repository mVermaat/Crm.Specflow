$result = Get-ChildItem -Path "$env:SYSTEM_DEFAULTWORKINGDIRECTORY\packages" -Directory -Filter "SpecRun.Runner.*"

if($result.Length -gt 0) {
    Write-Host "Setting variable to $($result[0].FullName)"
    Write-Host "##vso[task.setvariable variable=SpecRunFolder;]$($result[0].FullName)"  
}
else {
    Write-Host "SpecRun foulder not found"
    exit 1;
}