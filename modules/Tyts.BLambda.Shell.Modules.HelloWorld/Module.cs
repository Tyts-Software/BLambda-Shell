using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tyts.BLambda.Blazor.Application.Module;

[assembly: Module(
    name: "The first BLambda Shell Module",
    description: "This is just 'Hola Mundo' demonstation of the BLambda Shell modularity.",
    author: "Tyts"
)]

[assembly: Feature(
    name: "Home",
    description: "Welcome Page"
)]
[assembly: Feature(
    name: "Counter",
    description: "Counter Page"
)]
[assembly: Feature(
    name: "Weather",
    description: "Weather Forecast Page"
)]

namespace Tyts.BLambda.Shell.Modules.Demo;

public sealed class Module : IStartup
{
    private readonly IWebAssemblyHostEnvironment env;
    //private readonly IConfiguration configuration;

    public Module(
        IConfiguration configuration,
        IWebAssemblyHostEnvironment env
        )
    {
        this.env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var http = new HttpClient()
        {
            BaseAddress = new Uri(env.BaseAddress)
        };
        services.TryAddSingleton(sp => http);
    }
}
