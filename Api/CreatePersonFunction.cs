using Cosmos.Example.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using static Cosmos.Example.Shared.Constants.Cosmos;

namespace Cosmos.Example.Api;

public class CreatePerson
{
    private readonly ILogger _logger;

    public CreatePerson(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<CreatePerson>();
    }

    [Function("CreatePerson")]
    [CosmosDBOutput(
        databaseName: DATABASE_NAME,
        containerName: PEOPLE_CONTAINER_NAME,
        Connection = "COSMOSDB:CONNECTIONSTRING",
        CreateIfNotExists = true)]
    public async Task<Person> Run(
        [HttpTrigger(
            authLevel: AuthorizationLevel.Anonymous, 
            methods: "post")] HttpRequestData request)
    {
        Person? person = await request.ReadFromJsonAsync<Person>();

        if (person is not null)
        {
            _logger.LogInformation("[CREATING PERSON]\t{person}", person);
            return person;
        }
        else
        {
            throw new ArgumentNullException(nameof(person));
        }
    }
}