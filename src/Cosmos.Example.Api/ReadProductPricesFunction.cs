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
            collectionName: PRODUCTS_CONTAINER_NAME,
            ConnectionStringSetting = "COSMOSDB:CONNECTIONSTRING",
            LeaseCollectionName = LEASE_CONTAINER_NAME,
            LeaseCollectionPrefix = nameof(ReadProductPrices), 
            CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<Product> products)
        {
            if (products is not null && products.Count > 0)
            {
                _logger.LogInformation("[READING PRODUCT PRICES]\tDocuments modified: {count}", products.Count);
                foreach (var product in products)
                {
                    _logger.LogInformation("[PRODUCT PRICE - {id}]\t{price}", product.Id, product.Price);
                }
            }
        }
    }
}
