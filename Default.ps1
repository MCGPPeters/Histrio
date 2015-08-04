properties {
    $solution           = "Histrio.sln"
    $target_config      = "Release"

    $base_directory     = Resolve-Path .
    $src_directory      = "$base_directory\src"
    $output_directory   = "$base_directory\build"
    $merged_directory   = "$output_directory\merged"
    $package_directory  = "$src_directory\packages"
    $nuget_directory    = "$src_directory\.nuget"

    $sln_path           = "$src_directory\$solution"
    $assemblyInfo_path  = "$src_directory\SharedAssemblyInfo.cs"
    $nuget_path         = "$nuget_directory\nuget.exe"
    $nugetConfig_path   = "$nuget_directory\nuget.config"
    
    $ilmerge_path       = FindTool("ILRepack.*\tools\ILRepack.exe")
    $gitversion_path    = FindTool("GitVersion.CommandLine.*\tools\GitVersion.exe")
	
    $code_coverage      = $true
    $framework_version  = "v4.5"

    $gitVersionUserName = ""
    $gitVersionPassword = ""
}

TaskSetup {
    $taskName = $($psake.context.Peek().currentTaskName)
    TeamCity-OpenBlock $taskName
    TeamCity-ReportBuildProgress "Running task $taskName"
}

TaskTearDown {
    $taskName = $($psake.context.Peek().currentTaskName)
    TeamCity-CloseBlock $taskName
}

task default -depends Test, Package

task Init -depends Clean, VersionAssembly

task Compile -depends CompileClr

task Test -depends Compile, TestClr

task Clean {
    EnsureDirectory $output_directory

    Clean-Item $output_directory -ea SilentlyContinue
}

task VersionAssembly {
    $version = Get-Version

    if ($version) {
        Write-Output $version

        $assembly_information = "
			using System.Reflection;
						
			[assembly: AssemblyCompany(""Maurice CGP Peters"")]
			[assembly: AssemblyProduct(""Histrio"")]
		    [assembly: AssemblyCopyright(""Copyright © Maurice CGP Peters 2015"")]
		    [assembly: System.Reflection.AssemblyVersion(""$($version.AssemblySemVer)"")]
		    [assembly: System.Reflection.AssemblyFileVersion(""$($version.AssemblySemVer)"")]
		    [assembly: System.Reflection.AssemblyInformationalVersion(""$($version.InformationalVersion)"")]
    ".Trim()
        $assembly_information | Out-File $assemblyInfo_path -Encoding utf8
    } else {
        Write-Output "Warning: could not get assembly information."

        Write-Output "" > $assemblyInfo_path
    }
}



task RestoreNuget {
    Get-SolutionPackages |% {
        "Restoring " + $_
        &$nuget_path install $_ -o $package_directory -configfile $nugetConfig_path
    }
}

task CompileClr -depends RestoreNuget, Init {
    exec { msbuild /t:rebuild /nologo /verbosity:q $sln_path /p:"Configuration=$target_config;TargetFrameworkVersion=$framework_version;OutDir=$output_directory"  }
}

task TestClr -depends CompileClr {
    RunTest -test_project "Histrio"
}

Task ILMerge -depends Compile {
    EnsureDirectory $merged_directory

    $merge = @(
		"Microsoft.Owin",
		"Newtonsoft.Json",
        "System.Net.Http.Formatting",
        "System.Web.Http",`
        "System.Web.Http.Owin"
    )

    ILMerge -target "Histrio.Net.Http" -folder $output_directory -merge $merge
}

Task TestILMerge -depends ILMerge {
    exec {
        @(
            "Histrio.Net.Http"
        ) | %{ 
            $script = {
                $bytes = [System.IO.File]::ReadAllBytes($args[0])
                $assembly = [System.Reflection.Assembly]::Load($bytes)
                
                $types = $assembly.GetTypes() | %{
                    $_.FullName
                }

                $public_types = $assembly.GetExportedTypes() | %{
                    $_.FullName
                }
                
                $count = $types.Length
                $public_count = $public_types.Length

                Write-Output "$_"
                Write-Output "Found $count Types."
                Write-Output "Found $public_count public Types."
                Write-Output "Public Types:"
                Write-Output $public_types 
            }
            Start-Job $script -ArgumentList "$merged_directory\$_.dll"

            while (Get-Job -State "Running") {
                Start-Sleep 1
            }

            Get-Job | Receive-Job
        }
    }
}

task Package -depends ILMerge {
    $version = Get-Version

    if ($version) {

        gci "$src_directory" -Recurse -Include *.nuspec | % {
            exec { 
                & $nuget_path pack $_ -o $output_directory -version $version.NuGetVersionV2 
            }
        }
    } else {
        Write-Output "Warning: could not get version. No packages will be created."
    }
}

function EnsureDirectory {
    param($directory)

    if(!(test-path $directory)) {
        mkdir $directory
    }
}

function Get-SolutionPackages {
    gci $src_directory -Recurse "packages.config" -ea SilentlyContinue | foreach-object { $_.FullName }
}

function RunTest {
    param(
        [string] $test_project
    )
    $testrunner_path = FindTool("xunit.runner.console.*\tools\xunit.console.exe")
	$dotcover_path   = FindTool("JetBrains.dotCover.CommandLineTools.*\tools\dotcover.exe")

    $arguments = "$output_directory\$test_project.Tests.dll -parallel all"

    $has_dot_cover = Test-Path $dotcover_path

    if ($has_dot_cover -eq $false) {
        TeamCity-WriteServiceMessage 'message' @{ text="Code coverage skipped. Could not find dotcover at $dotcover_path" }
    }

    if ($code_coverage -eq $true -and $has_dot_cover -eq $true) {
        $dotcover_snapshot_path = "$output_directory\$test_project.Tests.Coverage.xml"
        $dotcover_filter = "+:$test_project;+:$test_project.*;-:$test_project.Tests.*"
        & $dotcover_path cover /TargetExecutable=$testrunner_path /TargetArguments=$arguments /Output=$dotcover_snapshot_path /ReportType=XML /Filters=$dotcover_filter
        TeamCity-ImportDotNetCoverageResult -tool "dotcover" -path $dotcover_snapshot_path
    } else {
        & $testrunner_path $arguments.Split(' ')
    }
}

function Get-Version {
    TeamCity-WriteServiceMessage 'message' @{ text="getting git version using $($gitversion_path)" }

    $result = Invoke-Expression "$gitversion_path /u $gitVersionUserName /p $gitVersionPassword"

    if ($LASTEXITCODE -ne 0){
        TeamCity-WriteServiceMessage 'message' @{ text="GERROR $($result)" }
    } else{
        return ConvertFrom-Json ($result -join "`n");
    }
}

function ILMerge {
    param(
        [string] $target,
        [string] $folder,
        [string[]] $merge
    )

    "<configuration>
	  <startup>	
		<supportedRuntime version=""v4.0""/>
	  </startup>
	</configuration>" | Out-File -FilePath "$ilmerge_path.config"

    $primary = "$folder\$target.dll"

    $merge = $merge |%  { "$folder\$_.dll" }

    $out = "$merged_directory\$target.dll"
    
    & $ilmerge_path /targetplatform:v4 /wildcards /internalize /allowDup /target:library /log:$output_directory\ilmerge_log.txt /out:$out $primary $merge
}

function FindTool {
    param(
        [string] $name
    )

    $result = Get-ChildItem "$package_directory\$name" | Select-Object -First 1

    return $result.FullName
}