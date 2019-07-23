function Remove-Folder {
  Param(
      [parameter(Mandatory = $true)]
      [string]
      $FolderDir
  )

  if ((Test-Path -path $FolderDir)) {
      Write-Host "Info: Clearing $FolderDir folder." -ForegroundColor Green
      Remove-Item -Recurse -Force $FolderDir
  }
}
