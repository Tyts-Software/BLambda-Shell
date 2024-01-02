using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using Tyts.BLambda.Blazor.Application;
using Tyts.BLambda.Blazor.Application.Boot;
using Tyts.BLambda.Blazor.Application.Module;
using Tyts.BLambda.Blazor.Components;
using Tyts.BLambda.Blazor.Infrastructure.Browser;
using Tyts.BLambda.Blazor.Auth.Cognito;
using Tyts.BLambda.Blazor.Theme;
using Tyts.BLambda.Blazor;
using Tyts.BLambda.Shell._SiteTemplates;
using Tyts.BLambda.Blazor.Application.Template;

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

        await builder.AddBLambda(configure => 
        {
            configure
                .AddDefault()
                .AddControls();
        });


        builder.Services.AddSingleton<ITemplateService, TemplateService>();
        
    }
}
