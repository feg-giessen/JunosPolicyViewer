using System;

namespace JunosPolicyViewer.Junos
{
    [Flags]
    public enum PolicyLog
    {
        SessionInit,
        SessionEnd
    }
}