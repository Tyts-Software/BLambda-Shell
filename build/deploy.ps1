param (
	[Alias('p')][Parameter(Mandatory=$true)]
	$profile='shell-dev', 
	$deployment='dev',
	$component,
	$domain,
	$configuration='Release', # [Debug | Release]
	$logLevel='Information', # [Debug | Information | Warning]
	[Alias('email')][String[]]
	$alertSubscribers=@('bobac@tyts.com.ua'), #coma-separated email list
	[Alias('ui')][switch]$isLanding,	# re-publish Landing UI (shall)
	
	[Alias('api')][switch]$isApi,		# re-build & re-package API lambda
	[Alias('app')][switch]$isApp,		# BL lambdas

	[Alias('cleanup')][switch]$isCleanup,	# clean up ALL binaries
	[Alias('prod')][switch]$isProd		# skip all dev only things. use for real production deploymant
)

####### FOR Dev
### .\deploy.ps1 -p shell-dev -ui

## local publish UI
# dotnet publish c:\Work\My\project\tyts\blambda.shell\src\Tyts.BLambda.Shell\ --output c:\Work\My\project\tyts\blambda.shell\~deploy\shell-dev\ui\ -c Debug


####### FOR Prod
####### .\deploy.ps1 -prod -p shell-prod -email admin@tyts.com.ua


## RULES
## 0 APP
$app = "shell"
## one account - one $upix
$upix = ($component -ne $null) ? "$app-$component" : "$app"
## 1 LOCAL FOLDER structure
$folder = ($deployment -ne $null) ? "$upix-$deployment" : "$upix"
##   ~deploy\$folder
##   locally we have 
##   ~deploy\
##      qqq-dev
##      qqq-prod
##      qqq-prod-x
## so it allow to have locally several deployments of one $upix
## 2 STACK
$stackName = "$upix"

## !!!
## due SSM name collisions and others it is not possible to have several copies of $upix in one accont


#colored output helper
function Console([string]$msg) { Write-Host; Write-Host -BackgroundColor DarkGray -noNewLine $msg; Write-Host; }

#cleanup pervious error state
$LASTEXITCODE = 0

if ($isProd) {
	$isLanding = $True
	
	$isApi = $True
	$isApp = $True

	$isCleanup = $True
	$logLevel='Warning'
}


$srcPath =  Resolve-Path "..\src"
$componentsPath = "$srcPath\components"

$deploy = "..\~deploy\$folder"
if (!(Test-Path $deploy -PathType Container)) {
    New-Item -ItemType Directory -Force -Path $deploy
}
$deploy = Resolve-Path $deploy
$deployAws = "$deploy\provision"
$deployPackages = "$deployAws\packages"
$deployTemplates = "$deployAws\templates"

Console "Set profile: $profile"
$env:AWS_PROFILE ="$profile"
$env:SAM_CLI_TELEMETRY =0


$confirmation = Read-Host "Really? [y]"
if ($confirmation -eq 'y') { 

#### Re-Build projects
#
if ($isCleanup) {
	#clean up binaries
	Console "Cleaning up binaries..."
	Get-ChildItem $componentsPath -include bin,obj -Recurse | ForEach-Object ($_) { Write-Host $_.FullName; Remove-Item $_.FullName -Force -Recurse  -ErrorAction SilentlyContinue | Out-Null }
	Remove-Item -LiteralPath $deployPackages -Force -Recurse  -ErrorAction SilentlyContinue | Out-Null
}

$packResult

#if ($isApi) {
#	$libName = "Tyts.NoCms.WebApi.SiteManagement"
#	$libPath = "$componentsPath\SiteManagement`\$libName"
#
#	Console "Rebuild SiteManagement API (dotnet publish inside): $libPath " 
#	$packResult += dotnet lambda package `
#		--project-location $libPath `
#		--configuration $configuration `
#		--framework "net6.0" `
#		--function-architecture "x86_64" `
#		--package-type "zip" `
#		--output-package "$deployPackages\$libName.zip" ` *>&1
#}

#if ($isApp)
#{
#	$libName = "Tyts.NoCms.CreateSite.Lambda"
#	$libPath = "$componentsPath\SiteManagement`\$libName"
#
#	Console "Rebuild CreateSite.Lambda (dotnet publish inside): $libPath " 
#	$packResult += dotnet lambda package `
#		--project-location $libPath `
#		--configuration $configuration `
#		--framework "net6.0" `
#		--function-architecture "x86_64" `
#		--package-type "zip" `
#		--output-package "$deployPackages\$libName.zip" ` *>&1
#		#--msbuild-parameters "/t:Rebuild=true" `
#}

# Evaluate success/failure
if($LASTEXITCODE -eq 0)
{
	# Success
	
	### CREATE stacks
	Console "Creating deploy templates..."
	#todo: should we clean previous?
	#Remove-Item -LiteralPath $templates -Force -Recurse  -ErrorAction SilentlyContinue | Out-Null


	$cdkPath = "$srcPath\.cdk"
	Console "Deploying stack: $cdkPath"
	Push-Location -Path $cdkPath

	#cdk synth	--all --json `
	#			--no-path-metadata --no-asset-metadata --no-version-reporting `
	#			--outputs-file "$deploy\result.json" `
	#			--output=$deployTemplates `
	#			--context component=$component `
	#			--context log-level=$logLevel `
	#			--context bot-package=$botPackage `
	#			--context will-package=$willPackage

	cdk deploy	--all --json `
				--no-path-metadata --no-asset-metadata --no-version-reporting `
				--outputs-file "$deploy\result.json" `
				--output=$deployTemplates `
				--context deployment=$deployment `
				--context component=$component `
				--context domain=$domain `
				--context log-level=$logLevel `
				--context alert-subscribers=$alertSubscribers `
				--context deploy-path=$deployPackages 
				#--context site-management-api-package=$apiPackage `
				#--context will-package=$willPackage

	Pop-Location #-PassThru

	#. .\DeployLanding.ps1
	#Deploy-Landing -on:$isLanding -c:$configuration -srcPath:$srcPath -stackName:$stackName -deployPath:$deploy
	
	#. .\PostDeploySiteManagement.ps1
	#PostDeploy-SiteManagement -on:$true -srcPath:$srcPath -stackName:$stackName
}
else
{
	# Failed, you can reconstruct stderr strings with:
	$ErrorString = $packResult -join [System.Environment]::NewLine
	Write-Host -BackgroundColor DarkRed $ErrorString
}


}
