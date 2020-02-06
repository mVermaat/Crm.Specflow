######################### Parameters #########################
param(
[string]$branch,
[string]$buildNumber
)

######################### Script #########################

Write-Host "Branch: $branch"
Write-Host "Build: $buildNumber"


if($branch -eq "refs/heads/master") {
    Write-Host "##vso[task.setvariable variable=NuGetVersion;]$buildNumber"
    Write-Host "##vso[task.setvariable variable=CreateNuGetPackage;]true"    
}
elseif($branch.StartsWith("refs/heads/")) {
    $branchName = $branch.Substring($branch.LastIndexOf('/') + 1)

    Write-Host "##vso[task.setvariable variable=NuGetVersion;]$buildNumber-$branchName"
    Write-Host "##vso[task.setvariable variable=CreateNuGetPackage;]true"     
}
else {
    Write-Host "##vso[task.setvariable variable=CreateNuGetPackage;]false"   
}
