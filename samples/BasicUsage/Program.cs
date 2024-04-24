using NativeFileDialogNET;

using var dialog = new NativeFileDialog();
var selectResult = dialog.OpenSelectDialog(out string selectOutput, [new FilterItem { Name = "Text files", Spec = "txt" }]);
Console.WriteLine($"{selectResult}, {selectOutput}");

var saveResult = dialog.OpenSaveDialog(out string saveOutput, [new FilterItem { Name = "Text files", Spec = "txt" }], Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "test.txt");
Console.WriteLine($"{saveResult}, {saveOutput}");