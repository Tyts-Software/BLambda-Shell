using Microsoft.Extensions.DependencyInjection;
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
    //[ModuleInitializer]
    //internal static void Initialize()
    //{
    //    //var serviceProvider = new ServiceCollection()
    //    //    .BuildServiceProvider();
    //}

    public void ConfigureServices(IServiceCollection services)
    {
        
    }
}
