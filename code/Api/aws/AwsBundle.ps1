
. "$PSScriptRoot/RemoveFolder.ps1"

$rootDir = Convert-Path $PSScriptRoot/..
Write-Host "Info: root dir: $rootDir" -ForegroundColor Green
$projectFile = "$rootDir/Api.csproj"
$outputDir = "$rootDir/output"
$bundleDir = "$rootDir/.."
$zipFile = "$bundleDir/output.zip"
$bundleFile = "$bundleDir/bundle.zip"
$manifestFile = "$bundleDir/aws-windows-deployment-manifest.json"

dotnet restore $projectFile
Remove-Folder -FolderDir $outputDir
dotnet publish -o $outputDir $projectFile
Remove-Folder -FolderDir $zipFile
Compress-Archive -Path "$outputDir\*" -DestinationPath $zipFile -Force
Remove-Folder -FolderDir $bundleFile
Write-Host "Info: Bundle file is here: $bundleFile" -ForegroundColor Green
Compress-Archive -Path $zipFile, $manifestFile -DestinationPath $bundleFile -Force
