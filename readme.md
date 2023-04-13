# Example ASP.NET Blazor WebAssembly + Azure Cosmos DB for NoSQL + Azure Static Web Apps

## Get started

1. Create a `src/Cosmos.Example.Client/wwwroot/appsettings.json` file with this content:

    ```json
    {
        "Api": {
            "Endpoint": "http://localhost:7071"
        }
    }
    ```

1. Run these commands:

    ```bash
    npm install -g @azure/static-web-apps-cli

    swa init

    swa start
    ```
