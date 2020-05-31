######################### Parameters #########################
param(
[bool]$isRelease,
[string]$testRunTitle
)

######################### Get Test Run ID #########################
$pat = "Bearer $env:System_AccessToken"
$baseUrl = "$($env:SYSTEM_TEAMFOUNDATIONCOLLECTIONURI)$env:SYSTEM_TEAMPROJECTID/_apis/test/runs"
$minUpdate = (Get-Date).AddDays(-1).ToString("yyyy-MM-dd")
$maxUpdate = (Get-Date).AddDays(1).ToString("yyyy-MM-dd")
$buildFilter = ""


if($isRelease -eq $true) {
	$buildFilter = "&releaseIds=$env:RELEASE_RELEASEID&releaseEnvIds=$env:RELEASE_ENVIRONMENTID"
}
else { # As there is no filter parameter for stage/environment id in YAML deployment jobs, use the run title instead
	$buildFilter = "&buildIds=$env:BUILD_BUILDID&runTitle=$testRunTitle"
}

$url = "$($baseUrl)?minLastUpdatedDate=$minUpdate&maxLastUpdatedDate=$maxUpdate$buildFilter&api-version=5.1"
Write-Host "url: $url"

$data = Invoke-RestMethod -Uri "$url" -Headers @{Authorization = $pat} 
Write-Host "Raw data returned from API call: $data"

$testRunId = $data.value[$data.value.Length-1].id
Write-Host "Test Run ID: $testRunId"

######################### Add Screenshots #########################
$testResultFolder = "$env:AGENT_TEMPDIRECTORY\TestResults"
Write-Host "Screenshot folder: $testResultFolder"

$resultData = Invoke-RestMethod -Uri "$baseUrl/$testRunId/results?api-version=5.1" -Headers @{Authorization = $pat} 

Foreach ($result in $resultData.value) {

    $testName = $result.testCaseTitle.Replace(' ', '_').Replace(':','')
    $comma = $testName.LastIndexOf(',')
    if($comma -ne -1) {
        $testName = $testName.Substring(0, $comma)
    }

    $search = "$testResultFolder\error_*_$($testName)_*"
    Write-Host "Processing $testName. Path: $search"

    $files = Get-ChildItem -Path $search -Include *.png
    Write-Host "Found files: $files"
   
    foreach($file in $files) {
        $bytes = [System.IO.File]::ReadAllBytes($file.FullName)
        $base64 = [System.Convert]::ToBase64String($bytes)
        $body = @{stream=$base64;fileName=$file.Name;comment="";attachmentType="GeneralAttachment"} | ConvertTo-Json
        $data = Invoke-RestMethod -Uri "$baseUrl/$testRunId/Results/$($result.id)/attachments?api-version=5.1-preview.1" -Headers @{Authorization = $pat} -Method Post -Body $body -ContentType "application/json"
        Write-Host "Raw data returned from API call: $data"
    }
}



