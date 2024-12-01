using NativeFileDialogNET;

// This links the native dialog library statically to the executable. see csproj for more details.

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