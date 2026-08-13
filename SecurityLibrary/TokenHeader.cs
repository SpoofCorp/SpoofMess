using System.Runtime.InteropServices;

namespace SecurityLibrary;
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TokenHeader
{
    public ushort Magic { get; set; }
    public byte Version { get; set; }
    public int BodySize { get; set; }
}
