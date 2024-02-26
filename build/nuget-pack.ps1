param (
	[Alias('v')]$version='', 
	[Alias('c')]$configuration='Release'
)


####### FOR Dev
### .\build\nuget-pack.ps1 -c Debug

####### FOR Prod
####### .\build\nuget-pack.ps1 -v 0.0.1-alpha.0


#colored output helper
function Console([string]$msg) { Write-Host; Write-Host -BackgroundColor DarkGray -noNewLine $msg; Write-Host; }

$old_version = $env:RELEASE_VERSION

try {

$env:RELEASE_VERSION = "$version"

cls
Console "Pack Version: $version"
Console "Configuration: $configuration"

$confirmation = Read-Host "Really? [y]"
if ($confirmation -eq 'y') { 

	$path =  Resolve-Path "."
	Push-Location -Path $path
	
	dotnet pack --force -c $configuration 

	Pop-Location 
}

}
#catch {
#  Write-Host "An error occurred:"
#  Write-Host $_.ScriptStackTrace
#}
finally {
  $env:RELEASE_VERSION = $old_version
}