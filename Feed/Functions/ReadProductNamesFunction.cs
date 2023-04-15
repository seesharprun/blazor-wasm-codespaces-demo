using Cosmos.Example.Shared.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using static Cosmos.Example.Shared.Constants.Cosmos;

namespace Cosmos.Example.Feed.Functions;

public static class ReadProductNames
{
    [FunctionName("ReadProductNames")]
    public static void Run([CosmosDBTrigger(
        databaseName: DATABASE_NAME,
        containerName: PRODUCTS_CONTAINER_NAME,
        Connection = "AZURE_COSMOS_DB_CONNECTION_STRING",
        LeaseContainerName = LEASE_CONTAINER_NAME,
        LeaseContainerPrefix = nameof(ReadProductNames),
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<Product> products,
        ILogger logger)
    {
        if (products is not null && products.Count > 0)
        {
            logger.LogInformation("[READING PRODUCT NAMES]\tDocuments modified: {count}", products.Count);
            foreach (var product in products)
            {
                logger.LogInformation("[PRODUCT NAME - {id}]\t{name}", product.id, product.Name);
            }
        }
    }
}