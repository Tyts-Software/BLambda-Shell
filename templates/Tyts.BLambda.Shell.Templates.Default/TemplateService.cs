using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tyts.BLambda.Blazor.Application.Template;

namespace Tyts.BLambda.Shell.Templates.Default;

internal class TemplateService : ITemplateService
{
    private readonly bool _useLockScreen;
    private readonly TemplateDescriptor _current;
    private readonly AuthenticationStateProvider? _authStateProvider;

    public TemplateService(
        IOptions<DefaultTemplateOptions> options, 
        IServiceProvider serviceProvider)
    {
        _authStateProvider = serviceProvider.GetService<AuthenticationStateProvider>();
        _useLockScreen = options.Value.UseLockScreen;
        _current = new("Default", typeof(Layouts.Public.MainLayout), typeof(Layouts.LockScreen.MainLayout));
    }

    public TemplateDescriptor Current => _current;

    public async Task<Type> GetLayout() 
    {
        var isAuthenticated = false;

        if (_authStateProvider is not null)
        {
            var auth = await _authStateProvider.GetAuthenticationStateAsync();
            isAuthenticated = auth.User.Identity?.IsAuthenticated ?? false;
        }        

        return !isAuthenticated && _useLockScreen
            ? _current.LockScreenLayout
            : _current.DefaultLayout;
    }
}
