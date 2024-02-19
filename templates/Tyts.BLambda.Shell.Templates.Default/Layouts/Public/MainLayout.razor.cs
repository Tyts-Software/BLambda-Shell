using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Tyts.BLambda.Shell.Templates.Default.Layouts.Public;

public partial class MainLayout : LayoutComponentBase
{
    private ErrorBoundary? errorBoundary;

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        errorBoundary?.Recover();
    }
}