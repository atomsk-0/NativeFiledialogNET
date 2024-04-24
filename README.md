# NativeFileDialogNET

NativeFileDialogNET is a C# binding for the [nativefiledialog-extended](https://github.com/btzy/nativefiledialog-extended) library. This project allows you to use native file dialog functionalities in your C# applications.

## Usage

Here is a simple example of how to use the library:

```csharp
using NativeFileDialogNET;

// Create a new instance of the dialog
using var dialog = new NativeFileDialog();

// Open a select file dialog
var selectResult = dialog.OpenSelectDialog(out string selectOutput, [new FilterItem { Name = "Text files", Spec = "txt" }]);

// Check the result
switch (selectResult)
{
    case DialogResult.Okay:
        Console.WriteLine($"Selected file: {selectOutput}");
        break;
    case DialogResult.Cancel:
        Console.WriteLine("User cancelled the dialog");
        break;
    default:
        Console.WriteLine("An error occurred");
        break;
}

// Open a save file dialog
var saveResult = dialog.OpenSaveDialog(out string saveOutput, [new FilterItem { Name = "Text files", Spec = "txt" }], Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "test.txt");

// Check the result
switch (saveResult)
{
    case DialogResult.Okay:
        Console.WriteLine($"Saved file path: {saveOutput}");
        break;
    case DialogResult.Cancel:
        Console.WriteLine("User cancelled the dialog");
        break;
    default:
        Console.WriteLine("An error occurred");
        break;
}
```

Contributions are welcome! Please feel free to submit a pull request.

## License

This project is licensed under the zlib License. See the LICENSE file for details.
