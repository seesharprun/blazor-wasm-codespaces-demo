# Example ASP.NET Blazor WebAssembly + Azure Cosmos DB for NoSQL + Azure Static Web Apps

## Test locally

1. This demo application requires Azure Cosmos DB for NoSQL to have a ``cosmicworks`` database, ``products`` container, and ``people`` container already existing. (It's a requirement for the Azure Functions to start). You can implement this in the emulator or a live service.

1. Update the `src/Api/local.settings.json` file with your Azure Cosmos DB connection string:

    ```json
    "COSMOSDB__CONNECTIONSTRING": "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
    ```

    > **Tip**: The default is the emulator.

1. Run the Azure Static Web Apps CLI:

    ```bash
    swa start
    ```

    > **Tip**: If you don't have it installed, `npm install -g @azure/static-web-apps-cli`.

## Deploy to Azure

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fseesharprun%2Fblazor-wasm-codespaces-demo%2Fmain%2Fazuredeploy.json)
