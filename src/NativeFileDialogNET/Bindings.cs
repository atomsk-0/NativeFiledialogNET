using System.Runtime.InteropServices;

namespace NativeFileDialogNET;

internal static unsafe  partial class Bindings
{
    private const string library = "nfd";

    [LibraryImport(library, EntryPoint = "NFD_Init")]
    internal static partial NfdResult Init();

    [LibraryImport(library, EntryPoint = "NFD_Quit")]
    public static partial void Quit();

    [LibraryImport(library, EntryPoint = "NFD_OpenDialogU8")]
    public static partial NfdResult OpenDialog(sbyte** outPath, NfdU8FilterItem* filterList, uint filterCount, sbyte* defaultPath);

    [LibraryImport(library, EntryPoint = "NFD_PickFolderU8")]
    public static partial NfdResult PickFolder(sbyte** outPath, sbyte* defaultPath);

    [LibraryImport(library, EntryPoint = "NFD_SaveDialogU8")]
    public static partial NfdResult SaveDialog(sbyte** outPath, NfdU8FilterItem* filterList, uint filterCount, sbyte* defaultPath, sbyte* defaultName);

    [LibraryImport(library, EntryPoint = "NFD_FreePathU8")]
    public static partial void FreePath(sbyte* path);
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct NfdU8FilterItem
{
    public sbyte* Name;
    public sbyte* Spec;
}

internal enum NfdResult
{
    Error, // Programmatic error
    Okay, // User pressed okay, or successful return
    Cancel // User pressed cancel
}