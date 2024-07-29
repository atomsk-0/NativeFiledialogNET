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

    public DialogResult Open(out string? output, string? defaultPath = null, string? defaultName = null)
    {
        output = null;
        if (initialized == false) throw new InvalidOperationException("NativeFileDialog is not initialized.");
        switch (dialogMode)
        {
            case DialogMode.SelectFile:
            {
                NfdResult result;
                NfdU8FilterItem[] nativeFilterItems = Util.ConvertFilterItemsToNative(CollectionsMarshal.AsSpan(filterItems));
                sbyte* selectedPath = null;
                fixed(NfdU8FilterItem* filterItemsPtr = nativeFilterItems)
                {
                    sbyte* defaultPathPtr = Util.ConvertStringToPointer(defaultPath);
                    result = Bindings.OpenDialog(&selectedPath, filterItemsPtr, (uint)nativeFilterItems.Length, defaultPathPtr);
                    if (result == NfdResult.Okay)
                    {
                        output = Marshal.PtrToStringUTF8((IntPtr)selectedPath);
                        Bindings.FreePath(selectedPath);
                    }
                    Util.FreeStringPointer(defaultPathPtr);
                }
                foreach (var item in nativeFilterItems)
                {
                    Util.FreeStringPointer(item.Name);
                    Util.FreeStringPointer(item.Spec);
                }
                return result switch
                {
                    NfdResult.Okay => DialogResult.Okay,
                    NfdResult.Cancel => DialogResult.Cancel,
                    _ => DialogResult.Error,
                };
            }
            case DialogMode.SelectFolder:
            {
                sbyte* selectedPath = null;
                sbyte* defaultPathPtr = Util.ConvertStringToPointer(defaultPath);
                NfdResult result = Bindings.PickFolder(&selectedPath, defaultPathPtr);
                if (result == NfdResult.Okay)
                {
                    output = Marshal.PtrToStringUTF8((IntPtr)selectedPath);
                    Bindings.FreePath(selectedPath);
                }
                Util.FreeStringPointer(defaultPathPtr);
                break;
            }
            case DialogMode.SaveFile:
            {
                NfdResult result;
                NfdU8FilterItem[] nativeFilterItems = Util.ConvertFilterItemsToNative(CollectionsMarshal.AsSpan(filterItems));
                sbyte* selectedPath = null;
                fixed(NfdU8FilterItem* filterItemsPtr = nativeFilterItems)
                {
                    sbyte* defaultPathPtr = Util.ConvertStringToPointer(defaultPath);
                    sbyte* defaultNamePtr = Util.ConvertStringToPointer(defaultName);
                    result = Bindings.SaveDialog(&selectedPath, filterItemsPtr, (uint)nativeFilterItems.Length, defaultPathPtr, defaultNamePtr);
                    if (result == NfdResult.Okay)
                    {
                        output = Marshal.PtrToStringUTF8((IntPtr)selectedPath);
                        Bindings.FreePath(selectedPath);
                    }
                    Util.FreeStringPointer(defaultPathPtr);
                    Util.FreeStringPointer(defaultNamePtr);
                }
                foreach (var item in nativeFilterItems)
                {
                    Util.FreeStringPointer(item.Name);
                    Util.FreeStringPointer(item.Spec);
                }
                return result switch
                {
                    NfdResult.Okay => DialogResult.Okay,
                    NfdResult.Cancel => DialogResult.Cancel,
                    _ => DialogResult.Error,
                };
            }
        }
        return DialogResult.Error;
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
    SelectFolder,
    SaveFile,
}