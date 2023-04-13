# Example ASP.NET Blazor WebAssembly + Azure Cosmos DB for NoSQL + Azure Static Web Apps

## Get started

1. This demo application requires Azure Cosmos DB for NoSQL to have a ``cosmicworks`` database, ``products`` container, and ``people`` container already existing. (It's a requirement for the Azure Functions to start). You can implement this in the emulator or the live service.

1. Update the `src/Cosmos.Example.Api/local.settings.json` file with your Azure Cosmos DB connection string:

    ```json
    "COSMOSDB__CONNECTIONSTRING": "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
    ```

    > **Tip**: The default is the emulator.

1. Run the Azure Static Web Apps CLI:

    ```bash
    swa start
    ```

    > **Tip**: If you don't have it installed, `npm install -g @azure/static-web-apps-cli`.

## Deploying

1. In Azure, create an Azure Cosmos DB for NoSQL account and get the connection string.

1. Create an Azure Static Web App

1. In the static web app, add the connection string as an application setting named `COSMOSDB__CONNECTIONSTRING`.

1. Get the deployment token from the static web app.

1. Run the Static Web Apps CLI again:

    ```bash
    swa deploy --deployment-token <token> --env production
    ```

    > **Tip**: You may need to provide parameters to deploy such as `--tenant-id` and `--subscription-id`