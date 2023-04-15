@description('Location where all resources will be deployed. This value defaults to the **West US 2** region.')
@allowed([
  'East US 2'
  'West US 2'
  'Central US'
  'West Europe'
  'East Asia'
])
param location string = 'East US 2'

@description('''
Unique name for the chat application.  The name is required to be unique as it will be used as a prefix for the names of these resources:
- Azure Cosmos DB
- Azure Static Web App
- Azure Functions
The name defaults to a unique string generated from the resource group identifier.
''')
@minLength(5)
@maxLength(15)
param name string = uniqueString(resourceGroup().id)

resource cosmosDbAccount 'Microsoft.DocumentDB/databaseAccounts@2022-08-15' = {
  name: '${name}nosql'
  location: location
  kind: 'GlobalDocumentDB'
  properties: {
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        failoverPriority: 0
        isZoneRedundant: false
        locationName: location
      }
    ]
  }
}

resource cosmosDbDatabase 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2022-08-15' = {
  parent: cosmosDbAccount
  name: 'cosmicworks'
  properties: {
    resource: {
      id: 'cosmicworks'
    }
    options: {
      throughput: 400
    }
  }
}

resource cosmosDbProductsContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2022-08-15' = {
  parent: cosmosDbDatabase
  name: 'products'
  properties: {
    resource: {
      id: 'products'
      partitionKey: {
        paths: [
          '/category'
        ]
      }
    }
  }
}

resource cosmosDbPeopleContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2022-08-15' = {
  parent: cosmosDbDatabase
  name: 'people'
  properties: {
    resource: {
      id: 'people'
      partitionKey: {
        paths: [
          '/lastName'
        ]
      }
    }
  }
}

resource cosmosDbChangeFeedLeaseContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2022-08-15' = {
  parent: cosmosDbDatabase
  name: 'changefeed-leases'
  properties: {
    resource: {
      id: 'changefeed-leases'
      partitionKey: {
        paths: [
          '/id'
        ]
      }
    }
  }
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${name}insights'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Request_Source: 'rest'
  }
}

resource hostingPlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: '${name}plan'
  location: location
  kind: 'linux'
  sku: {
    name: 'B1'
  }
  properties: {
    reserved: true
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: '${name}storage'
  location: location
  kind: 'Storage'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    supportsHttpsTrafficOnly: true
    minimumTlsVersion: 'TLS1_2'
  }
}

resource functionApp 'Microsoft.Web/sites@2022-09-01' = {
  name: '${name}functions'
  location: location
  kind: 'functionapp,linux'
  properties: {
    serverFarmId: hostingPlan.id
    httpsOnly: true
    siteConfig: {}
  }
}

resource functionAppSettings 'Microsoft.Web/sites/config@2022-03-01' = {
  parent: functionApp
  name: 'appsettings'
  kind: 'string'
  properties: {
    AzureWebJobsStorage: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
    WEBSITE_CONTENTAZUREFILECONNECTIONSTRING: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
    APPINSIGHTS_INSTRUMENTATIONKEY: applicationInsights.properties.InstrumentationKey
    APPLICATIONINSIGHTS_CONNECTION_STRING: 'InstrumentationKey=${applicationInsights.properties.InstrumentationKey};IngestionEndpoint=https://eastus2-0.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus2.livediagnostics.monitor.azure.com/'
    FUNCTIONS_EXTENSION_VERSION: '~4'
    FUNCTIONS_WORKER_RUNTIME: 'dotnet'
    PROJECT: 'Api/Cosmos.Example.Api.csproj'
    WEBSITE_CONTENTSHARE: toLower('${name}functionshare')
    AZURE_COSMOS_DB_CONNECTION_STRING: 'AccountEndpoint=${cosmosDbAccount.properties.documentEndpoint};AccountKey=${cosmosDbAccount.listKeys().primaryMasterKey};'
  }
}

resource functionAppConfiguration 'Microsoft.Web/sites/config@2022-09-01' = {
  parent: functionApp
  name: 'web'
  kind: 'string'
  properties: {
    linuxFxVersion: 'DOTNET|6.0'
    minTlsVersion: '1.2'
    alwaysOn: true
    cors: {
      allowedOrigins: [
        'https://${webApp.properties.defaultHostName}'
        'https://portal.azure.com'
      ]
    }
  }
}

resource functionAppDeployment 'Microsoft.Web/sites/sourcecontrols@2022-09-01' = {
  parent: functionApp
  dependsOn: [
    functionAppSettings
    functionAppConfiguration
  ]
  name: 'web'
  properties: {
    repoUrl: 'https://github.com/seesharprun/blazor-wasm-codespaces-demo.git'
    branch: 'main'
    isManualIntegration: true
  }
}

resource webApp 'Microsoft.Web/sites@2022-09-01' = {
  name: '${name}web'
  kind: 'app,linux'
  location: location
  properties: {
    serverFarmId: hostingPlan.id
    httpsOnly: true
  }
}

resource webAppConfiguration 'Microsoft.Web/sites/config@2022-09-01' = {
  parent: webApp
  name: 'web'
  kind: 'string'
  properties: {
    minTlsVersion: '1.2'
    linuxFxVersion: 'DOTNETCORE|7.0'
  }
}

resource webAppSettings 'Microsoft.Web/sites/config@2022-03-01' = {
  parent: webApp
  name: 'appsettings'
  kind: 'string'
  properties: {
    API__ENDPOINT: 'https://${functionApp.properties.defaultHostName}/'
    PROJECT: 'Client/Cosmos.Example.Client.csproj'
  }
}

resource webAppDeployment 'Microsoft.Web/sites/sourcecontrols@2021-03-01' = {
  parent: webApp
  dependsOn: [
    webAppSettings
    webAppConfiguration
  ]
  name: 'web'
  properties: {
    repoUrl: 'https://github.com/seesharprun/blazor-wasm-codespaces-demo.git'
    branch: 'main'
    isManualIntegration: true
  }
}
