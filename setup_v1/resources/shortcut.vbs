' Set the path of the .exe file and the shortcut location
Dim objShell, objShortcut
Dim exePath, shortcutPath, shortcutName, currentDirectory

' Create the FileSystemObject
Set objFSO = CreateObject("Scripting.FileSystemObject")

' Get the current directory
currentDirectory = objFSO.GetAbsolutePathName(".")

' Path to your executable file
exePath = currentDirectory & "\mark_of_idle.exe"  ' Correct path concatenation

' Where you want to create the shortcut (e.g., Start Menu or Desktop)
shortcutName = "Mark of Idle"  ' The name of the shortcut

' Create the shortcut path (Start Menu for this example)
shortcutPath = CreateObject("WScript.Shell").SpecialFolders("Programs") & "\" & shortcutName & ".lnk"

' Create the WScript.Shell object
Set objShell = CreateObject("WScript.Shell")

' Create the shortcut
Set objShortcut = objShell.CreateShortcut(shortcutPath)

' Set properties of the shortcut
objShortcut.TargetPath = exePath  ' Set the path to your .exe file
objShortcut.WorkingDirectory = currentDirectory  ' Set the working directory to the current directory
objShortcut.IconLocation = exePath  ' Use the icon of the .exe file
objShortcut.Save  ' Save the shortcut

' Clean up
Set objShortcut = Nothing
Set objShell = Nothing
Set objFSO = Nothing

