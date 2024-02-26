using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Modularity;
using System.Diagnostics;
using Tyts.BLambda.Shell;
using Tyts.BLambda.Shell.Templates.Default;

[assembly: System.Runtime.Versioning.SupportedOSPlatformAttribute("browser")]
Debug.Assert(OperatingSystem.IsBrowser());

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


await builder.AddBLambdaShell(shell =>
{
    shell.AddDefaultTemplate();
});

var app = builder.Build();
app.UseBLambdaShell();


await app.RunAsync();

