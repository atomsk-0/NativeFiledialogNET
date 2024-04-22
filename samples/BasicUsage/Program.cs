using NativeFileDialogNET;

using var dialog = new NativeFileDialog();
var result = dialog.OpenDialog(out string output, [new FilterItem { Name = "Text files", Spec = "txt" }]);
Console.WriteLine($"{result}, {output}");