using Cosmos.Example.Shared.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using static Cosmos.Example.Shared.Constants.Cosmos;

namespace Cosmos.Example.Feeed.Functions;

public static class ReadPeopleNames
{
    [FunctionName("ReadPeopleNames")]
    public static void Run([CosmosDBTrigger(
        databaseName: DATABASE_NAME,
        containerName: PEOPLE_CONTAINER_NAME,
        Connection = "AZURE_COSMOS_DB_CONNECTION_STRING",
        LeaseContainerName = LEASE_CONTAINER_NAME,
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<Person> people,
        ILogger logger)
    {
        if (people is not null && people.Count > 0)
        {
            logger.LogInformation("[READING PEOPLE FIRST NAMES]\tDocuments modified: {count}", people.Count);
            foreach (var person in people)
            {
                logger.LogInformation("[PERSON FIRST NAME - {id}]\t{firstName}", person.id, person.firstName);
            }
        }
    }
}