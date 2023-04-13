using Cosmos.Example.Api.Options;
using Cosmos.Example.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((services) =>
    {
        services.AddOptions<CosmosDb>().BindConfiguration(nameof(CosmosDb));
        services.AddSingleton<ICosmosService, CosmosService>((provider) =>
        {
            var cosmosDbOptions = provider.GetRequiredService<IOptions<CosmosDb>>();
            if (cosmosDbOptions is null)
            {
                throw new ArgumentException($"{nameof(IOptions<CosmosDb>)} was not resolved through dependency injection.");
            }
            else
            {
                return new CosmosService(cosmosDbOptions.Value?.ConnectionString ?? String.Empty);
            }
        });
    })
    .Build();

await host.RunAsync();