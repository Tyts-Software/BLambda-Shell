using Microsoft.Extensions.DependencyInjection;

namespace Tyts.BLambda.Shell.Modules.Demo.Notifier;

public static partial class ServiceCollectionExtensions
{
    internal static IServiceCollection AddNotifierFeature(this IServiceCollection services)
    {
        services.AddSingleton<NotifierService>();
        services.AddSingleton<TimerService>();

        return services;
    }
}
