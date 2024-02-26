using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sandbox;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using Tyts.BLambda.Shell;
using Tyts.BLambda.Shell.Templates.Default;

[assembly: System.Runtime.Versioning.SupportedOSPlatformAttribute("browser")]
Debug.Assert(OperatingSystem.IsBrowser());

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


await JSHost.ImportAsync("shell", "/js/shell.min.js").ConfigureAwait(false);

await builder.AddBLambdaShell(shell =>
{
    shell
        .AddCognito()
        .AddDefaultTemplate(o =>
        {
            o.UseLockScreen = false;
        });
});

var app = builder.Build();
app.UseBLambdaShell();

await app.RunAsync();
