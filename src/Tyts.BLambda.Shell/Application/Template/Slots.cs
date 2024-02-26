using Tyts.BLambda.Blazor.Application.Template;

namespace Tyts.BLambda.Shell.Application.Template;

public static class Slots
{
    public sealed class NavMenuSlot : SlotAttribute { }
    public sealed class FooterToolbarSlot : SlotAttribute { }
    public sealed class HeaderToolbarSlot : SlotAttribute { }
    public sealed class HeaderTitleSlot : SlotAttribute { }
}
