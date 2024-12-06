using System.Runtime.InteropServices;

namespace NativeFileDialogNET;

public unsafe class NativeFileDialog : IDisposable
{
    private bool initialized;

    private DialogMode dialogMode;
    private readonly List<FilterItem> filterItems = [];

    public NativeFileDialog()
    {
        if (Bindings.Init() == NfdResult.Okay)
        {
            initialized = true;
        }
    }

    // To not break changes as Open with string[] would be now the main method to use
    public DialogResult Open(out string? output, string? defaultPath = null, string? defaultName = null)
    {
        DialogResult result = Open(out string[]? outputs, defaultPath, defaultName);
        output = outputs?[0];
        return result;
    }

    public DialogResult Open(out string[]? output, string? defaultPath = null, string? defaultName = null)
    {
        output = null;
        if (initialized == false) throw new InvalidOperationException("NativeFileDialog is not initialized.");
        switch (dialogMode)
        {
            case DialogMode.SelectFile:
            {
                NfdResult result;
                NfdU8FilterItem[] nativeFilterItems = ConvertFilterItemsToNative(CollectionsMarshal.AsSpan(filterItems));
                sbyte* selectedPath = null;
                fixed(NfdU8FilterItem* filterItemsPtr = nativeFilterItems)
                {
                    sbyte* defaultPathPtr = (sbyte*)NativeMemoryUtils.StringToUTF8Ptr(defaultPath);
                    result = Bindings.OpenDialog(&selectedPath, filterItemsPtr, (uint)nativeFilterItems.Length, defaultPathPtr);
                    if (result == NfdResult.Okay)
                    {
                        string? rt = NativeMemoryUtils.PtrToUTF8String((byte*)selectedPath);
                        if (rt != null) output = [rt];
                        Bindings.FreePath(selectedPath);
                    }
                    NativeMemoryUtils.Free(defaultPathPtr);
                }
                foreach (var item in nativeFilterItems)
                {
                    NativeMemoryUtils.Free(item.Name);
                    NativeMemoryUtils.Free(item.Spec);
                }
                return mapResult(result);
            }
            case DialogMode.SelectMultipleFiles:
            {
                NfdResult result;
                NfdU8FilterItem[] nativeFilterItems = ConvertFilterItemsToNative(CollectionsMarshal.AsSpan(filterItems));
                void* selectedPaths = null;
                fixed(NfdU8FilterItem* filterItemsPtr = nativeFilterItems)
                {
                    sbyte* defaultPathPtr = (sbyte*)NativeMemoryUtils.StringToUTF8Ptr(defaultPath);
                    result = Bindings.OpenDialogMultiple(&selectedPaths, filterItemsPtr, (uint)nativeFilterItems.Length, defaultPathPtr);
                    if (result == NfdResult.Okay)
                    {
                        int numPaths;
                        Bindings.PathSetGetCount(selectedPaths, &numPaths);

                        List<string> strings = [];
                        for (int i = 0; i < numPaths; i++)
                        {
                            sbyte* path = null;
                            Bindings.PathSetGetPath(selectedPaths, i, &path);
                            string? rt = NativeMemoryUtils.PtrToUTF8String((byte*)path);
                            if (rt != null) strings.Add(rt);
                            Bindings.PathSetFreePath(path);
                        }
                        if (strings.Count > 0)
                        {
                            output = strings.ToArray();
                        }
                        Bindings.PathSetFreePath(selectedPaths);
                    }
                    NativeMemoryUtils.Free(defaultPathPtr);
                }
                foreach (var item in nativeFilterItems)
                {
                    NativeMemoryUtils.Free(item.Name);
                    NativeMemoryUtils.Free(item.Spec);
                }
                return mapResult(result);
            }
            case DialogMode.SelectFolder:
            {
                sbyte* selectedPath = null;
                sbyte* defaultPathPtr = (sbyte*)NativeMemoryUtils.StringToUTF8Ptr(defaultPath);
                NfdResult result = Bindings.PickFolder(&selectedPath, defaultPathPtr);
                if (result == NfdResult.Okay)
                {
                    string? rt = NativeMemoryUtils.PtrToUTF8String((byte*)selectedPath);
                    if (rt != null) output = [rt];
                    Bindings.FreePath(selectedPath);
                }
                NativeMemoryUtils.Free(defaultPathPtr);
                return mapResult(result);
            }
            case DialogMode.SelectMultipleFolders:
            {
                void* selectedPaths = null;
                sbyte* defaultPathPtr = (sbyte*)NativeMemoryUtils.StringToUTF8Ptr(defaultPath);
                NfdResult result = Bindings.PickFolderMultiple(&selectedPaths, defaultPathPtr);
                if (result == NfdResult.Okay)
                {
                    int numPaths;
                    Bindings.PathSetGetCount(selectedPaths, &numPaths);

                    List<string> strings = [];
                    for (int i = 0; i < numPaths; i++)
                    {
                        sbyte* path = null;
                        Bindings.PathSetGetPath(selectedPaths, i, &path);
                        string? rt = NativeMemoryUtils.PtrToUTF8String((byte*)path);
                        if (rt != null) strings.Add(rt);
                        Bindings.PathSetFreePath(path);
                    }
                    if (strings.Count > 0)
                    {
                        output = strings.ToArray();
                    }
                    Bindings.PathSetFreePath(selectedPaths);
                }
                NativeMemoryUtils.Free(defaultPathPtr);
                return mapResult(result);
            }
            case DialogMode.SaveFile:
            {
                NfdResult result;
                NfdU8FilterItem[] nativeFilterItems = ConvertFilterItemsToNative(CollectionsMarshal.AsSpan(filterItems));
                sbyte* selectedPath = null;
                fixed(NfdU8FilterItem* filterItemsPtr = nativeFilterItems)
                {
                    sbyte* defaultPathPtr = (sbyte*)NativeMemoryUtils.StringToUTF8Ptr(defaultPath);
                    sbyte* defaultNamePtr = (sbyte*)NativeMemoryUtils.StringToUTF8Ptr(defaultName);
                    result = Bindings.SaveDialog(&selectedPath, filterItemsPtr, (uint)nativeFilterItems.Length, defaultPathPtr, defaultNamePtr);
                    if (result == NfdResult.Okay)
                    {
                        string? rt = NativeMemoryUtils.PtrToUTF8String((byte*)selectedPath);
                        if (rt != null) output = [rt];
                        Bindings.FreePath(selectedPath);
                    }
                    NativeMemoryUtils.Free(defaultPathPtr);
                    NativeMemoryUtils.Free(defaultNamePtr);
                }
                foreach (var item in nativeFilterItems)
                {
                    NativeMemoryUtils.Free(item.Name);
                    NativeMemoryUtils.Free(item.Spec);
                }
                return mapResult(result);
            }
        }
        return DialogResult.Error;

        static DialogResult mapResult(NfdResult result)
            => result switch
            {
                NfdResult.Okay => DialogResult.Okay,
                NfdResult.Cancel => DialogResult.Cancel,
                _ => DialogResult.Error,
            };
    }

    public NativeFileDialog SaveFile()
    {
        dialogMode = DialogMode.SaveFile;
        filterItems.Clear();
        return this;
    }

    public NativeFileDialog SelectFile()
    {
        dialogMode = DialogMode.SelectFile;
        filterItems.Clear();
        return this;
    }

    public NativeFileDialog SelectFolder()
    {
        dialogMode = DialogMode.SelectFolder;
        filterItems.Clear();
        return this;
    }

    public NativeFileDialog AllowMultiple()
    {
        dialogMode = dialogMode == DialogMode.SelectFolder ? DialogMode.SelectMultipleFolders : DialogMode.SelectMultipleFiles;
        filterItems.Clear();
        return this;
    }

    public NativeFileDialog AddFilter(string name, string spec)
    {
        filterItems.Add(new FilterItem(name, spec));
        return this;
    }

    public void Dispose()
    {
        if (initialized)
        {
            Bindings.Quit();
            initialized = false;
        }
        filterItems.Clear();
        GC.SuppressFinalize(this);
    }

    internal static NfdU8FilterItem[] ConvertFilterItemsToNative(in Span<FilterItem> filterItems)
    {
        NfdU8FilterItem[] nativeFilterItems = new NfdU8FilterItem[filterItems.Length];
        for (int i = 0; i < filterItems.Length; i++)
        {
            nativeFilterItems[i] = new NfdU8FilterItem
            {
                Name = (sbyte*)NativeMemoryUtils.StringToUTF8Ptr(filterItems[i].Name),
                Spec = (sbyte*)NativeMemoryUtils.StringToUTF8Ptr(filterItems[i].Spec)
            };
        }
        return nativeFilterItems;
    }

    ~NativeFileDialog() => Dispose();
}

public readonly struct FilterItem(string name, string spec)
{
    public readonly string Name = name;
    public readonly string Spec = spec;
}

public enum DialogResult
{
    Error,
    Okay,
    Cancel,
}

internal enum DialogMode
{
    SelectFile,
    SelectMultipleFiles,
    SelectFolder,
    SelectMultipleFolders,
    SaveFile,
}