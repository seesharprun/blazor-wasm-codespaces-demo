using Cosmos.Example.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static Cosmos.Example.Shared.Constants.Cosmos;

namespace Cosmos.Example.Api
{
    public class ReadPeopleNames
    {
        private readonly ILogger _logger;

        public ReadPeopleNames(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ReadPeopleNames>();
        }

        [Function("ReadPeopleNames")]
        public void Run([CosmosDBTrigger(
            databaseName: DATABASE_NAME,
            collectionName: PEOPLE_CONTAINER_NAME,
            ConnectionStringSetting = "COSMOSDB:CONNECTIONSTRING",
            LeaseCollectionName = LEASE_CONTAINER_NAME, 
            CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<Person> people)
        {
            if (people is not null && people.Count > 0)
            {
                _logger.LogInformation("[READING PEOPLE FIRST NAMES]\tDocuments modified: {count}", people.Count);
                foreach (var person in people)
                {
                    _logger.LogInformation("[PERSON FIRST NAME - {id}]\t{firstName}", person.Id, person.FirstName);
                }
            }
        }
    }
}
