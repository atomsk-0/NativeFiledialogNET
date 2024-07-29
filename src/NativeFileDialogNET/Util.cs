using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace NativeFileDialogNET;

internal static unsafe class Util
{
    internal static NfdU8FilterItem[] ConvertFilterItemsToNative(in Span<FilterItem> filterItems)
    {
        NfdU8FilterItem[] nativeFilterItems = new NfdU8FilterItem[filterItems.Length];
        for (int i = 0; i < filterItems.Length; i++)
        {
            nativeFilterItems[i] = new NfdU8FilterItem
            {
                Name = ConvertStringToPointer(filterItems[i].Name),
                Spec = ConvertStringToPointer(filterItems[i].Spec)
            };
        }
        return nativeFilterItems;
    }

    internal static sbyte* ConvertStringToPointer(string? str)
    {
        if (str == null) return null;
        int length = Encoding.UTF8.GetByteCount(str) + 1;
        sbyte* result = (sbyte*)NativeMemory.Alloc((nuint)length);
        for (int i = 0; i < str.Length; i++)
        {
            result[i] = (sbyte)str[i];
        }
        result[length - 1] = 0;
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void FreeStringPointer(sbyte* ptr)
    {
        if (ptr == null) return;
        NativeMemory.Free(ptr);
    }
}