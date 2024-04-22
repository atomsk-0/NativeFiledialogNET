using static NativeFileDialogNET.nfd;
using __IntPtr = System.IntPtr;

namespace NativeFileDialogNET;

public unsafe class NativeFileDialog : IDisposable
{
    public NativeFileDialog()
    {
        NFD_Init();
    }

    /// <summary>
    /// Opens a dialog to select a file or folder
    /// </summary>
    /// <param name="output">selected path</param>
    /// <returns>dialog result</returns>
    public DialogResult OpenDialog(out string output)
    {
        char* outputPtr;
        var result = NFD_OpenDialogN(&outputPtr, null, 0, null);
        output = new string(outputPtr);
        if (result == NfdresultT.NFD_OKAY)
        {
            NFD_FreePathN(outputPtr);
        }
        return (DialogResult)result;
    }

    /// <summary>
    /// Opens a dialog to select a file or folder with filters
    /// </summary>
    /// <param name="output">selected path</param>
    /// <param name="filters">dialog filters</param>
    /// <param name="defaultPath">default start path</param>
    /// <returns>dialog result</returns>
    public DialogResult OpenDialog(out string output, FilterItem[] filters, string? defaultPath = null)
    {
        char* outputPtr;
        NfdresultT result;

        var filtersNative = ConvertToNativeFilters(filters);

        fixed (FilterItemNative* filtersPtr = filtersNative)
        {
            result = __Internal.NFD_OpenDialogN(&outputPtr, (__IntPtr)filtersPtr, (uint)filters.Length, defaultPath);
        }

        output = new string(outputPtr);

        if (result == NfdresultT.NFD_OKAY)
        {
            NFD_FreePathN(outputPtr);
        }

        return (DialogResult)result;
    }

    private FilterItemNative[] ConvertToNativeFilters(FilterItem[] filters)
    {
        var filtersNative = new FilterItemNative[filters.Length];

        for (int i = 0; i < filters.Length; i++)
        {
            filtersNative[i].Name = GetPointerFromString(filters[i].Name);
            filtersNative[i].Spec = GetPointerFromString(filters[i].Spec);
        }

        return filtersNative;
    }

    private char* GetPointerFromString(string str)
    {
        fixed (char* ptr = str)
        {
            return ptr;
        }
    }
    
    public void Dispose()
    {
        NFD_Quit();
        GC.SuppressFinalize(this);
    }

    ~NativeFileDialog() => Dispose();
}
