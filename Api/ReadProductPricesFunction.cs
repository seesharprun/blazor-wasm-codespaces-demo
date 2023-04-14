using Cosmos.Example.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static Cosmos.Example.Shared.Constants.Cosmos;

namespace Cosmos.Example.Api
{
    public class ReadProductPrices
    {
        private readonly ILogger _logger;

        public ReadProductPrices(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ReadProductPrices>();
        }

        [Function("ReadProductPrices")]
        public void Run([CosmosDBTrigger(
            databaseName: DATABASE_NAME,
            containerName: PRODUCTS_CONTAINER_NAME,
            Connection = "COSMOSDB:CONNECTIONSTRING",
            LeaseContainerName = LEASE_CONTAINER_NAME,
            LeaseContainerPrefix = nameof(ReadProductPrices),
            CreateLeaseContainerIfNotExists = true)] IReadOnlyList<Product> products)
        {
            if (products is not null && products.Count > 0)
            {
                _logger.LogInformation("[READING PRODUCT PRICES]\tDocuments modified: {count}", products.Count);
                foreach (var product in products)
                {
                    _logger.LogInformation("[PRODUCT PRICE - {id}]\t{price}", product.id, product.price);
                }
            }
        }
    }
}
