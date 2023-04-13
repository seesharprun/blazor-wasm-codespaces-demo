using Microsoft.Azure.Cosmos;
using Cosmos.Example.Shared.Models;

namespace Cosmos.Example.Api.Services;

public interface ICosmosService
{
    Task<Product> CreateProductAsync(Product product);

    Task<Person> CreatePersonAsync(Person person);
}

public sealed class CosmosService : ICosmosService
{
    private const string DATABASE_NAME = "cosmicworks";
    private readonly CosmosClient _client;

    private Container? _productsContainer;
    private Container? _peopleContainer;

    public CosmosService(string connectionString)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(connectionString);

        _client = new CosmosClient(connectionString);
    }

    private async Task<Container> InitializeProductsContainerAsync()
    {
        Database database = await _client.CreateDatabaseIfNotExistsAsync(DATABASE_NAME, throughput: 400);
        return await database.CreateContainerIfNotExistsAsync("products", "/category");
    }

    private async Task<Container> InitializePeopleContainerAsync()
    {
        Database database = await _client.CreateDatabaseIfNotExistsAsync(DATABASE_NAME, throughput: 400);
        return await database.CreateContainerIfNotExistsAsync("people", "/lastName");
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        return await (_productsContainer ??= await InitializeProductsContainerAsync())
            .CreateItemAsync(product, new PartitionKey(product.category));
    }

    public async Task<Person> CreatePersonAsync(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);
        return await (_peopleContainer ??= await InitializePeopleContainerAsync())
            .CreateItemAsync(person, new PartitionKey(person.lastName));
    }
}