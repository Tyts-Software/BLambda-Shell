using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.Versioning;
using Tyts.BLambda.Blazor;
using Tyts.BLambda.Blazor.Application.Template;
using Tyts.BLambda.Blazor.Auth.Cognito;
using Tyts.BLambda.Configuration;

namespace Tyts.BLambda.Shell.Cognito;

[SupportedOSPlatform("browser")]
public static class Startup
{
    public static BLambdaStartupConfigurator AddCognito(this BLambdaStartupConfigurator builder)
    {
        builder.AddInterop();

        // add embeded 'secrets' appsettings from Entry Assembly
        builder.builder.Configuration.AddEmbeddedSettings(
                assembly: Assembly.GetEntryAssembly(),
                resourceName: "wwwroot.appsettings",
                environment: builder.builder.HostEnvironment.Environment);

        // AUTH
        builder.builder.AddCognito();

        return builder;
    }
}
