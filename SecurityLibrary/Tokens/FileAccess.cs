using System.Runtime.InteropServices;

namespace SecurityLibrary.Tokens;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FileAccess
{
    public Guid Mask;
}