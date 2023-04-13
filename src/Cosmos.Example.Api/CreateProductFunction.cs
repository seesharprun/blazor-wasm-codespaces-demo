using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Cosmos.Example.Api.Services;
using Cosmos.Example.Shared.Models;

namespace Cosmos.Example.Api;

public class CreateProduct
{
    private readonly ILogger _logger;
    private readonly ICosmosService _cosmosService;

    public CreateProduct(ILoggerFactory loggerFactory, ICosmosService cosmosService)
    {
        _logger = loggerFactory.CreateLogger<CreateProduct>();
        _cosmosService = cosmosService;
    }

    [Function("CreateProduct")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")]HttpRequestData request)
    {
        _logger.LogInformation("Create new person triggered");

        Product? product = await request.ReadFromJsonAsync<Product>();

        if (product is not null)
        {
            Product result = await _cosmosService.CreateProductAsync(product);

            var response = request.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(result);

            return response;
        }
        else
        {
            return request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}