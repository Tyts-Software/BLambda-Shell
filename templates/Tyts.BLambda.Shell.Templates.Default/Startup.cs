using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Versioning;
using Tyts.BLambda.Blazor;
using Tyts.BLambda.Blazor.Application.Template;

namespace Tyts.BLambda.Shell.Templates.Default;

[SupportedOSPlatform("browser")]
public static class Startup
{
    public static BLambdaStartupConfigurator AddDefaultTemplate(this BLambdaStartupConfigurator builder, Action<DefaultTemplateOptions>? config = null)
    {
        builder.builder.Services.Configure<DefaultTemplateOptions>(config ?? ((o) => { }));

        builder.builder.Services.AddScoped<ITemplateService, TemplateService>();

        return builder;
    }
}
