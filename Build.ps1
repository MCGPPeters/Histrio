param(
    $task = "default",
    $gitVersionUserName = "",
    $gitVersionPassword = ""
)

$base_directory = Resolve-Path .
$src_directory = "$base_directory\src"

get-module psake | remove-module

& "$base_directory\src\.nuget\NuGet.exe" install "$base_directory\src\.nuget\packages.config" -OutputDirectory "$base_directory\src\packages"

Import-Module (Get-ChildItem "$base_directory\src\packages\psake.*\tools\psake.psm1" | Select-Object -First 1)

Import-Module $base_directory\IO.psm1
Import-Module $base_directory\teamcity.psm1

Invoke-Psake $base_directory\default.ps1 $task -framework "4.0x64" -properties @{ gitVersionUserName=$gitVersionUserName; gitVersionPassword=$gitVersionPassword }

Remove-Module teamcity
Remove-Module psake
Remove-Module IO