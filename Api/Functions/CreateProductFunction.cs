using Cosmos.Example.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using static Cosmos.Example.Shared.Constants.Cosmos;

namespace Cosmos.Example.Api;

public class CreateProduct
{
    private readonly ILogger _logger;

    public CreateProduct(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<CreateProduct>();
    }

    [Function("CreateProduct")]
    [CosmosDBOutput(
        databaseName: DATABASE_NAME,
        containerName: PRODUCTS_CONTAINER_NAME,
        Connection = "COSMOSDB:CONNECTIONSTRING",
        CreateIfNotExists = true)]
    public async Task<Product> Run(
        [HttpTrigger(
            authLevel: AuthorizationLevel.Anonymous,
            methods: "post")] HttpRequestData request)
    {
        Product? product = await request.ReadFromJsonAsync<Product>();

        if (product is not null)
        {
            _logger.LogInformation("[CREATING PRODUCT]\t{person}", product);
            return product;
        }
        else
        {
            throw new ArgumentNullException(nameof(product));
        }
    }
}