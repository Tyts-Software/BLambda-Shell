using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using Tyts.BLambda.Blazor;
using Tyts.BLambda.Blazor.Components;
using Tyts.BLambda.Shell.Cognito;
using Tyts.BLambda.Shell.Templates.Default;

namespace Tyts.BLambda.Shell;

internal static class ShellStartupExtensions
{
    [SupportedOSPlatform("browser")]
    public async static Task AddShell(this WebAssemblyHostBuilder builder)
    {
        //// use appsettings instead
        //#if DEBUG
        //builder.Logging.AddFilter(
        //    "Microsoft.AspNetCore.Components.WebAssembly.Authentication",
        //    LogLevel.Debug);

        //builder.Logging.SetMinimumLevel(LogLevel.Debug);
        //#endif 

        // INTEROP
        //
        await JSHost.ImportAsync("shell", "/js/shell.min.js").ConfigureAwait(false);

        await builder.AddBLambda(blambda =>
        {
            blambda
                .AddDefault()
                .AddCognito()
                .AddControls()
                .AddDefaultTemplate(o =>
                {
                    o.UseLockScreen = false;
                });
        });
    }
}
