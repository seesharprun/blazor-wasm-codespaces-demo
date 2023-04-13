using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Cosmos.Example.Shared.Models;
using static Cosmos.Example.Shared.Constants.Cosmos;

namespace Cosmos.Example.Api.Services;

public interface ICosmosService
{
    Task<Product> CreateProductAsync(Product product);

    Task<Person> CreatePersonAsync(Person person);
}

public sealed class CosmosService : ICosmosService
{
    private readonly CosmosClient _client;

    private Container? _productsContainer;
    private Container? _peopleContainer;

    public CosmosService(string connectionString)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(connectionString);

        CosmosSerializationOptions serializationOptions = new()
        {
            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
        };
        _client = new CosmosClientBuilder(connectionString)
            .WithSerializerOptions(serializationOptions)
            .Build();
    }

    private async Task<Container> InitializeProductsContainerAsync()
    {
        Database database = await _client.CreateDatabaseIfNotExistsAsync(DATABASE_NAME, throughput: 400);
        return await database.CreateContainerIfNotExistsAsync(PRODUCTS_CONTAINER_NAME, PRODUCTS_CONTAINER_PARTITION_KEY_PATH);
    }

    private async Task<Container> InitializePeopleContainerAsync()
    {
        Database database = await _client.CreateDatabaseIfNotExistsAsync(DATABASE_NAME, throughput: 400);
        return await database.CreateContainerIfNotExistsAsync(PEOPLE_CONTAINER_NAME, PEOPLE_CONTAINER_PARTITION_KEY_PATH);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        return await (_productsContainer ??= await InitializeProductsContainerAsync())
            .CreateItemAsync(product, new PartitionKey(product.Category));
    }

    public async Task<Person> CreatePersonAsync(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);
        return await (_peopleContainer ??= await InitializePeopleContainerAsync())
            .CreateItemAsync(person, new PartitionKey(person.LastName));
    }
}