namespace Cosmos.Example.Api.Options;

public record CosmosDb
{
    public required string ConnectionString { get; init; }
}