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
    /// Opens a save dialog with specified filters, default path and default name.
    /// </summary>
    /// <param name="output">The output path selected by the user.</param>
    /// <param name="filters">The filters for the dialog.</param>
    /// <param name="defaultPath">The default path for the dialog.</param>
    /// <param name="defaultName">The default name for the file.</param>
    /// <returns>The result of the dialog operation.</returns>
    public DialogResult OpenSaveDialog(out string output, FilterItem[] filters, string defaultPath, string defaultName)
    {
        char* outputPtr;
        NfdresultT result;
        var filtersNative = ConvertToNativeFilters(filters);

        fixed (FilterItemNative* filtersPtr = filtersNative)
        {
            result = __Internal.NFD_SaveDialogN(&outputPtr, (__IntPtr)filtersPtr, (uint)filters.Length, defaultPath, defaultName);
        }
        
        output = new string(outputPtr);
        if (result == NfdresultT.NFD_OKAY)
        {
            NFD_FreePathN(outputPtr);
        }
        return (DialogResult)result;
    }

    /// <summary>
    /// Opens a save dialog with a specified default path and name.
    /// </summary>
    /// <param name="output">The output path selected by the user.</param>
    /// <param name="defaultPath">The default path for the dialog.</param>
    /// <param name="defaultName">The default name for the file.</param>
    /// <returns>The result of the dialog operation.</returns>
    public DialogResult OpenSaveDialog(out string output, string defaultPath, string defaultName)
    {
        char* outputPtr;
        var result = NFD_SaveDialogN(&outputPtr, null, 0, defaultPath, defaultName);
        output = new string(outputPtr);
        if (result == NfdresultT.NFD_OKAY)
        {
            NFD_FreePathN(outputPtr);
        }
        return (DialogResult)result;
    }
    
    
    /// <summary>
    /// Opens a save dialog with a specified default name.
    /// </summary>
    /// <param name="output">The output path selected by the user.</param>
    /// <param name="defaultName">The default name for the file.</param>
    /// <returns>The result of the dialog operation.</returns>
    public DialogResult OpenSaveDialog(out string output, string defaultName)
    {
        char* outputPtr;
        var result = NFD_SaveDialogN(&outputPtr, null, 0, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), defaultName);
        output = new string(outputPtr);
        if (result == NfdresultT.NFD_OKAY)
        {
            NFD_FreePathN(outputPtr);
        }
        return (DialogResult)result;
    }
    
    
    /// <summary>
    /// Opens a save dialog.
    /// </summary>
    /// <param name="output">The output path selected by the user.</param>
    /// <returns>The result of the dialog operation.</returns>
    public DialogResult OpenSaveDialog(out string output)
    {
        char* outputPtr;
        var result = NFD_SaveDialogN(&outputPtr, null, 0, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "");
        output = new string(outputPtr);
        if (result == NfdresultT.NFD_OKAY)
        {
            NFD_FreePathN(outputPtr);
        }
        return (DialogResult)result;
    }

    /// <summary>
    /// Opens a select dialog.
    /// </summary>
    /// <param name="output">The output path selected by the user.</param>
    /// <returns>The result of the dialog operation.</returns>
    public DialogResult OpenSelectDialog(out string output)
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
    /// Opens a select dialog with specified filters and a default path.
    /// </summary>
    /// <param name="output">The output path selected by the user.</param>
    /// <param name="filters">The filters for the dialog.</param>
    /// <param name="defaultPath">The default path for the dialog.</param>
    /// <returns>The result of the dialog operation.</returns>
    public DialogResult OpenSelectDialog(out string output, FilterItem[] filters, string? defaultPath = null)
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


    /// <summary>
    /// Opens a select dialog.
    /// </summary>
    /// <param name="output">The output path selected by the user.</param>
    /// <returns>The result of the dialog operation.</returns>
    [Obsolete("Use scoped methods instead (OpenSelectDialog, OpenSaveDialog)")]
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
    /// Opens a select dialog with specified filters and a default path.
    /// </summary>
    /// <param name="output">The output path selected by the user.</param>
    /// <param name="filters">The filters for the dialog.</param>
    /// <param name="defaultPath">The default path for the dialog.</param>
    /// <returns>The result of the dialog operation.</returns>
    [Obsolete("Use scoped methods instead (OpenSelectDialog, OpenSaveDialog)")]
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
