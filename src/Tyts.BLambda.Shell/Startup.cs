using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Tyts.BLambda.Blazor;
using Tyts.BLambda.Blazor.Auth.Cognito;
using Tyts.BLambda.Blazor.Components;

namespace Tyts.BLambda.Shell;

//[SupportedOSPlatform("browser")]
public static class Startup
{
    public static async Task AddBLambdaShell(this WebAssemblyHostBuilder builder, Action<BLambdaShellStartupConfigurator>? configure = null)
    {
        await builder.AddBLambda(blambda =>
        {
            blambda
                .AddDefault()
                .AddControls();

            configure?.Invoke(new BLambdaShellStartupConfigurator(blambda));
        });
    }

    public static WebAssemblyHost UseBLambdaShell(this WebAssemblyHost app)
    {
        app.UseBLambdaControls();

        return app;
    }
}

public class BLambdaShellStartupConfigurator
{
    public BLambdaStartupConfigurator BLambda { get; }

    internal BLambdaShellStartupConfigurator(BLambdaStartupConfigurator blambdaBuilder)
    {
        BLambda = blambdaBuilder;
    }

    public BLambdaShellStartupConfigurator AddCognito()
    {
        BLambda.AddCognito();

        return this;
    }
}
