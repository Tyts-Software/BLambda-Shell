using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using System.Reflection;
using Tyts.BLambda.Blazor.Application;
using Tyts.BLambda.Blazor.Application.Boot;
using Tyts.BLambda.Blazor.Application.Extension;
using Tyts.BLambda.Blazor.Application.Module;

namespace BLambda.Shall.Module;

// Assembly 1 --- 1 Module
internal sealed class ModulService : IModulService
{
    private readonly Task<BlazorBootConfiguration> blazorBootConfigurationLoader;
    private readonly LazyAssemblyLoader assemblyLoader;
    private readonly ILogger<ModulService> logger;
    private readonly HttpClient http;
    private readonly NavigationManager nav;
    private readonly IServiceCollection services;
    private readonly IWebAssemblyHostEnvironment env;
    private readonly IConfiguration configuration;
    private readonly IServiceProvider serviceProvider;

    private readonly AsyncCache<string, ModuleInfo> loadedModules;
    private readonly Lazy<Task<IEnumerable<string>>> lazyModules;

    public ModulService(
        ILogger<ModulService> logger,
        HttpClient http,
        NavigationManager nav,
        LazyAssemblyLoader assemblyLoader,
        Task<BlazorBootConfiguration> loadBlazorBootConfiguration,
        IServiceCollection services,
        IWebAssemblyHostEnvironment env,
        IConfiguration configuration,
        IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.http = http;
        this.nav = nav;

        this.blazorBootConfigurationLoader = loadBlazorBootConfiguration;
        this.assemblyLoader = assemblyLoader;
        this.services = services;
        this.env = env;
        this.configuration = configuration;
        this.serviceProvider = serviceProvider;

        lazyModules = new Lazy<Task<IEnumerable<string>>>(async () =>
        {
            var config = await blazorBootConfigurationLoader;
            return config?.Resources?.LazyAssembly?.Keys?
                .Where(k => k.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
                ?? Enumerable.Empty<string>();
        });

        LazyInitializer.EnsureInitialized<AsyncCache<string, ModuleInfo>>(ref loadedModules, Initialize);
    }

    public event Action<string>? ModuleLoaded;

    public Task<ModuleInfo> GetModule(string name)
    {
        return loadedModules[name];
    }

    public async Task<IEnumerable<ModuleInfo>> GetModules()
    {
        return await loadedModules.Values;
    }

    public async Task<IEnumerable<Assembly>> GetLoadedAssemblies()
    {
        return (await loadedModules.Values)
            .Where(m => m.IsLoaded)
            .Select(m => m.Assembly!);
    }

    public async Task<IEnumerable<Type>> GetComponentsByName(string name)
    {
        return (await loadedModules.Values)
            .Where(m => m.Components is not null)
            .SelectMany(m => m.Components!)
            .Where(c => c.Item1.EndsWith($".{name}"))
            .Select(c => c.Item2)
            .ToArray();
    }

    public async Task<IEnumerable<Type>> GetComponentsBySlot<T>()
    {
        return (await loadedModules.Values)
            .Where(m => m.Components is not null)
            .SelectMany(m => m.Components!)
            .Where(c => c.Item2.HasAttribute<T>())
            .Select(c => c.Item2)
            .ToArray();
    }

    public void LoadModules(IEnumerable<string> modulesToLoad)
    {
        var newModulesToLoad = modulesToLoad.Except(loadedModules.Keys);

        // load modules
        loadedModules.AddRange(Task.FromResult(newModulesToLoad), (fileName) => ModuleLoaded?.Invoke(fileName));
    }

    // for now it is used as for NotLazyModules are not loaded yet when Initialize happens
    // todo: but it doesn't work
    public async Task EnsureNotLazyModulesLoaded()
    {
        await foreach (var assembly in GetNotLazyModules())
        {
            loadedModules.Add(assembly.ManifestModule.Name, (_) => Task.Run(() => new ModuleInfo(assembly)), (fileName) => ModuleLoaded?.Invoke(fileName));
        }
    }

    private AsyncCache<string, ModuleInfo> Initialize()
    {
        var modulesCache = new AsyncCache<string, ModuleInfo>(ModuleLoader);

        modulesCache.AddRange(lazyModules.Value, (fileName) => ModuleLoaded?.Invoke(fileName));

        //IEnumerable<string> availableModules = await ;

        //IEnumerable<string> modulesToLoad = availableModules;

        //await LoadModules(modulesToLoad);

        return modulesCache;
    }

    private async IAsyncEnumerable<Assembly> GetNotLazyModules()
    {
        var lazy = (await lazyModules.Value).AsQueryable();
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (a.GetCustomAttributes<ModuleAttribute>().Any()
                        && !lazy.Contains(a.ManifestModule.Name))
            {
                yield return a;
            }
        }
    }

    private async Task<IEnumerable<string>> GetLazyModules() => await lazyModules.Value;

    private Task<ModuleInfo> ModuleLoader(string fileName)
    {
        return assemblyLoader.LoadAssembliesAsync(new[] { fileName }).ContinueWith<ModuleInfo>(task =>
        {
            var name = fileName[..^4];
            var assemblies = task.Result
                .Where(a => a.GetCustomAttributes<ModuleAttribute>().Any())
                .ToList();

            foreach (var assembly in assemblies)
            {
                // for those should be started up
                var tobeConfigured = assembly.ExportedTypes.SingleOrDefault(t => typeof(IStartup).IsAssignableFrom(t));
                if (tobeConfigured is not null)
                {
                    var startup = ActivatorUtilities.CreateInstance(serviceProvider, tobeConfigured) as IStartup;
                    //var startup = Activator.CreateInstance(tobeConfigured, configuration, env) as IStartup;
                    startup?.ConfigureServices(services);
                }

                return new ModuleInfo(assembly);
            }

            return new ModuleInfo(name);

        }, TaskContinuationOptions.OnlyOnRanToCompletion);
    }
}
