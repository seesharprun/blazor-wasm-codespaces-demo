using Cosmos.Example.Client;
using Cosmos.Example.Client.Options;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.Services.AddOptions<Api>().Bind(builder.Configuration.GetSection(nameof(Api)));
builder.Services.AddScoped<HttpClient>((provider) =>
{
    var apiOptions = provider.GetRequiredService<IOptions<Api>>();
    if (apiOptions.Value is null)
    {
        throw new ArgumentException($"{nameof(IOptions<Api>)} was not resolved through dependency injection.");
    }
    else
    {
        string overridenApiEndpoint = apiOptions.Value?.Endpoint ?? String.Empty;
        string baseAddress = String.IsNullOrEmpty(overridenApiEndpoint) ? builder.HostEnvironment.BaseAddress : overridenApiEndpoint;
        return new HttpClient { BaseAddress = new Uri(baseAddress) };
    }
});

await builder.Build().RunAsync();