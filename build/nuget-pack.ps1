param ($version='', $configuration='Release')


####### FOR Dev
### .\nuget-pack.ps1 -configuration Debug

####### FOR Prod
####### .\nuget-pack.ps1 -version 0.0.0.1


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

	$path =  Resolve-Path ".."
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