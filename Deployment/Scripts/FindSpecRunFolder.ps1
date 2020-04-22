######################### Parameters #########################
param(
[string]$packageDirectory
)

######################### Script #########################
$result = Get-ChildItem -Path "$packageDirectory\specrun.runner" -Directory

if($result.Length -eq 0) {
    Write-Host "SpecRun foulder not found"
    exit 1; 
}
$highest = $null
$highestVersion = $null

for($i = 0; $i -lt $result.Length;$i++) {
    $version = [Version]::new($result[$i].Name)

    if($highestVersion -eq $null -or $highestVersion -lt $version) {
        $highestVersion = $version
        $highest = $result[$i]
    }
}

Write-Host "Setting variable to $($highest.FullName)"
Write-Host "##vso[task.setvariable variable=SpecRunFolder;]$($highest.FullName)"  

