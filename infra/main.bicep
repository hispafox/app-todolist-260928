@description('Nombre del recurso App Service para la API de la lista de tareas.')
param appName string

@description('Ubicación de Azure para los recursos.')
param location string = resourceGroup().location

@description('SKU del plan de App Service.')
param skuName string = 'B1'

@description('Versión del runtime de .NET para la aplicación.')
param linuxFxVersion string = 'DOTNETCORE|10.0'

resource appServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: '${appName}-plan'
  location: location
  kind: 'linux'
  sku: {
    name: skuName
    tier: 'Basic'
    capacity: 1
  }
  properties: {
    reserved: true
  }
}

resource webApp 'Microsoft.Web/sites@2023-01-01' = {
  name: appName
  location: location
  kind: 'app,linux'
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: linuxFxVersion
      minTlsVersion: '1.2'
      http20Enabled: true
      ftpsState: 'FtpsOnly'
      appSettings: [
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
      ]
    }
  }
}

output webAppUrl string = 'https://${webApp.properties.defaultHostName}'
output planName string = appServicePlan.name
