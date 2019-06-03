@{
# region definition 
  RootModule = './Todoweb.psm1'
  ModuleVersion = '0.1.0'
  CompatiblePSEditions = 'Core', 'Desktop'
  Author = ''
  CompanyName = ''
  Copyright = ''
  Description = ''
  PowerShellVersion = '5.1'
  DotNetFrameworkVersion = '4.7.2'
  RequiredAssemblies = './bin/Todoweb.private.dll'
  FormatsToProcess = './Todoweb.format.ps1xml'
# endregion 

# region persistent data 
  GUID = '64524f43-7684-4978-aea0-cbd65e9ddf7c'
# endregion 

# region private data 
  PrivateData = @{
    PSData = @{
      Tags = ''
      LicenseUri = ''
      ProjectUri = ''
      ReleaseNotes = ''
    }
  }
# endregion 

# region exports
  CmdletsToExport = 'Get-Todo', 'New-Todo', 'Remove-Todo', 'Set-Todo', '*'
  AliasesToExport = '*'
# endregion

}