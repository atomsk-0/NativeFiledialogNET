# NativeFileDialogNET

NativeFileDialogNET is a C# binding for the [nativefiledialog-extended](https://github.com/btzy/nativefiledialog-extended) library. This project allows you to use native file dialog functionalities in your C# applications.

## Usage

Here is a simple example of how to use the library:

```csharp
using NativeFileDialogNET;

using var selectFileDialog = new NativeFileDialog()
    .SelectFile()
    .AddFilter("Text Files", "*.txt") // Optionally add filters
    .AddFilter("All Files", "*.*");  // Optionally add filters

DialogResult result = selectFileDialog.Open(out string? output, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
Console.WriteLine(result == DialogResult.Okay ? $"Selected file: {output}" : "User canceled the dialog.");

using var selectFolderDialog = new NativeFileDialog()
    .SelectFolder();

result = selectFolderDialog.Open(out string? folder,  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
Console.WriteLine(result == DialogResult.Okay ? $"Selected folder: {folder}" : "User canceled the dialog.");

using var saveFileDialog = new NativeFileDialog()
    .SaveFile()
    .AddFilter("Text Files", "*.txt")  // Optionally add filters
    .AddFilter("All Files", "*.*");  // Optionally add filters

result = saveFileDialog.Open(out string? saveFile, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DefaultName.txt");
Console.WriteLine(result == DialogResult.Okay ? $"Selected file: {saveFile}" : "User canceled the dialog.");
```

Contributions are welcome! Please feel free to submit a pull request.

## License

This project is licensed under the zlib License. See the LICENSE file for details.
