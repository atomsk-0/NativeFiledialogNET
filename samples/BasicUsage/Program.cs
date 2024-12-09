using System.Text;
using NativeFileDialogNET;

using var selectFileDialog = new NativeFileDialog()
    .SelectFile()
    .AddFilter("Text Files", "*.txt") // Optionally add filters
    .AddFilter("All Files", "*.*") // Optionally add filters
    .AllowMultiple(); // Optionally allow multiple selections

DialogResult result = selectFileDialog.Open(out string[]? output, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
StringBuilder builder1 = new();
if (result == DialogResult.Okay && output != null)
{
    foreach (string file in output)
    {
        builder1.Append(file).Append(", ");
    }
}
Console.WriteLine(result == DialogResult.Okay ? $"Selected file(s): {builder1}" : "User canceled the dialog.");

using var selectFolderDialog = new NativeFileDialog()
    .SelectFolder()
    .AllowMultiple(); // Optionally allow multiple selections

result = selectFolderDialog.Open(out string[]? output2,  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
StringBuilder builder2 = new();
if (result == DialogResult.Okay && output2 != null)
{
    foreach (string file in output2)
    {
        builder2.Append(file).Append(", ");
    }
}
Console.WriteLine(result == DialogResult.Okay ? $"Selected folder(s): {builder2}" : "User canceled the dialog.");

using var saveFileDialog = new NativeFileDialog()
    .SaveFile()
    .AddFilter("Text Files", "*.txt")  // Optionally add filters
    .AddFilter("All Files", "*.*");  // Optionally add filters

result = saveFileDialog.Open(out string? saveFile, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DefaultName.txt");
Console.WriteLine(result == DialogResult.Okay ? $"Selected file: {saveFile}" : "User canceled the dialog.");