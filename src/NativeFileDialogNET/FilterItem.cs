using System.Runtime.InteropServices;

namespace NativeFileDialogNET;

public struct FilterItem
{
    public string Name;
    public string Spec;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FilterItemNative
{
    public char* Name;
    public char* Spec;
}