using Microsoft.Extensions.DependencyInjection;
using Tyts.BLambda.Blazor.Application.Module;
using Tyts.BLambda.Shell.Modules.Demo.Notifier;

[assembly: Module(
    name: "The second BLambda Shell Module",
    description: "This is just demonstation of the BLambda Shell modularity.",
    author: "Tyts"
)]

[assembly: Feature(
    name: "Notifier",
    description: "Demo of the component re-rendering based on external event."
)]

namespace Tyts.BLambda.Shell.Modules.Demo;

public sealed class Module : IStartup
{
    //[ModuleInitializer]
    //internal static void Initialize()
    //{
    //    //var serviceProvider = new ServiceCollection()
    //    //    .AddNotifierFeature()
    //    //    .BuildServiceProvider();
    //}

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddNotifierFeature();
    }
}
