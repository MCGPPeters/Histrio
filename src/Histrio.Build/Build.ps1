param(
    [int]$buildNumber = 0,
    [string]$task = "default",
    [string]$nugetSource = "http://www.nuget.org/api/v2"
    )

if(Test-Path Env:\APPVEYOR_BUILD_NUMBER){
    $buildNumber = [int]$Env:APPVEYOR_BUILD_NUMBER
    Write-Output "Using APPVEYOR_BUILD_NUMBER"
}

"Build number $buildNumber"

..\..\tools\Nuget.exe restore ..\..\src\Histrio.sln

Import-Module .\src\packages\psake.4.4.2\tools\psake.psm1

Invoke-Psake .\default.ps1 $task -framework "4.5.1x64" -properties @{ buildNumber=$buildNumber; nugetSource=$nugetSource }

Remove-Module psake
