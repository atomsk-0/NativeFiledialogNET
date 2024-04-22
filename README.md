# NativeFileDialogNET [WIP]

NativeFileDialogNET is a C# binding for the [nativefiledialog-extended](https://github.com/btzy/nativefiledialog-extended) library. This project allows you to use native file dialog functionalities in your C# applications.

## Usage

Here is a simple example of how to use the library:

```csharp
using NativeFileDialogNET;

// Create a new instance of the dialog
var dialog = new NativeFileDialog();

// Open a file dialog
DialogResult result = dialog.OpenDialog(out string output);

// Check the result
if (result == DialogResult.Okay)
{
    Console.WriteLine($"Selected file: {output}");
}
else if (result == DialogResult.Cancel)
{
    Console.WriteLine("User cancelled the dialog");
}
else
{
    Console.WriteLine("An error occurred");
}
```
Contributions are welcome! Please feel free to submit a pull request.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
