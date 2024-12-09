using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace NativeFileDialogNET;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static unsafe class NativeMemoryUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static T* Alloc<T>(nuint size) where T : unmanaged => (T*)NativeMemory.Alloc(size * (nuint)sizeof(T));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void Free(void* ptr) => NativeMemory.Free(ptr);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetByteCountUTF8(string str)
    {
        return Encoding.UTF8.GetByteCount(str);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetByteCountUTF16(string str)
    {
        return Encoding.Unicode.GetByteCount(str);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static byte* StringToUTF8Ptr(string? str)
    {
        if (str == null) return null;
        int size = GetByteCountUTF8(str);
        byte* ptr = Alloc<byte>((nuint)(size + 1));
        fixed (char* pStr = str)
        {
            Encoding.UTF8.GetBytes(pStr, str.Length, ptr, size);
        }
        ptr[size] = 0;
        return ptr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string? PtrToUTF8String(byte* ptr)
    {
        if (ptr == null) return null;
        int length = 0;
        while (ptr[length] != 0) length++;
        return Encoding.UTF8.GetString(ptr, length);
    }
}