using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Tyts.BLambda.Shell._SiteTemplates.Default;

public partial class DefaultLayout : LayoutComponentBase
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