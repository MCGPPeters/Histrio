properties {
    $projectName            = "Histrio"
    $buildNumber            = 0
    $rootDir                = Resolve-Path .\
    $buildOutputDir         = "$rootDir\build"
    $mergedDir              = "$buildOutputDir\merged"
    $reportsDir             = "$buildOutputDir\reports"
    $srcDir                 = "$rootDir\src"
	$toolsDir               = "$rootDir\tools"
    $packagesDir            = "$srcDir\packages"
    $solutionFilePath       = "$srcDir\$projectName.sln"
    $assemblyInfoFilePath   = "$srcDir\SharedAssemblyInfo.cs"
	$nugetPath              = "$toolsDir\nuget.exe"
    $ilmergePath            = FindTool "ILRepack.*\tools\ILRepack.exe" "$packagesDir"
    $xunitRunner            = FindTool "xunit.runner.console.*\tools\xunit.console.exe" "$packagesDir"
    $nugetSource            = "http://www.nuget.org/api/v2"
    $script:errorOccured    = $false
}

TaskSetup {
    $taskName = $($psake.context.Peek().currentTaskName)
}

TaskTearDown {
    $taskName = $($psake.context.Peek().currentTaskName)
}

task default -depends Clean, UpdateVersion, RunTests, CreateNuGetPackages, AssertBuildResult

task Clean {
    Remove-Item $buildOutputDir -Force -Recurse -ErrorAction SilentlyContinue
    exec { msbuild /nologo /verbosity:quiet $solutionFilePath /t:Clean /p:platform="Any CPU"}
}

task RestoreNuget {
    "Using nuget source $nugetSource"
    Get-PackageConfigs |% {
        "Restoring " + $_
        &$nugetPath install $_ -o "$srcDir\packages" -configfile $_ -source $nugetSource
    }
}

task UpdateVersion {
    $version = Get-Version $assemblyInfoFilePath
    $oldVersion = New-Object Version $version
    $newVersion = New-Object Version ($oldVersion.Major, $oldVersion.Minor, $oldVersion.Build, $buildNumber)
    Update-Version $newVersion $assemblyInfoFilePath
}

task Compile {
    exec { msbuild /nologo /verbosity:quiet $solutionFilePath /p:Configuration=Release /p:platform="Any CPU"}
}

task RunTests -depends ILMerge {
    New-Item $reportsDir\xUnit\$project -Type Directory -ErrorAction SilentlyContinue
    .$xunitRunner "$srcDir\Histrio.Tests\bin\Release\Histrio.Tests.dll" -html "$reportsDir\xUnit\$project\index.html"
}

task ILMerge -depends Compile {
    New-Item $mergedDir -Type Directory -ErrorAction SilentlyContinue

    $dllDir = "$srcDir\Histrio.Net.Http\bin\Release"
    $inputDlls = "$dllDir\Histrio.Net.Http.dll"
    @(  "Microsoft.Owin",
        "Newtonsoft.Json",
        "System.Net.Http.Formatting",
        "System.Web.Http",`
        "System.Web.Http.Owin") |% { $inputDlls = "$inputDlls $dllDir\$_.dll" }
    Invoke-Expression "$ilmergePath /targetplatform:v4 /internalize /allowDup /target:library /log /out:$mergedDir\Histrio.Net.Http.dll $inputDlls"
}

task CreateNuGetPackages -depends ILMerge {
    $versionString = Get-Version $assemblyInfoFilePath
    $version = New-Object Version $versionString
    $packageVersion = $version.Major.ToString() + "." + $version.Minor.ToString() + "." + $version.Build.ToString() + "." + $buildNumber.ToString()
    $packageVersion
    gci $srcDir -Recurse -Include *.nuspec | % {
        exec { .$nugetPath pack $_ -o $buildOutputDir -version $packageVersion }
    }
}

task AssertBuildResult {
    if ($script:errorOccured){
        Throw ("Error: One of the build tasks failed. Please check the output above")
    }
}

function Get-PackageConfigs {
    $packages = gci $srcDir -Recurse "packages.config" -ea SilentlyContinue
    $customPachage = gci $srcDir -Recurse "packages.*.config" -ea SilentlyContinue
    $packages + $customPachage  | foreach-object { $_.FullName }
}

function FindTool {
    param(
        [string]$name,
        [string]$packageDir
    )

    $result = Get-ChildItem "$packageDir\$name" | Select-Object -First 1

    return $result.FullName
}

function Get-Version
{
    param
    (
        [string]$assemblyInfoFilePath
    )
    Write-Host "path $assemblyInfoFilePath"
    $pattern = '(?<=^\[assembly\: AssemblyVersion\(\")(?<versionString>\d+\.\d+\.\d+\.\d+)(?=\"\))'
    $assmblyInfoContent = Get-Content $assemblyInfoFilePath
    return $assmblyInfoContent | Select-String -Pattern $pattern | Select -expand Matches |% {$_.Groups['versionString'].Value}
}

function Update-Version
{
    param
    (
        [string]$version,
        [string]$assemblyInfoFilePath
    )

    $newVersion = 'AssemblyVersion("' + $version + '")';
    $newFileVersion = 'AssemblyFileVersion("' + $version + '")';
    $tmpFile = $assemblyInfoFilePath + ".tmp"

    Get-Content $assemblyInfoFilePath |
        %{$_ -replace 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', $newFileVersion }  | Out-File -Encoding UTF8 $tmpFile

    Move-Item $tmpFile $assemblyInfoFilePath -force
}