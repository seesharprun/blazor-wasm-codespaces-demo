using Cosmos.Example.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using static Cosmos.Example.Shared.Constants.Cosmos;

namespace Cosmos.Example.Api.Functions;

public static class CreateProduct
{
    [FunctionName("CreateProduct")]
    public static IActionResult Run(
        [HttpTrigger(
            authLevel: AuthorizationLevel.Anonymous,
            methods: "post")] Product input,
        [CosmosDB(
            databaseName: DATABASE_NAME,
            containerName: PRODUCTS_CONTAINER_NAME,
            Connection = "AZURE_COSMOS_DB_CONNECTION_STRING",
            CreateIfNotExists = true)]out Product output,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(input);

        logger.LogInformation("[CREATING PRODUCT]\t{person}", input);
        output = input;

        return new ObjectResult(input) { StatusCode = StatusCodes.Status201Created };
    }
}