using Cosmos.Example.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static Cosmos.Example.Shared.Constants.Cosmos;

namespace Cosmos.Example.Api
{
    public class ReadProductNames
    {
        private readonly ILogger _logger;

        public ReadProductNames(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ReadProductNames>();
        }

        [Function("ReadProductNames")]
        public void Run([CosmosDBTrigger(
            databaseName: DATABASE_NAME,
            containerName: PRODUCTS_CONTAINER_NAME,
            Connection = "COSMOSDB:CONNECTIONSTRING",
            LeaseContainerName = LEASE_CONTAINER_NAME,
            LeaseContainerPrefix = nameof(ReadProductNames),
            CreateLeaseContainerIfNotExists = true)] IReadOnlyList<Product> products)
        {
            if (products is not null && products.Count > 0)
            {
                _logger.LogInformation("[READING PRODUCT NAMES]\tDocuments modified: {count}", products.Count);
                foreach (var product in products)
                {
                    _logger.LogInformation("[PRODUCT NAME - {id}]\t{name}", product.id, product.Name);
                }
            }
        }
    }
}
