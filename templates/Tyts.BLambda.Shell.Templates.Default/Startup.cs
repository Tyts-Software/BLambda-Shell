using Microsoft.Extensions.DependencyInjection;
using Tyts.BLambda.Blazor.Application.Template;

namespace Tyts.BLambda.Shell.Templates.Default;

//[SupportedOSPlatform("browser")]
public static class Startup
{
    public static BLambdaShellStartupConfigurator AddDefaultTemplate(this BLambdaShellStartupConfigurator shell, Action<DefaultTemplateOptions>? config = null)
    {
        shell.BLambda.Builder.Services.Configure<DefaultTemplateOptions>(config ?? ((o) => { }));

        shell.BLambda.Builder.Services.AddScoped<ITemplateService, TemplateService>();

        return shell;
    }
}
