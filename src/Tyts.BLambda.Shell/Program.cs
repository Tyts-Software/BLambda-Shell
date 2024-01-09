using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Tyts.BLambda.Shell;

Debug.Assert(OperatingSystem.IsBrowser());

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var http = new HttpClient()
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
};
builder.Services.AddSingleton(sp => http);

builder.Services.AddCascadingAuthenticationState();

//builder.Services.AddFluentUIComponents();
await builder.AddShell();

var app = builder.Build();
await app.RunAsync();
