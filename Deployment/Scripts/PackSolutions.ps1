######################### Parameters #########################
######################### Script #########################

$solutionPath = "..\Solutions\SpecFlowDemo"
$packPath = "..\Tools\XrmCIFramework\PackSolution.ps1"
$coreToolsPath = "..\Tools\CoreTools"

& $packPath -UnpackedFilesFolder $solutionPath -PackageType "Both" -CoreToolsPath $coreToolsPath -OutputPath ".\"