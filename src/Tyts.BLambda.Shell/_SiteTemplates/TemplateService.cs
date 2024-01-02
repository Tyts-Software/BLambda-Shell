﻿using Tyts.BLambda.Blazor.Application.Template;
using Tyts.BLambda.Shell._SiteTemplates.Default;
using Tyts.BLambda.Shell._SiteTemplates.Default.Anonymous;

namespace Tyts.BLambda.Shell._SiteTemplates;

internal class TemplateService : ITemplateService
{
    private readonly List<TemplateDescriptor> templates = new()
    {
        new ("Default", typeof(DefaultLayout), typeof(AnonymousLayout))
    };

    public TemplateDescriptor Current => templates.First();
}
