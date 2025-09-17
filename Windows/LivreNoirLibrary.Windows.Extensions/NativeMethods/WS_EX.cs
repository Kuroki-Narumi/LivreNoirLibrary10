using System;

namespace LivreNoirLibrary.Windows.NativeMethods
{
    [Flags]
    public enum WS_EX : int
    {
        Left           = 0x0000_0000,
        LtrReading     = Left,
        RightScrollBar = Left,

        DlgModalFrame  = 0x0000_0001,
        NoParentNotify = 0x0000_0004,
        Topmost        = 0x0000_0008,

        AcceptFiles = 0x0000_0010,
        Transparent = 0x0000_0020,
        MdiChild    = 0x0000_0040,
        ToolWindow  = 0x0000_0080,

        WindowEdge  = 0x0000_0100,
        ClientEdge  = 0x0000_0200,
        ContextHelp = 0x0000_0400,

        Right         = 0x0000_1000,
        RtlReading    = 0x0000_2000,
        LeftScrollBar = 0x0000_4000,

        ControlParent = 0x0001_0000,
        StaticEdge    = 0x0002_0000,
        AppWindow     = 0x0004_0000,
        Layered       = 0x0008_0000,

        NoInheritLayout     = 0x0010_0000,
        NoRedirectionBitmap = 0x0020_0000,
        LayoutRtl           = 0x0040_0000,

        Composited = 0x0200_0000,
        NoActivate = 0x0800_0000,

        OverLappedWindow = WindowEdge | ClientEdge,
        PaletteWindow = WindowEdge | ToolWindow | Topmost,
    }
}
