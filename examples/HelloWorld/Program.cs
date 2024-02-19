using HelloWorld;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Diagnostics;
using Tyts.BLambda.Blazor;
using Tyts.BLambda.Blazor.Components;
using Tyts.BLambda.Shell.Templates.Default;

[assembly: System.Runtime.Versioning.SupportedOSPlatformAttribute("browser")]
Debug.Assert(OperatingSystem.IsBrowser());

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


await builder.AddBLambda(blambda =>
{
    blambda
        .AddDefault()
        .AddControls()
        .AddDefaultTemplate();
});

var app = builder.Build();
app.UseBLambdaControls();


await app.RunAsync();

