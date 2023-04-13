using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Cosmos.Example.Api.Services;
using Cosmos.Example.Shared.Models;

namespace Cosmos.Example.Api;

public class CreatePerson
{
    private readonly ILogger _logger;
    private readonly ICosmosService _cosmosService;

    public CreatePerson(ILoggerFactory loggerFactory, ICosmosService cosmosService)
    {
        _logger = loggerFactory.CreateLogger<CreatePerson>();
        _cosmosService = cosmosService;
    }

    [Function("CreatePerson")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request)
    {
        Person? person = await request.ReadFromJsonAsync<Person>();

        if (person is not null)
        {
            _logger.LogInformation("New person op:\t{id}", person.Id);

            Person result = await _cosmosService.CreatePersonAsync(person);

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