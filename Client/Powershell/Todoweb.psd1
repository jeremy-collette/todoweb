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
  GUID = '505e8756-d15d-4818-fe53-0824aa44af5e'
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