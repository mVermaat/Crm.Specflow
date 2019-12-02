######################### Parameters #########################
param(
[string]$branch,
[string]$buildNumber
)

######################### Script #########################

$branchName = $branch.Replace("refs/heads/","")

if($branchName -eq "master") {
	Write-Host "##vso[task.setvariable variable=NuGetVersion;]$buildNumber"  
}
else {
	Write-Host "##vso[task.setvariable variable=NuGetVersion;]$buildNumber-Prerelease"  
}
