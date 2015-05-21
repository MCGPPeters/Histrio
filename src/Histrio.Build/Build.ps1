param(
    [int]$buildNumber = 0,
    [string]$task = "default",
    [string]$nugetSource = "http://www.nuget.org/api/v2"
    )

$rootDir                = Resolve-Path ..\..\
$srcDir                 = "$rootDir\src"
$packagesDir            = "$srcDir\packages"
$solutionFilePath       = "$srcDir\$projectName.sln"
$nugetPath              = FindTool "NuGet.CommandLine.*\tools\nuget.exe" "$packagesDir"

if(Test-Path Env:\APPVEYOR_BUILD_NUMBER){
    $buildNumber = [int]$Env:APPVEYOR_BUILD_NUMBER
    Write-Output "Using APPVEYOR_BUILD_NUMBER"
}

"Build number $buildNumber"

exec { .$nugetPath restore src\Histrio.sln }

Import-Module .\src\packages\psake.4.4.2\tools\psake.psm1

Invoke-Psake .\default.ps1 $task -framework "4.5.1x64" -properties @{ buildNumber=$buildNumber; nugetSource=$nugetSource }

Remove-Module psake

function FindTool {
    param(
        [string]$name,
        [string]$packageDir
    )

    $result = Get-ChildItem "$packageDir\$name" | Select-Object -First 1

    return $result.FullName
}
